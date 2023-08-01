using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.TextCore.Text;
using Assets.Scripts.Database.DAO;
using System.Security.Principal;
using ExitGames.Client.Photon;
using Photon.Pun.Demo.PunBasics;
using System.Threading;
using Assets.Scripts.Hospital;
using UnityEngine.UIElements;
using Assets.Scripts.Database.Entity;
using System.Data;
using Unity.VisualScripting;
using System.Data.SqlTypes;
using System;
using Assets.Scripts.GameManager;

public class Game_Manager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameObject PlayerManager;

    public string Role;

    [SerializeField] GameObject PlayerMelee;
    [SerializeField] GameObject PlayerRange;
    [SerializeField] GameObject PlayerSupport;

    [SerializeField] PolygonCollider2D CameraBox;

    public static Game_Manager Instance;

    ExitGames.Client.Photon.Hashtable PlayerProperties = new ExitGames.Client.Photon.Hashtable();

    public bool IsBusy;

    public AccountStatus AccountStatus;

    public string CurrentAreaName;

    RoomOptions roomOptions = new RoomOptions();

    [Header("Player Instance")]
    [SerializeField] GameObject LoadingPrefabs;

    [SerializeField] Sprite LoadingImage;

    GameObject LoadingInstance;

    [SerializeField] GameObject InfoPrefabs;
    public GameObject InfoInstance;

    [Header("JoinRoom Failed")]
    [SerializeField] GameObject JoinRoomFailedPrefabs;
    GameObject JoinRoomFailedInstance;

    private void Awake()
    {
        Instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        LoadingInstance = Instantiate(LoadingPrefabs);
        LoadingInstance.GetComponent<Loading>().SetUpImage(LoadingImage);
        LoadingInstance.GetComponent<Loading>().Begin();

        InfoInstance = Instantiate(InfoPrefabs);

        if (PhotonNetwork.IsConnectedAndReady)
        {
            roomOptions.MaxPlayers = 0; // Maximum number of players allowed in the room
            roomOptions.IsOpen = true;
            roomOptions.BroadcastPropsChangeToAll = true;
            PhotonNetwork.JoinOrCreateRoom(CurrentAreaName, roomOptions, TypedLobby.Default);
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonPeer.RegisterType(typeof(Account_Entity), (byte)'A', Account_Entity.Serialize, Account_Entity.Deserialize);
        References.IsInvite = false;
        SetupPlayer(References.PlayerSpawnPosition, CameraBox, AccountStatus.Normal);    
        LoadingInstance.GetComponent<Loading>().End();
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    public void SetupPlayer(Vector3 position, PolygonCollider2D CameraBox, AccountStatus accountStatus)
    {
        if (PlayerManager == null && PhotonNetwork.IsConnectedAndReady)
        {
            switch (References.accountRefer.RoleInGameID)
            {
                case "Role_Melee":
                    Role = "Melee";
                    PlayerManager = PhotonNetwork.Instantiate("Player/Melee/" + Path.Combine(PlayerMelee.name), position, Quaternion.identity);
                    break;
                case "Role_Range":
                    Role = "Range";
                    PlayerManager = PhotonNetwork.Instantiate("Player/Range/" + Path.Combine(PlayerRange.name), position, Quaternion.identity);
                    break;
                case "Role_Support":
                    Role = "Support";
                    PlayerManager = PhotonNetwork.Instantiate("Player/Support/" + Path.Combine(PlayerSupport.name), position, Quaternion.identity);
                    break;
            }
            PlayerManager.GetComponent<PlayerBase>().CameraBox = CameraBox;
            AccountStatus = accountStatus;
            ReloadPlayerProperties();
            Debug.Log("Successfully joined room S1!");
        }
    }


    public void ReloadPlayerProperties()
    {
        if(References.accountRefer.CurrentHealth <= 0) References.accountRefer.IsDead = true;
        References.UpdateAccountToDB();
        References.LoadHasWeaponNSkill(Role);
        References.LoadAccount();

        int accountStatus = (int)AccountStatus;
        string AccountJson = JsonUtility.ToJson(References.accountRefer);
        string HasWeaponJson = JsonUtility.ToJson(References.hasWeapon);
        string HasSkillOneJson = JsonUtility.ToJson(References.hasSkillOne);
        string HasSkillTwoJson = JsonUtility.ToJson(References.hasSkillTwo);
        string HasSkillThreeJson = JsonUtility.ToJson(References.hasSkillThree);

        PlayerProperties["AccountStatus"] = accountStatus;
        PlayerProperties["Account"] = AccountJson;
        PlayerProperties["HasWeapon"] = HasWeaponJson;
        PlayerProperties["HasSkillOne"] = HasSkillOneJson;
        PlayerProperties["HasSkillTwo"] = HasSkillTwoJson;
        PlayerProperties["HasSkillThree"] = HasSkillThreeJson;

        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperties);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        JoinRoomFailedInstance = Instantiate(JoinRoomFailedPrefabs);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        JoinRoomFailedInstance = Instantiate(JoinRoomFailedPrefabs);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        ReloadPlayerProperties();
    }

    public override void OnLeftRoom()
    {
        if (PlayerManager != null && PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Destroy(PlayerManager);
            PlayerManager = null;
        }
    }

    public void GoToMenu()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.IsMessageQueueRunning = false;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(Scenes.Login);
        }
    }

    public void GoingOutHospital()
    {
        PlayerManager.GetComponent<PlayerBase>().CallInvoke();
        ReloadPlayerProperties();
        PlayerManager.GetComponent<PlayerBase>().CallRpcPlayerLive();
        PlayerManager.transform.position = References.HouseAddress[House.Hospital.ToString()];
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (cause != DisconnectCause.DisconnectByClientLogic)
        {
            Debug.Log(Message.LostWifi);
            if (!PhotonNetwork.IsConnected)
            {
                
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            roomOptions.MaxPlayers = 0;
            roomOptions.IsOpen = true;
            roomOptions.BroadcastPropsChangeToAll = true;
            PhotonNetwork.JoinOrCreateRoom(CurrentAreaName, roomOptions, TypedLobby.Default);
        }
    }


    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit GameManager");
        if (References.accountRefer != null)
        {
            References.UpdateAccountToDB();
            Account_DAO.ChangeStateOnline(References.accountRefer.ID, false);
        }

    }

    public void SpawnEnemyAfterDie(string AreaID, string EnemyID, int ViewID, Coroutine SpawnEnemyCoroutine)
    {
        if (SpawnEnemyCoroutine == null)
        {
            SpawnEnemyCoroutine = StartCoroutine(SpawnEnemy(AreaID, EnemyID, ViewID, SpawnEnemyCoroutine));
        }
    }


    IEnumerator SpawnEnemy(string AreaID, string EnemyID, int ViewID, Coroutine SpawnEnemyCoroutine)
    {
        yield return new WaitForSeconds(10f);
        gameObject.SetActive(true);
        if (PhotonNetwork.IsConnected)
        {
            object[] data = new object[] { ViewID };
            PhotonNetwork.RaiseEvent((byte)CustomEventCode.EnemyActive, data, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }
        AreaEnemy_DAO.SetAreaEnemyAlive(AreaID, EnemyID);

        if(SpawnEnemyCoroutine != null)
        {
            StopCoroutine(SpawnEnemyCoroutine);
        }
    }

    public void ShowEndgamePanel()
    {
        PhotonNetwork.RaiseEvent((byte)CustomEventCode.EnemyDeactivate, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)CustomEventCode.EnemyDeactivate)
        {
            object[] data = (object[])photonEvent.CustomData;
            int viewID = (int)data[0];

            GameObject enemyObject = PhotonView.Find(viewID).gameObject;
            enemyObject.SetActive(false);
        }
        else if (photonEvent.Code == (byte)CustomEventCode.EnemyActive)
        {
            object[] data = (object[])photonEvent.CustomData;
            int viewID = (int)data[0];

            GameObject enemyObject = PhotonView.Find(viewID).gameObject;
            enemyObject.SetActive(true);
        }
    }
}
