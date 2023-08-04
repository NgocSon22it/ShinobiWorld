using Assets.Scripts.Database.DAO;
using Assets.Scripts.GameManager;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossArena_Manager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [Header("Set Up")]
    [SerializeField] Canvas sortCanvas;
    [SerializeField] Transform SpawnPoint;
    [SerializeField] GameObject Boss;
    [SerializeField] GameObject BossPool;
    [SerializeField] PolygonCollider2D CameraBox;

    [SerializeField] List<GameObject> ListBoss = new List<GameObject>();
    [SerializeField] List<GameObject> ListBossPool = new List<GameObject>();

    [Header("Battle Time")]
    float TotalTime = 180f, currentTime;
    [SerializeField] TMP_Text Battle_Fight_CountdownTxt;
    [SerializeField] GameObject ReadyBase;

    [Header("Battle Start")]
    float TotalProgress = 1f, CurrentProgress = 0f, ReadyTime = 3f;
    bool BattleStart, ProgressRun;
    Coroutine ProgressBar_Coroutine;
    [SerializeField] GameObject ProgressBar;
    [SerializeField] Image CurrentProgressBar;
    [SerializeField] TMP_Text Battle_Start_CountdownTxt;
    public int RequireNumber, CurrentNumber;

    [Header("Battle End")]
    [SerializeField] GameObject Battle_End_Panel;
    [SerializeField] TMP_Text Battle_End_Text;

    bool BattleEnd;

    [Header("Player Instance")]
    [SerializeField] GameObject LoadingPrefabs;
    [SerializeField] Sprite LoadingImage;
    GameObject LoadingInstance;

    private const byte ShowEndgamePanelEventCode = 1;
    private const byte ActiveBossEventCode = 2;
    private const byte BattleStart_CheckReady = 3;
    private const byte BattleStart_InitBoss = 4;

    private const string EndGamePro = "EndGame";

    [Header("Room Value")]
    [SerializeField] MapType mapType;
    BossArenaType arenaType;
    string BossName;
    RoomOptions roomOptions = new RoomOptions();
    PlayerBase[] players;

    [Header("JoinRoom Failed")]
    [SerializeField] GameObject JoinRoomFailedPrefabs;
    GameObject JoinRoomFailedInstance;

    public static BossArena_Manager Instance;

    private void Start()
    {
        LoadingInstance = Instantiate(LoadingPrefabs);
        LoadingInstance.GetComponent<Loading>().SetUpImage(LoadingImage);
        LoadingInstance.GetComponent<Loading>().Begin();
    }

    private void Awake()
    {
        Instance = this;
    }

    public override void OnJoinedRoom()
    {
        References.ChatServer = PhotonNetwork.CurrentRoom.Name;

        References.inviteType = InviteType.Arena;
        References.MapInvite = SceneType.BossArena_.ToString() + mapType.ToString();
        References.RoomNameInvite = PhotonNetwork.CurrentRoom.Name;

        arenaType = References.bossArenaType;

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Boss"))
        {
            BossName = PhotonNetwork.CurrentRoom.CustomProperties["Boss"].ToString();
            Boss = ListBoss.Find(obj => obj.gameObject.name  == BossName);
            BossPool = ListBossPool.Find(obj => obj.gameObject.name  == BossName + "Pool");
        }

        Game_Manager.Instance.SetupPlayer(SpawnPoint.position, CameraBox, AccountStatus.WaitingRoom);
        LoadingInstance.GetComponent<Loading>().End();
        PhotonNetwork.IsMessageQueueRunning = true;
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        JoinRoomFailedInstance = Instantiate(JoinRoomFailedPrefabs);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        JoinRoomFailedInstance = Instantiate(JoinRoomFailedPrefabs);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Game_Manager.Instance.ReloadPlayerProperties();
        CheckAllPlayerReady();
    }
    public void CheckAllPlayerReady()
    {
        PhotonNetwork.RaiseEvent(BattleStart_CheckReady, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    #region ProgressBar
    public void BattleStart_ProgressBar_Run()
    {
        ProgressRun = true;
        if (ProgressBar_Coroutine == null)
        {
            ProgressBar_Coroutine = StartCoroutine(Battle_ProgressBar());
        }
    }

    public void BattleStart_ProgressBar_Stop()
    {
        if (ProgressBar_Coroutine != null)
        {
            StopCoroutine(ProgressBar_Coroutine);
            ProgressBar_Coroutine = null;
        }

        ProgressBar.SetActive(false);
        ProgressRun = false;
        CurrentProgress = 0f;
    }
    #endregion

    #region Check Player Status

    public void CheckPlayerDead()
    {
        players = FindObjectsOfType<PlayerBase>();

        foreach (var player in players)
        {
            if (player.AccountEntity.CurrentHealth > 0)
            {
                return;
            }
        }

        Battle_End(false);
    }
    #endregion

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (References.IsInvite)
            {
                PhotonNetwork.JoinRoom(References.RoomNameInvite);
            }
            else
            {
                roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
                roomOptions.CustomRoomPropertiesForLobby = new string[] { "Boss" };
                roomOptions.CustomRoomProperties.Add("Boss", References.BossNameInvite);
                roomOptions.MaxPlayers = 5;
                roomOptions.BroadcastPropsChangeToAll = true;
                PhotonNetwork.CreateRoom(References.accountRefer.ID + References.GenerateRandomString(10), roomOptions, TypedLobby.Default);
            }
        }
    }

    private IEnumerator Battle_StartCoroutine()
    {
        float currentTime = ReadyTime;
        Battle_Start_CountdownTxt.gameObject.SetActive(true);
        while (currentTime > 0)
        {
            Battle_Start_CountdownTxt.text = string.Format("{0}", currentTime);

            yield return new WaitForSeconds(1f);

            currentTime--;
        }

        Battle_Start_CountdownTxt.gameObject.SetActive(false);
        Game_Manager.Instance.IsBusy = false;
        ActiveBoss();
        StartCoroutine(Battle_FightCoroutine());

    }

    public void Battle_End(bool Win)
    {
        if (Win)
        {
            ShowEndgamePanel("Thắng");
        }
        else
        {
            ShowEndgamePanel("Thua");
        }

    }

    private IEnumerator Battle_FightCoroutine()
    {
        currentTime = TotalTime;
        int minutes, seconds;
        while (currentTime > 0 && !BattleEnd)
        {
            minutes = Mathf.FloorToInt(currentTime / 60);
            seconds = Mathf.FloorToInt(currentTime % 60);

            Battle_Fight_CountdownTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return new WaitForSeconds(1f);

            currentTime--;
        }

        Battle_Fight_CountdownTxt.text = "00:00";
        Battle_End(false);

    }

    private IEnumerator Battle_ProgressBar()
    {
        ProgressBar.SetActive(true);
        while (CurrentProgress < TotalProgress && ProgressRun)
        {
            CurrentProgress += 0.005f;
            CurrentProgressBar.fillAmount = CurrentProgress / TotalProgress;
            yield return new WaitForSeconds(0.01f);
        }

        BattleStart = true;
        ProgressBar.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        StartCoroutine(Battle_StartCoroutine());
        Game_Manager.Instance.AccountStatus = AccountStatus.Arena;
        Game_Manager.Instance.ReloadPlayerProperties();
        Game_Manager.Instance.IsBusy = true;
        ReadyBase.SetActive(false);

    }

    public override void OnLeftRoom()
    {
        if (Game_Manager.Instance.PlayerManager != null && PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Destroy(Game_Manager.Instance.PlayerManager);
            Game_Manager.Instance.PlayerManager = null;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CheckPlayerDead();
    }

    public void ReturnToKonoha()
    {
        if (PhotonNetwork.InRoom)
        {
            Game_Manager.Instance.IsBusy = false;
            PhotonNetwork.IsMessageQueueRunning = false;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(Scenes.Konoha);
        }
    }

    private void OnApplicationQuit()
    {
        if (References.accountRefer != null && PhotonNetwork.IsConnectedAndReady)
        {
            References.UpdateAccountToDB();
            Account_DAO.ChangeStateOnline(References.accountRefer.ID, false);
        }

    }

    public void ShowEndgamePanel(string Text)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { EndGamePro, Text } });

        PhotonNetwork.RaiseEvent(ShowEndgamePanelEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void ActiveBoss()
    {
        PhotonNetwork.RaiseEvent(ActiveBossEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == ShowEndgamePanelEventCode)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(EndGamePro, out object EndText) && EndText != null)
            {
                string End = (string)EndText;
                Battle_End_Text.text = End;
            }
            sortCanvas.sortingOrder = 31;
            BossPool.SetActive(false);
            Boss.SetActive(false);
            BattleEnd = true;
            StopAllCoroutines();
            Game_Manager.Instance.IsBusy = true;
            Battle_End_Panel.SetActive(true);
        }
        else if (photonEvent.Code == ActiveBossEventCode)
        {           
            Boss.SetActive(true);
        }
        else if (photonEvent.Code == BattleStart_CheckReady)
        {
            RequireNumber = FindObjectsOfType<PlayerBase>().Length;
            if (CurrentNumber == RequireNumber && BattleStart == false)
            {
                BattleStart_ProgressBar_Run();
            }
            else
            {
                BattleStart_ProgressBar_Stop();

            }
        }

    }
}
