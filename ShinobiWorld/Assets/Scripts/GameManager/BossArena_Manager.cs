using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossArena_Manager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text countdownText;

    [SerializeField] GameObject Boss;

    [SerializeField] Transform SpawnPoint;

    [Header("Time")]
    float TotalTime = 180f;

    public GameObject PlayerInstance;

    private void Start()
    {
        //StartCoroutine(StartCountdown());
    }

    public override void OnJoinedRoom()
    {
        Game_Manager.Instance.IsBusy = true;
        Game_Manager.Instance.SetupPlayer(SpawnPoint.position);
        StartCoroutine(Test());

    }

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 1; // Maximum number of players allowed in the room
            PhotonNetwork.CreateRoom(References.accountRefer.ID, roomOptions, TypedLobby.Default);
        }
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(3f);
        Game_Manager.Instance.IsBusy = false;
        Boss.SetActive(true);

    }

    private IEnumerator StartCountdown()
    {
        float currentTime = TotalTime;
        int minutes, seconds;
        while (currentTime > 0)
        {
            minutes = Mathf.FloorToInt(currentTime / 60);
            seconds = Mathf.FloorToInt(currentTime % 60);

            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return new WaitForSeconds(1f);

            currentTime--;
        }

        countdownText.text = "00:00";
    }

}
