using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviourPunCallbacks
{

    [SerializeField] GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        int a = Random.Range(0, 100);
        PhotonNetwork.NickName = a.ToString();
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnConnectedToMaster()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 0; // Maximum number of players allowed in the room
        roomOptions.CleanupCacheOnLeave = false;
        PhotonNetwork.JoinOrCreateRoom("S1", roomOptions, TypedLobby.Default);
    }

    // Callback function for successful join to a Photon room
    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player/"+Path.Combine(Player.name), new(0,0,0), Quaternion.identity);
        Debug.Log("Successfully joined room S1!");
    }
}
