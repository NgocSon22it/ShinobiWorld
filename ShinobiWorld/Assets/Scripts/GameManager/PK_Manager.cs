using Assets.Scripts.Database.DAO;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PK_Manager : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform Spawnpoint_Player1;
    [SerializeField] Transform Spawnpoint_Player2;

    [SerializeField] PolygonCollider2D CameraBox;

    [Header("Battle Start")]
    float ReadyTime = 3f;

    [SerializeField] TMP_Text Battle_Start_CountdownTxt;



    public static PK_Manager Instance;

    private void Awake()
    {
        Instance = this;
    }
    public override void OnJoinedRoom()
    {
        Game_Manager.Instance.IsBusy = true;
        Game_Manager.Instance.SetupPlayer(Spawnpoint_Player1.position, CameraBox, AccountStatus.PK);
        StartCoroutine(Battle_StartCoroutine());
    }

    private IEnumerator Battle_StartCoroutine()
    {
        float currentTime = ReadyTime;
        while (currentTime > 0)
        {
            Battle_Start_CountdownTxt.text = string.Format("{0}", currentTime);

            yield return new WaitForSeconds(1f);

            currentTime--;
        }

        Battle_Start_CountdownTxt.gameObject.SetActive(false);
        Game_Manager.Instance.IsBusy = false;

    }

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom("2", roomOptions, TypedLobby.Default);
        }
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Game_Manager.Instance.ReloadPlayerProperties();
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
