using Assets.Scripts.Database.DAO;
using Assets.Scripts.GameManager;
using ExitGames.Client.Photon;
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
    [SerializeField] Transform Spawnpoint;

    [SerializeField] PolygonCollider2D CameraBox;

    [Header("Battle Start")]
    float ReadyTime = 3f;
    float TotalProgress = 1f, CurrentProgress = 0f;

    bool BattleStart, ProgressRun;
    Coroutine ProgressBar_Coroutine;
    [SerializeField] GameObject ProgressBar;
    [SerializeField] Image CurrentProgressBar;
    [SerializeField] TMP_Text Battle_Start_CountdownTxt;

    [SerializeField] GameObject GuideTxt;

    [Header("Battle Time")]
    float TotalTime = 180f;
    float currentTime;
    [SerializeField] TMP_Text Battle_Fight_CountdownTxt;

    bool BattleEnd;

    [Header("Battle End")]
    [SerializeField] GameObject BattleEnd_Panel;
    [SerializeField] GameObject WinLose, Draw;
    [SerializeField] TMP_Text PlayerWin_Nametxt;
    [SerializeField] TMP_Text PlayerLose_Nametxt;
    string PlayerWin_Name, PlayerLose_Name;

    [Header("Player Instance")]
    [SerializeField] GameObject LoadingPrefabs;
    [SerializeField] Sprite LoadingImage;

    GameObject LoadingInstance;

    [Header("JoinRoom Failed")]
    [SerializeField] GameObject JoinRoomFailedPrefabs;
    GameObject JoinRoomFailedInstance;

    [SerializeField] List<PK_ReadyBase> ListReady;

    private const byte ShowEndgamePanelEventCode = 1;

    private const string WinnerTextPropKey = "WinnerText";
    private const string LoserTextPropKey = "LoserText";
    [SerializeField] string SceneName;

    int PlayerCount;
    string PlayerLoseID;

    RoomOptions roomOptions = new RoomOptions();

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


    public void IsAllPlayerReady()
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
            ProgressRun = true;
            GuideTxt.SetActive(false);
            ProgressBar_Coroutine = StartCoroutine(Battle_ProgressBar());
        }
        else
        {
            if (ProgressBar_Coroutine != null)
            {
                StopCoroutine(ProgressBar_Coroutine);
            }
            GuideTxt.SetActive(true);
            ProgressBar.SetActive(false);
            ProgressRun = false;
            CurrentProgress = 0f;
        }
    }

    // Function to raise the event to show the endgame panel for all players
    public void ShowEndgamePanel(string Winner, string Loser)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { WinnerTextPropKey, Winner }, { LoserTextPropKey, Loser } });

        PhotonNetwork.RaiseEvent(ShowEndgamePanelEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }


    // Handle the event when received
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == ShowEndgamePanelEventCode)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(WinnerTextPropKey, out object winnerNameObj) && winnerNameObj != null)
            {
                string winnerName = (string)winnerNameObj;
                PlayerWin_Nametxt.text = winnerName;
            }

            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(LoserTextPropKey, out object loserNameObj) && loserNameObj != null)
            {
                string loserName = (string)loserNameObj;
                PlayerLose_Nametxt.text = loserName;
            }
            Game_Manager.Instance.IsBusy = true;
            BattleEnd_Panel.SetActive(true);
        }
    }

    public void Battle_End(string LoserID)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            BoxCollider2D boxCollider = player.GetComponent<BoxCollider2D>();
            if (boxCollider != null)
            {
                boxCollider.enabled = false;
            }

            if (player.GetComponent<PlayerBase>().AccountEntity.ID == LoserID)
            {
                PlayerLose_Name = player.GetComponent<PlayerBase>().AccountEntity.Name;
            }
            else
            {
                PlayerWin_Name = player.GetComponent<PlayerBase>().AccountEntity.Name;
            }
        }
        ShowEndgamePanel(PlayerWin_Name, PlayerLose_Name);
    }


    public override void OnJoinedRoom()
    {
        References.SceneNameInvite = SceneName;
        References.InviteType = AccountStatus.PK;
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

    public void Battle_End()
    {

    }

    private IEnumerator Battle_ProgressBar()
    {
        ProgressBar.SetActive(true);
        while (CurrentProgress < TotalProgress && ProgressRun)
        {
            CurrentProgress += 0.01f;
            CurrentProgressBar.fillAmount = CurrentProgress / TotalProgress;
            yield return new WaitForSeconds(0.01f);
        }

        BattleStart = true;
        ProgressBar.SetActive(false);
        StartCoroutine(Battle_StartCoroutine());
        Game_Manager.Instance.AccountStatus = AccountStatus.PK;
        Game_Manager.Instance.ReloadPlayerProperties();
        Game_Manager.Instance.IsBusy = true;
        foreach (var p in ListReady)
        {
            p.gameObject.SetActive(false);
        }
        GuideTxt.SetActive(false);

    }

    private IEnumerator Battle_StartCoroutine()
    {
        float currentTime = ReadyTime;
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
                roomOptions.MaxPlayers = 2;
                roomOptions.BroadcastPropsChangeToAll = true;
                PhotonNetwork.CreateRoom(References.accountRefer.ID + References.GenerateRandomString(10), roomOptions, TypedLobby.Default);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Game_Manager.Instance.ReloadPlayerProperties();

    }
    private void OnApplicationQuit()
    {
        if (References.accountRefer != null && PhotonNetwork.IsConnectedAndReady)
        {
            References.UpdateAccountToDB();
            Account_DAO.ChangeStateOnline(References.accountRefer.ID, false);
        }

    }
}
