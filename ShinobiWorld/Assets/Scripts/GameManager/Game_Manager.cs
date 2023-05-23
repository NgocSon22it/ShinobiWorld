using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.TextCore.Text;
using Assets.Scripts.Database.DAO;

public class Game_Manager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerManager;

    [SerializeField] GameObject PlayerMelee;
    [SerializeField] GameObject PlayerRange;
    [SerializeField] GameObject PlayerSupport;

    public static Game_Manager Instance;
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
        if (PlayerManager == null && PhotonNetwork.IsConnectedAndReady)
        {
            switch (References.accountRefer.RoleInGameID)
            {
                case "Role_Melee":
                    PlayerManager = PhotonNetwork.Instantiate("Player/" + Path.Combine(PlayerMelee.name), new(0, 0, 0), Quaternion.identity);
                    break;
                case "Role_Range":
                    PlayerManager = PhotonNetwork.Instantiate("Player/" + Path.Combine(PlayerRange.name), new(0, 0, 0), Quaternion.identity);
                    break;
                case "Role_Support":
                    PlayerManager = PhotonNetwork.Instantiate("Player/" + Path.Combine(PlayerSupport.name), new(0, 0, 0), Quaternion.identity);
                    break;
            }


            Debug.Log("Successfully joined room S1!");
        }
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed");
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

    private void OnApplicationQuit()
    {
        Account_DAO.ChangeStateOnline(References.accountRefer.ID, false);
    }
}