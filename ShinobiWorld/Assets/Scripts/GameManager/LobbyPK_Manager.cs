using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPK_Manager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject havePlayer;
    [SerializeField] GameObject waitPlayer;

    [SerializeField] GameObject StartBtn;
    [SerializeField] GameObject ReadyBtn;
    [SerializeField] GameObject UnReadyBtn;

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            roomOptions.BroadcastPropsChangeToAll = true;
            PhotonNetwork.CreateRoom("2", roomOptions, TypedLobby.Default);
        }
    }






}
