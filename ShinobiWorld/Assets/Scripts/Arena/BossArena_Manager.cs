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
using Unity.VisualScripting;
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
    [SerializeField] GameObject Battle_End_Panel, Prize_Panel, NotPrize_Panel, NormalPrize_Panel, UpTrophy_Panel, Lose_Panel;
    [SerializeField] TMP_Text Battle_End_Text, Prize_CoinTxt, Prize_ExperienceTxt, Prize_TrophyTxt;

    bool BattleEnd;

    [Header("Player Instance")]
    [SerializeField] GameObject LoadingPrefabs;
    [SerializeField] Sprite LoadingImage;
    GameObject LoadingInstance;

    private const byte ShowEndgamePanelEventCode = 1;
    private const byte ActiveBossEventCode = 2;
    private const byte BattleStart_CheckReady = 3;

    private const string WinProperties = "Win";

    [Header("Room Value")]
    [SerializeField] MapType mapType;
    BossArenaType arenaType;
    string BossName;
    RoomOptions roomOptions = new RoomOptions();
    PlayerBase[] players;
    int CoinBonus = 1000, ExperienceBonus = 1000;
    [Header("JoinRoom Failed")]
    [SerializeField] GameObject JoinRoomFailedPrefabs;
    GameObject JoinRoomFailedInstance;

    [Header("LostConnect")]
    [SerializeField] GameObject LostConnectPrefabs;
    GameObject LostConnectInstance;

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

        SetUp_BossName();
        SetUp_ArenaType();

        Game_Manager.Instance.SetupPlayer(SpawnPoint.position, CameraBox, AccountStatus.WaitingRoom);
        LoadingInstance.GetComponent<Loading>().End();
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (cause == DisconnectCause.ClientTimeout)
        {
            CallOnquit();
            LostConnectInstance = Instantiate(LostConnectPrefabs);
        }
    }

    public void SetUp_BossName()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Boss"))
        {
            BossName = PhotonNetwork.CurrentRoom.CustomProperties["Boss"].ToString();
            Boss = ListBoss.Find(obj => obj.gameObject.name == BossName);
            BossPool = ListBossPool.Find(obj => obj.gameObject.name == BossName + "Pool");
        }
    }
    public void SetUp_ArenaType()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("IsOfficial"))
        {
            bool IsOfficial = (bool)PhotonNetwork.CurrentRoom.CustomProperties["IsOfficial"];
            if (IsOfficial)
            {
                arenaType = BossArenaType.Official;
            }
            else
            {
                arenaType = BossArenaType.Practice;
            }

        }
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
                roomOptions.CustomRoomPropertiesForLobby = new string[] { "Boss", "IsOfficial", "WhoRegister", "TrophyRegister" };
                roomOptions.CustomRoomProperties.Add("Boss", References.BossNameInvite);
                if (References.bossArenaType == BossArenaType.Official)
                {
                    roomOptions.CustomRoomProperties.Add("IsOfficial", true);
                    roomOptions.CustomRoomProperties.Add("WhoRegister", References.accountRefer.ID);
                    roomOptions.CustomRoomProperties.Add("TrophyRegister", References.TrophyRegister);
                }
                else if (References.bossArenaType == BossArenaType.Practice)
                {
                    roomOptions.CustomRoomProperties.Add("IsOfficial", false);
                }
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
            ShowEndgamePanel(Win);
        }
        else
        {
            ShowEndgamePanel(Win);
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
        DeleteTicketForRegister();
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
            if (References.accountRefer.CurrentHealth > 0)
            {
                References.PlayerSpawnPosition = new Vector3(-43, -27, 0);
            }
            else
            {
                References.PlayerSpawnPosition = new Vector3(17, -27, 0);
            }
            Game_Manager.Instance.IsBusy = false;
            PhotonNetwork.IsMessageQueueRunning = false;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(Scenes.Konoha);
        }
    }

    private void OnApplicationQuit()
    {
        CallOnquit();
    }

    public void ShowEndgamePanel(bool Win)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { WinProperties, Win } });

        PhotonNetwork.RaiseEvent(ShowEndgamePanelEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void ActiveBoss()
    {
        PhotonNetwork.RaiseEvent(ActiveBossEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void DeleteTicketForRegister()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("IsOfficial"))
        {
            bool isOfficial = (bool)PhotonNetwork.CurrentRoom.CustomProperties["IsOfficial"];

            if (isOfficial)
            {
                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("WhoRegister"))
                {
                    string Register = (string)PhotonNetwork.CurrentRoom.CustomProperties["WhoRegister"];

                    if (References.accountRefer.ID.Equals(Register))
                    {
                        References.accountRefer.HasTicket = false;
                    }
                }

            }
        }

    }

    public void CheckOfficial_Practice(bool Win)
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("IsOfficial"))
        {
            bool isOfficial = (bool)PhotonNetwork.CurrentRoom.CustomProperties["IsOfficial"];

            if (isOfficial)
            {
                if (Win)
                {
                    Prize_Panel.SetActive(true);
                    if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("WhoRegister"))
                    {
                        string Register = (string)PhotonNetwork.CurrentRoom.CustomProperties["WhoRegister"];

                        if (References.accountRefer.ID.Equals(Register))
                        {
                            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("TrophyRegister"))
                            {
                                UpTrophy_Panel.SetActive(true);
                                TrophyID Trophy = (TrophyID)PhotonNetwork.CurrentRoom.CustomProperties["TrophyRegister"];
                                References.accountRefer.TrophyID = Trophy.ToString();
                                switch (Trophy)
                                {
                                    case TrophyID.Trophy_Genin:
                                        Prize_TrophyTxt.text = "Hạ đẳng";
                                        break;
                                    case TrophyID.Trophy_Chunin:
                                        Prize_TrophyTxt.text = "Trung đẳng";
                                        break;
                                    case TrophyID.Trophy_Jonin:
                                        Prize_TrophyTxt.text = "Thượng đẳng";
                                        break;
                                }
                            }
                        }
                        else
                        {
                            NormalPrize_Panel.SetActive(true);
                            Prize_CoinTxt.text = CoinBonus.ToString();
                            Prize_ExperienceTxt.text = ExperienceBonus.ToString();
                            References.AddCoin(CoinBonus);
                            References.AddExperience(ExperienceBonus);
                        }
                    }
                }
                else
                {
                    Lose_Panel.SetActive(true);
                }
            }
            else
            {
                if (Win)
                {
                    NotPrize_Panel.SetActive(true);
                }
                else
                {
                    Lose_Panel.SetActive(true);
                }
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

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == ShowEndgamePanelEventCode)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(WinProperties, out object Win) && Win != null)
            {
                bool IsWin = (bool)Win;
                if (IsWin)
                {
                    Battle_End_Text.text = "Thắng";
                }
                else
                {
                    Battle_End_Text.text = "Thua";
                }
                CheckOfficial_Practice(IsWin);
            }
            Battle_Fight_CountdownTxt.text = "00:00";
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
