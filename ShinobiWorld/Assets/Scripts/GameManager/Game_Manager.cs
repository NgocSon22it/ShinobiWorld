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

public class EnemyInfo
{
    public string EnemyID;
    public string Extension;
    public string AreaName;

    public GameObject enemyPrefab;
    public Vector3 SpawnPosition;

    public Boss_Entity boss_Entity;
    public AreaBoss_Entity areaBoss_Entity;

    public void SetBossEntity() { boss_Entity = Boss_DAO.GetBossByID(EnemyID); }
    public void SetAreaBossEntity() { areaBoss_Entity = AreaBoss_DAO.GetAreaBossByID(AreaName, EnemyID); }

}


public class Game_Manager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerManager;

    public string Role;

    [SerializeField] GameObject PlayerMelee;
    [SerializeField] GameObject PlayerRange;
    [SerializeField] GameObject PlayerSupport;

    [SerializeField] GameObject BossPrefabs_Bat;
    [SerializeField] GameObject BossPrefabs_Fish;
    [SerializeField] GameObject BossPrefabs_Crap;

    public static Game_Manager Instance;

    ExitGames.Client.Photon.Hashtable PlayerProperties = new ExitGames.Client.Photon.Hashtable();

    [SerializeField] List<EnemyInfo> SpawnList_LangLa1 = new List<EnemyInfo>();


    [SerializeField] List<GameObject> List_LangLa1 = new List<GameObject>();

    public bool IsBusy;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 0; // Maximum number of players allowed in the room
            roomOptions.IsOpen = true;
            roomOptions.BroadcastPropsChangeToAll = true;
            PhotonNetwork.JoinOrCreateRoom("S1", roomOptions, TypedLobby.Default);
        }
    }


    public override void OnJoinedRoom()
    {
        PhotonPeer.RegisterType(typeof(Account_Entity), (byte)'A', Account_Entity.Serialize, Account_Entity.Deserialize);
        SetupPlayer(References.HouseAddress[House.Hokage.ToString()]);
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



    public void ReloadPlayerProperties()
    {
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
        References.UpdateAccountToDB();
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
        PhotonNetwork.LeaveRoom(false);
        PhotonNetwork.LoadLevel(Scenes.Login);
    }

    public void GoingToHospital()
    {
        PlayerManager.GetComponent<BoxCollider2D>().enabled = false;
        Hospital.Instance.SetDuration(References.RespawnTime).Begin();
    }

    public void GoingOutHospital()
    {
        References.accountRefer.CurrentHealth = References.accountRefer.Health;
        References.accountRefer.CurrentChakra = References.accountRefer.Chakra;
        PlayerManager.GetComponent<PlayerBase>().CallInvoke();
        References.UpdateAccountToDB();
        ReloadPlayerProperties();
        PlayerManager.GetComponent<BoxCollider2D>().enabled = true;
        PlayerManager.transform.position = References.HouseAddress[House.Hospital.ToString()];
    }

    private void OnApplicationQuit()
    {
        if (References.accountRefer != null)
        {
            Account_DAO.ChangeStateOnline(References.accountRefer.ID, false);
            References.UpdateAccountToDB();
        }
    }
}
