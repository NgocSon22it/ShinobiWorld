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


public class Game_Manager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerManager;

    public string Role;

    [SerializeField] GameObject PlayerMelee;
    [SerializeField] GameObject PlayerRange;
    [SerializeField] GameObject PlayerSupport;

    [SerializeField] PolygonCollider2D CameraBox;

    public static Game_Manager Instance;

    ExitGames.Client.Photon.Hashtable PlayerProperties = new ExitGames.Client.Photon.Hashtable();

    [SerializeField] List<GameObject> List_LangLa1 = new List<GameObject>();

    [SerializeField] List<GameObject> List_LangLa2 = new List<GameObject>();

    [SerializeField] List<GameObject> List_LangLa3 = new List<GameObject>();

    [SerializeField] List<GameObject> List_LangLa4 = new List<GameObject>();

    public bool IsBusy;

    SqlDateTime dateTime;

    public Vector3 PlayerReconnectPosition;

    Coroutine SpawnEnemyCoroutine;

    RoomOptions roomOptions = new RoomOptions();

    private void Awake()
    {
        Instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            roomOptions.MaxPlayers = 0; // Maximum number of players allowed in the room
            roomOptions.IsOpen = true;
            roomOptions.BroadcastPropsChangeToAll = true;
            PhotonNetwork.JoinOrCreateRoom(References.ServerName, roomOptions, TypedLobby.Default);
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonPeer.RegisterType(typeof(Account_Entity), (byte)'A', Account_Entity.Serialize, Account_Entity.Deserialize);

        SetupPlayer(References.PlayerSpawnPosition, CameraBox, AccountStatus.Normal);
        ChatManager.Instance.ConnectToChat();
        SpawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
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
            PlayerManager.GetComponent<PlayerBase>().AccountStatus = accountStatus;
            ReloadPlayerProperties();
            Debug.Log("Successfully joined room S1!");
        }
    }

    public IEnumerator SpawnEnemy()
    {
        while (true)
        {
            // Get the current time
            dateTime = new SqlDateTime(System.DateTime.Now);
            foreach (GameObject enemy in List_LangLa1)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    if (dateTime >= enemyScript.AreaEnemy_Entity.TimeSpawn
                        && enemyScript.AreaEnemy_Entity.IsDead == false
                        && enemyScript.AreaEnemy_Entity.CurrentHealth > 0)
                    {
                        enemyScript.LoadHealthUI(enemyScript.AreaEnemy_Entity.CurrentHealth, enemyScript.boss_Entity.Health);
                        enemy.SetActive(true);
                    }
                }
            }
            // Wait for the next frame
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }


    public void ReloadPlayerProperties()
    {
        References.UpdateAccountToDB();
        References.LoadHasWeaponNSkill(Role);
        References.LoadAccount();
        string AccountJson = JsonUtility.ToJson(References.accountRefer);
        string HasWeaponJson = JsonUtility.ToJson(References.hasWeapon);
        string HasSkillOneJson = JsonUtility.ToJson(References.hasSkillOne);
        string HasSkillTwoJson = JsonUtility.ToJson(References.hasSkillTwo);
        string HasSkillThreeJson = JsonUtility.ToJson(References.hasSkillThree);

        PlayerProperties["Account"] = AccountJson;
        PlayerProperties["HasWeapon"] = HasWeaponJson;
        PlayerProperties["HasSkillOne"] = HasSkillOneJson;
        PlayerProperties["HasSkillTwo"] = HasSkillTwoJson;
        PlayerProperties["HasSkillThree"] = HasSkillThreeJson;

        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperties);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed");
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
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(Scenes.Login);
            if (SpawnEnemyCoroutine != null)
            {
                StopCoroutine(SpawnEnemyCoroutine);
            }
        }
    }

    public void GoingToHospital()
    {
        Hospital.Instance.SetDuration(References.RespawnTime).Begin();
    }

    public void GoingOutHospital()
    {
        References.accountRefer.CurrentHealth = References.accountRefer.Health;
        References.accountRefer.CurrentChakra = References.accountRefer.Chakra;
        PlayerManager.GetComponent<PlayerBase>().CallInvoke();
        ReloadPlayerProperties();
        PlayerManager.GetComponent<PlayerBase>().SetUpPlayerLive();
        PlayerManager.transform.position = References.HouseAddress[House.Hospital.ToString()];
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        /*if (cause != DisconnectCause.DisconnectByClientLogic)
        {
            Debug.Log(Message.LostWifi);
            References.IsDisconnect = true;
            if (References.IsDisconnect && !PhotonNetwork.IsConnected)
            {
                StartCoroutine(RetryConnection());
            }
        }*/
    }

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            roomOptions.MaxPlayers = 0; // Maximum number of players allowed in the room
            roomOptions.IsOpen = true;
            roomOptions.BroadcastPropsChangeToAll = true;
            PhotonNetwork.JoinOrCreateRoom(References.ServerName, roomOptions, TypedLobby.Default);
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
}
