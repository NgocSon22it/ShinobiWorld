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

public class BossArena_Manager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] GameObject Boss;
    [SerializeField] GameObject BossPool;

    [SerializeField] Transform SpawnPoint;

    [SerializeField] GameObject GuideTxt;

    [SerializeField] PolygonCollider2D CameraBox;

    [Header("Battle Time")]
    float TotalTime = 180f;
    float currentTime;
    [SerializeField] TMP_Text Battle_Fight_CountdownTxt;
    [SerializeField] GameObject ReadyBase;

    [Header("Battle Start")]
    float TotalProgress = 1f, CurrentProgress = 0f;
    bool BattleStart, ProgressRun;
    Coroutine ProgressBar_Coroutine;
    [SerializeField] GameObject ProgressBar;
    [SerializeField] Image CurrentProgressBar;
    float ReadyTime = 3f;
    [SerializeField] TMP_Text Battle_Start_CountdownTxt;
    public int RequireNumber, CurrentNumber;

    [Header("Battle End")]
    [SerializeField] GameObject Battle_End_Panel;
    [SerializeField] TMP_Text Battle_End_Text;

    bool BattleEnd;

    [Header("Player Instance")]
    [SerializeField] GameObject LoadingPrefabs;
    [SerializeField] Sprite LoadingImage;

    private const byte ShowEndgamePanelEventCode = 1;
    private const string EndGamePro = "EndGame";

    RoomOptions roomOptions = new RoomOptions();

    GameObject LoadingInstance;

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
    public void Battle_Start()
    {

    }

    public override void OnJoinedRoom()
    {
        Game_Manager.Instance.SetupPlayer(SpawnPoint.position, CameraBox, AccountStatus.Arena);
        LoadingInstance.GetComponent<Loading>().End();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Game_Manager.Instance.ReloadPlayerProperties();
        CheckPlayerReady();
    }

    public void CheckPlayerReady()
    {
        RequireNumber = PhotonNetwork.PlayerList.Length;
        if (CurrentNumber == RequireNumber && BattleStart == false) 
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

            ProgressBar.SetActive(false);
            GuideTxt.SetActive(true);
            ProgressRun = false;
            CurrentProgress = 0f;
        }
    }

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {           
            roomOptions.MaxPlayers = 5;
            roomOptions.BroadcastPropsChangeToAll = true;
            PhotonNetwork.JoinOrCreateRoom("123", roomOptions, TypedLobby.Default);
        }
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
        Boss.SetActive(true);
        StartCoroutine(Battle_FightCoroutine());

    }

    public void Battle_End(bool Win)
    {

        BossPool.SetActive(false);
        Boss.SetActive(false);
        BattleEnd = true;

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
        Game_Manager.Instance.IsBusy = true;
        ReadyBase.SetActive(false);
        GuideTxt.SetActive(false);
        roomOptions.IsOpen = false;

    }

    public override void OnLeftRoom()
    {
        if (Game_Manager.Instance.PlayerManager != null && PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Destroy(Game_Manager.Instance.PlayerManager);
            Game_Manager.Instance.PlayerManager = null;
        }
    }

    public void ReturnToKonoha()
    {
        if (PhotonNetwork.InRoom)
        {
            Game_Manager.Instance.IsBusy = false;
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
        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { EndGamePro, Text } }) ;

        PhotonNetwork.RaiseEvent(ShowEndgamePanelEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
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

            Game_Manager.Instance.IsBusy = true;
            Battle_End_Panel.SetActive(true);
        }
    }
}
