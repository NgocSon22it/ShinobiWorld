using Assets.Scripts.Database.DAO;
using Assets.Scripts.GameManager;
using ExitGames.Client.Photon;
using Pathfinding.Util;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PK_Manager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [Header("Set Up")]
    [SerializeField] Transform Spawnpoint;
    [SerializeField] PolygonCollider2D CameraBox;
    [SerializeField] Canvas sortCanvas;

    [Header("Battle Start")]
    float ReadyTime = 3f, TotalProgress = 1f, CurrentProgress = 0f;
    bool BattleStart, ProgressRun;
    Coroutine ProgressBar_Coroutine;
    [SerializeField] GameObject ProgressBar;
    [SerializeField] Image CurrentProgressBar;
    [SerializeField] TMP_Text Battle_Start_CountdownTxt;

    [Header("Battle Time")]
    float TotalTime = 180f, currentTime;
    [SerializeField] TMP_Text Battle_Fight_CountdownTxt;

    [Header("Battle End")]
    [SerializeField] GameObject Win_Panel, Lose_Panel, Draw_Panel;
    [SerializeField] TMP_Text CoinWin_txt, CoinLose_txt, CoinDraw_txt;
    bool BattleEnd;

    [Header("Player Instance")]
    [SerializeField] GameObject LoadingPrefabs;
    [SerializeField] Sprite LoadingImage;
    GameObject LoadingInstance;

    [Header("JoinRoom Failed")]
    [SerializeField] GameObject JoinRoomFailedPrefabs;
    GameObject JoinRoomFailedInstance;

    [Header("Event Code")]
    private const byte BattleEnd_DrawEventCode = 1;
    private const byte BattleEnd_WinLoseEventCode = 2;
    private const byte BattleStart_CheckReadyEventCode = 3;

    [Header("Room Value")]
    int CurrentBet, PlayerCount;
    [SerializeField] MapType mapType;
    [SerializeField] List<PK_ReadyBase> ListReady;
    PlayerBase[] players;
    RoomOptions roomOptions = new RoomOptions();

    [Header("LostConnect")]
    [SerializeField] GameObject LostConnectPrefabs;
    GameObject LostConnectInstance;

    [Header("Sound")]
    [SerializeField] AudioSource WinSound;
    [SerializeField] AudioSource LoseSound;

    public static PK_Manager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadingInstance = Instantiate(LoadingPrefabs);
        LoadingInstance.GetComponent<Loading>().SetUpImage(LoadingImage);
        LoadingInstance.GetComponent<Loading>().Begin();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        JoinRoomFailedInstance = Instantiate(JoinRoomFailedPrefabs);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        JoinRoomFailedInstance = Instantiate(JoinRoomFailedPrefabs);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        if (cause == DisconnectCause.ClientTimeout)
        {
            CallOnquit();
            LostConnectInstance = Instantiate(LostConnectPrefabs);
        }
    }

    // Check 2 player is in ReadyBase

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

    public void CheckAllPlayerReady()
    {
        PhotonNetwork.RaiseEvent(BattleStart_CheckReadyEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);     
    }

    public void ShowEndgamePanel_Draw()
    {
        PhotonNetwork.RaiseEvent(BattleEnd_DrawEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void ShowEndgamePanel_WinLose()
    {
        PhotonNetwork.RaiseEvent(BattleEnd_WinLoseEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == BattleEnd_DrawEventCode)
        {
            sortCanvas.sortingOrder = 31;
            References.AddCoin(CurrentBet + (CurrentBet * 80 / 100));
            CoinDraw_txt.text = CurrentBet.ToString();
            Battle_Fight_CountdownTxt.text = "00:00";
            WinSound.Play();
            Draw_Panel.SetActive(true);
            Game_Manager.Instance.IsBusy = true;
        }
        else if (photonEvent.Code == BattleEnd_WinLoseEventCode)
        {
            sortCanvas.sortingOrder = 31;
            Battle_Fight_CountdownTxt.text = "00:00";
            if (References.accountRefer.CurrentHealth > 0)
            {
                References.AddCoin(CurrentBet + (CurrentBet * 80 / 100));
                CoinWin_txt.text = (CurrentBet + (CurrentBet * 80 / 100)).ToString();
                Account_DAO.IncreaseWinTime(References.accountRefer.ID);
                WinSound.Play();
                Win_Panel.SetActive(true);
            }
            else
            {
                References.SaveCurrentHealth = 0;
                References.SaveCurrentChakra = 0;
                CoinLose_txt.text = "-" + CurrentBet.ToString();
                LoseSound.Play();
                Lose_Panel.SetActive(true);
            }
            Game_Manager.Instance.IsBusy = true;
        }
        else if (photonEvent.Code == BattleStart_CheckReadyEventCode)
        {
            PlayerCount = 0;

            foreach (var p in ListReady)
            {
                if (p.GetReady())
                {
                    PlayerCount++;
                }
            }

            if (PlayerCount == 2 && BattleStart == false)
            {
                BattleStart_ProgressBar_Run();
            }
            else
            {
                BattleStart_ProgressBar_Stop();
            }
        }
    }

    public void CheckPlayerDead()
    {
        players = FindObjectsOfType<PlayerBase>();

        foreach (var player in players)
        {
            if (player.AccountEntity.CurrentHealth <= 0)
            {
                player.playerCollider.enabled = false;
                BattleEnd = true;
            }
        }
    }

    public bool IsGameDraw()
    {
        players = FindObjectsOfType<PlayerBase>();

        if (players.Length == 1)
        {
            return false;
        }
        else
        {
            foreach (var player in players)
            {
                if (player.AccountEntity.CurrentHealth <= 0)
                {
                    return false;
                }
            }

            return true;
        }

    }

    public void SetUp_PKBet()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("PKBet"))
        {
            CurrentBet = (int) PhotonNetwork.CurrentRoom.CustomProperties["PKBet"];
        }
    }

    public void Battle_End()
    {
        if (IsGameDraw())
        {
            ShowEndgamePanel_Draw();
        }
        else
        {
            ShowEndgamePanel_WinLose();
        }
    }


    public override void OnJoinedRoom()
    {
        References.ChatServer = PhotonNetwork.CurrentRoom.Name;

        References.inviteType = InviteType.PK;
        References.MapInvite = SceneType.PK_.ToString() + mapType.ToString();
        References.RoomNameInvite = PhotonNetwork.CurrentRoom.Name;

        SetUp_PKBet();

        Game_Manager.Instance.SetupPlayer(Spawnpoint.position, CameraBox, AccountStatus.WaitingRoom);
        LoadingInstance.GetComponent<Loading>().End();
        PhotonNetwork.IsMessageQueueRunning = true;
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
        Battle_End();

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
        StartCoroutine(Battle_StartCoroutine());
        References.AddCoin(-References.PKBet);
        Game_Manager.Instance.AccountStatus = AccountStatus.PK;
        Game_Manager.Instance.ReloadPlayerProperties();
        Game_Manager.Instance.IsBusy = true;
        foreach (var readyBase in ListReady)
        {
            readyBase.gameObject.SetActive(false);
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
        StartCoroutine(Battle_FightCoroutine());
    }

    public void ReturnToKonoha()
    {
        if (PhotonNetwork.InRoom)
        {
            if (References.accountRefer.CurrentHealth > 0)
            {
                References.PlayerSpawnPosition = new Vector3(-43, -27, 0);
            }
            else
            {
                References.PlayerSpawnPosition = new Vector3(17, -27, 0);
            }    
            ChatManager.Instance.DisconnectFromChat();
            Game_Manager.Instance.IsBusy = false;
            PhotonNetwork.IsMessageQueueRunning = false;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(Scenes.Konoha);
        }
    }

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
                roomOptions.CustomRoomPropertiesForLobby = new string[] { "PKBet"};
                roomOptions.CustomRoomProperties.Add("PKBet", References.PKBet);
                roomOptions.MaxPlayers = 2;
                roomOptions.BroadcastPropsChangeToAll = true;
                PhotonNetwork.CreateRoom(References.accountRefer.ID + References.GenerateRandomString(10), roomOptions, TypedLobby.Default);
            }
        }
    }

    public void CallOnquit()
    {
        if (References.accountRefer != null)
        {
            References.SetUp_Normal();
            References.UpdateAccountToDB();
            Account_DAO.ChangeStateOnline(References.accountRefer.ID, false);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Game_Manager.Instance.ReloadPlayerProperties();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (BattleStart && !BattleEnd)
        {
            BattleEnd = true;
        }
    }

    private void OnApplicationQuit()
    {
        CallOnquit();

    }
}
