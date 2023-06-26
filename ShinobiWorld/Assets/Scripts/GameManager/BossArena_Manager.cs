using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.XR;
using UnityEngine;

public class BossArena_Manager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text Test;

    public GameObject PlayerInstance;
    // Start is called before the first frame update

    public override void OnJoinedRoom()
    {
        Debug.Log("hello room ");
        Game_Manager.Instance.SetupPlayer(Vector3.zero);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("hello master ");
        Test.text = References.accountRefer.ID;
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 1; // Maximum number of players allowed in the room
            PhotonNetwork.CreateRoom(References.accountRefer.ID, roomOptions, TypedLobby.Default);
        }
    }
}
