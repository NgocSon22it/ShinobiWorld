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

public class Game_Manager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerManager;

    [SerializeField] GameObject PlayerMelee;
    [SerializeField] GameObject PlayerRange;
    [SerializeField] GameObject PlayerSupport;

    public static Game_Manager Instance;

    ExitGames.Client.Photon.Hashtable PlayerProperties = new ExitGames.Client.Photon.Hashtable();
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
        
        PhotonPeer.RegisterType(typeof(Account_Entity), (byte) 'A', Account_Entity.Serialize, Account_Entity.Deserialize);
        SetupPlayer(References.HouseAddress[House.Hokage.ToString()]);
    }

    public void SetupPlayer(Vector3 position)
    {
        if (PlayerManager == null && PhotonNetwork.IsConnectedAndReady)
        {
            switch (References.accountRefer.RoleInGameID)
            {
                case "Role_Melee":
                    PlayerManager = PhotonNetwork.Instantiate("Player/Melee/" + Path.Combine(PlayerMelee.name), position, Quaternion.identity);
                    PlayerManager.GetComponent<PlayerBase>().SetUpAccountWeaponName("Weapon_Sword");
                    PlayerManager.GetComponent<PlayerBase>().SetUpAccountSkillName("Skill_MeleeOne", "Skill_MeleeTwo", "Skill_MeleeThree");
                    break;
                case "Role_Range":
                    PlayerManager = PhotonNetwork.Instantiate("Player/Range/" + Path.Combine(PlayerRange.name), position, Quaternion.identity);
                    PlayerManager.GetComponent<PlayerBase>().SetUpAccountWeaponName("Weapon_Dart");
                    PlayerManager.GetComponent<PlayerBase>().SetUpAccountSkillName("Skill_RangeOne", "Skill_RangeTwo", "Skill_RangeThree");
                    break;
                case "Role_Support":
                    PlayerManager = PhotonNetwork.Instantiate("Player/Support/" + Path.Combine(PlayerSupport.name), position, Quaternion.identity);
                    PlayerManager.GetComponent<PlayerBase>().SetUpAccountWeaponName("Weapon_Glove");
                    PlayerManager.GetComponent<PlayerBase>().SetUpAccountSkillName("Skill_SupportOne", "Skill_SupportTwo", "Skill_SupportThree");
                    break;
            }
            ReloadPlayerProperties();
            Debug.Log("Successfully joined room S1!");
        }
    }

    public void ReloadPlayerProperties()
    {
        string AccountJson = JsonUtility.ToJson(References.accountRefer);
        PlayerProperties["Account"] = AccountJson;

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
        References.accountRefer.CurrentCharka = References.accountRefer.Charka;
        PlayerManager.GetComponent<PlayerBase>().CallInvoke();
        ReloadPlayerProperties();
        PlayerManager.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnApplicationQuit()
    {
        Account_DAO.ChangeStateOnline(References.accountRefer.ID, false);
    }
}
