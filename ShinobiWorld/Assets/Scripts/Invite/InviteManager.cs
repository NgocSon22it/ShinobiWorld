﻿using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InviteManager : MonoBehaviour
{
    public GameObject ReceivePanel, SendPanel, ListInvitePanel;

    public TMP_Text InviteContent, CountDownPopup;

    public Button AcceptBtn, DenyBtn, CloseAcceptPanel;

    TypePrivateMessage type;

    [Header("Invite List")]
    [SerializeField] GameObject Item;
    [SerializeField] Transform Content;

    public List<Account_Entity> ListInvite = new List<Account_Entity>();

    public static InviteManager Instance;

    int SecondsPopup;

    private void Awake()
    {
        Instance = this;
    }


    public void SendArenaMessage(string receiverName)
    {
        ChatManager.Instance.chatClient
               .SendPrivateMessage(receiverName,
               string.Format(Message.PrivateMessage, TypePrivateMessage.Arena.ToString(),
               Message.BossAreaMessage, References.SceneNameInvite, PhotonNetwork.CurrentRoom.Name));
    }


    public void LoadListInvite()
    {
        ListInvite = Account_DAO.GetAllAccountForInvite();

        foreach (Transform trans in Content)
        {
            Destroy(trans.gameObject);
        }

        foreach (Account_Entity a in ListInvite)
        {
            if (!a.ID.Equals(References.accountRefer.ID))
            {
                Instantiate(Item, Content).GetComponent<InviteItem>().SetUp(a);
            }
        }
    }

    public void OpenReceiveInvitePopup(TypePrivateMessage type, string Content, string SceneName, string RoomName)
    {
        if (!ReceivePanel.activeInHierarchy && Player_AllUIManagement.Instance.Player.accountStatus == AccountStatus.Normal)
        {
            this.type = type;
            References.SceneNameInvite = SceneName;
            References.RoomNameInvite = RoomName;
            InviteContent.text = Content;
            ReceivePanel.SetActive(true);
            StartCoroutine(PopupInvite());
        }
    }

    public void CloseReceiveInvitePopup()
    {
        ReceivePanel.SetActive(false);
        StopAllCoroutines();
    }

    public void OpenSendInvitePanel()
    {
        LoadListInvite();
        Game_Manager.Instance.IsBusy = true;
        ListInvitePanel.SetActive(true);
    }

    public void CloseSendInvitePanel()
    {
        Game_Manager.Instance.IsBusy = false;
        ListInvitePanel.SetActive(false);
    }

    public void AccpectInvite()
    {
        switch (type)
        {
            case TypePrivateMessage.Arena:
                References.IsInvite = true;
                PhotonNetwork.IsMessageQueueRunning = false;
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel(References.SceneNameInvite);
                break;
        }
    }



    IEnumerator PopupInvite()
    {
        SecondsPopup = 15;
        while (SecondsPopup > 0)
        {
            CountDownPopup.text = string.Format("{0}", SecondsPopup);

            yield return new WaitForSeconds(1f);

            SecondsPopup--;
        }

        CloseReceiveInvitePopup();
    }
}