using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Photon.Pun;
using Photon.Realtime;
using System;
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

    public void SendInvite(string receiverName)
    {
        switch (References.inviteType)
        {
            case InviteType.PK:

                ChatManager.Instance.chatClient
               .SendPrivateMessage(receiverName,
               string.Format(Message.PrivateMessage, TypePrivateMessage.PK.ToString(),
               Message.PKMessage, References.MapInviteInvite, References.RoomNameInvite, References.PKBet));

                break;

            case InviteType.Arena:
                ChatManager.Instance.chatClient
               .SendPrivateMessage(receiverName,
               string.Format(Message.PrivateMessage, TypePrivateMessage.Arena.ToString(),
               Message.BossAreaMessage, References.MapInviteInvite, References.RoomNameInvite, References.BossNameInvite));
                break;
        }
    }

    public void LoadListInvite()
    {
        ListInvite = Account_DAO.GetAllAccountForInvite();

        foreach (Transform trans in Content)
        {
            Destroy(trans.gameObject);
        }

        foreach (Account_Entity account in ListInvite)
        {
            if (!account.ID.Equals(References.accountRefer.ID))
            {
                Instantiate(Item, Content).GetComponent<InviteItem>().SetUp(account);
            }
        }
    }

    public void OpenReceiveInvitePopup_Arena(TypePrivateMessage type, string Content, string SceneName, string RoomName, string BossName)
    {
        if (!ReceivePanel.activeInHierarchy && Player_AllUIManagement.Instance.Player.accountStatus == AccountStatus.Normal
            && !References.RoomNameInvite.Equals(RoomName))
        {
            this.type = type;

            References.MapInviteInvite = SceneName;
            References.RoomNameInvite = RoomName;
            InviteContent.text = Content + " " + BossName;
            ReceivePanel.SetActive(true);
            StartCoroutine(PopupInvite());
        }
    }

    public void OpenReceiveInvitePopup_PK(TypePrivateMessage type, string Content, string SceneName, string RoomName, string Bet)
    {
        if (!ReceivePanel.activeInHierarchy && Player_AllUIManagement.Instance.Player.accountStatus == AccountStatus.Normal
            && !References.RoomNameInvite.Equals(RoomName))
        {
            this.type = type;
            References.MapInviteInvite = SceneName;
            References.RoomNameInvite = RoomName;
            References.PKBet = Convert.ToInt32(Bet);
            InviteContent.text = Content + " " + Bet + " Vàng.";
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
                PhotonNetwork.LoadLevel(References.MapInviteInvite);
                break;

            case TypePrivateMessage.PK:
                if (References.accountRefer.Coin >= References.PKBet)
                {
                    References.AddCoin(-References.PKBet);
                    References.IsInvite = true;
                    PhotonNetwork.IsMessageQueueRunning = false;
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel(References.MapInviteInvite);
                }
                break;
        }
        CloseReceiveInvitePopup();
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
