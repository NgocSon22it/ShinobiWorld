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


    public static Game_Manager Instance;

    ExitGames.Client.Photon.Hashtable PlayerProperties = new ExitGames.Client.Photon.Hashtable();

    [SerializeField] List<GameObject> List_LangLa1 = new List<GameObject>();

    [SerializeField] List<GameObject> List_LangLa2 = new List<GameObject>();

    [SerializeField] List<GameObject> List_LangLa3 = new List<GameObject>();

    [SerializeField] List<GameObject> List_LangLa4 = new List<GameObject>();

    public bool IsBusy;

    SqlDateTime dateTime;

    public Vector3 PlayerReconnectPosition;

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
            PhotonNetwork.JoinOrCreateRoom("S1", roomOptions, TypedLobby.Default);
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonPeer.RegisterType(typeof(Account_Entity), (byte)'A', Account_Entity.Serialize, Account_Entity.Deserialize);

        SetupPlayer(References.PlayerSpawnPosition);
        ChatManager.Instance.ConnectToChat();
        StartCoroutine(SpawnEnemy());
    }

    public void SetupPlayer(Vector3 position)
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
                    if (dateTime >= enemyScript.areaBoss_Entity.TimeSpawn
                        && enemyScript.areaBoss_Entity.isDead == false
                        && enemyScript.areaBoss_Entity.CurrentHealth > 0)
                    {
                        enemyScript.LoadHealthUI();
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
        References.LoadAccountWeaponNSkill(Role);
        References.LoadAccount();
        string AccountJson = JsonUtility.ToJson(References.accountRefer);
        string AccountWeaponJson = JsonUtility.ToJson(References.accountWeapon);
        string AccountSkillOneJson = JsonUtility.ToJson(References.accountSkillOne);
        string AccountSkillTwoJson = JsonUtility.ToJson(References.accountSkillTwo);
        string AccountSkillThreeJson = JsonUtility.ToJson(References.accountSkillThree);

        PlayerProperties["Account"] = AccountJson;
        PlayerProperties["AccountWeapon"] = AccountWeaponJson;
        PlayerProperties["AccountSkillOne"] = AccountSkillOneJson;
        PlayerProperties["AccountSkillTwo"] = AccountSkillTwoJson;
        PlayerProperties["AccountSkillThree"] = AccountSkillThreeJson;

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
        //References.UpdateAccountToDB();
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
        //References.UpdateAccountToDB();
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
        /*Debug.Log(Message.HaveWifi);
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinOrCreateRoom("S1", roomOptions, TypedLobby.Default);
        }*/
    }

    private IEnumerator RetryConnection()
    {
        yield return new WaitForSeconds(5f);  // Wait for 5 seconds before retrying

        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnApplicationQuit()
    {
        if (References.accountRefer != null && PhotonNetwork.IsConnectedAndReady)
        {
            Account_DAO.ChangeStateOnline(References.accountRefer.ID, false);
            References.UpdateAccountToDB();
        }

    }
}
