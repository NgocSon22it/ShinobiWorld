using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Error_Manager : MonoBehaviour
{
    public void BackToKonoha()
    {
        if (PhotonNetwork.InRoom)
        {           
            PhotonNetwork.LeaveRoom();
        }
        PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.LoadLevel(Scenes.Konoha);
    }

    public void BackToMenu()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.LoadLevel(Scenes.Login);
    }
}
