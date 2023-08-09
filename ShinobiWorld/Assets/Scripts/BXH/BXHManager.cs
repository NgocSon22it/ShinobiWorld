using Assets.Scripts.BXH;
using Assets.Scripts.Database.DAO;
using Assets.Scripts.Mission;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class BXHManager : MonoBehaviour
{
    public GameObject BXHPanel, BXHItemPrefab, BXHItemFriendPrefab, BXHItemRequestPrefab, BXHItemSendRequestPrefab, BXHMessage;
    public Transform Content;

    public Button OpenBtn, CloseBtn;
    public static BXHManager Instance;

    List<Account_Entity> list = new List<Account_Entity> ();
    List<string> listFriend, listRequest, listSendRequest;
    private void Awake()
    {
        Instance = this;

        OpenBtn.onClick.AddListener(Open);
        CloseBtn.onClick.AddListener(Close);
    }

    private void Start()
    {
        if (DateTime.Today.Day == 1 )
        {
            SortRank();
            SendMailRank();
        }
        References.listMailBox = MailBox_DAO.GetAllByUserID(References.accountRefer.ID);

    }
    public void Open()
    {
        BXHMessage.SetActive(false);
        Game_Manager.Instance.IsBusy = true;


        GetList();

        BXHPanel.SetActive(true);
    }

    public void Destroy()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }
    }

    public void GetList()
    {
        Destroy();
        SortRank();
        if (list.Count <= 0) BXHMessage.SetActive(true);
        else
        {
            BXHMessage.SetActive(false);
            InitFriendList();
            for (var i = 0; i < list.Count;  ++i)
            {
                if (listFriend.Contains(list[i].ID))
                    Instantiate(BXHItemFriendPrefab, Content).GetComponent<BXHItem>().Setup(i+1, list[i]);
                else if (listRequest.Contains(list[i].ID))
                    Instantiate(BXHItemRequestPrefab, Content).GetComponent<BXHItem>().Setup(i + 1, list[i]);
                else if (listSendRequest.Contains(list[i].ID))
                    Instantiate(BXHItemSendRequestPrefab, Content).GetComponent<BXHItem>().Setup(i + 1, list[i]);
                else 
                    Instantiate(BXHItemPrefab, Content).GetComponent<BXHItem>().Setup(i + 1, list[i]);
            }
        }

    }

    public void InitFriendList()
    {
        var list = References.listAllFriend = Friend_DAO.GetAll(References.accountRefer.ID);
        listFriend = new List<string>();
        listRequest = new List<string>();
        listSendRequest = new List<string>();

        var MyID = References.accountRefer.ID;
        foreach (var friend in list)
        {
            if (friend.IsFriend)
            {
                var FriendAccountID = (friend.FriendAccountID + friend.MyAccountID).Replace(MyID, "");
                if (!listFriend.Contains(FriendAccountID)) listFriend.Add(FriendAccountID);
            }
            else
            {
                if (friend.MyAccountID == MyID)
                {
                    if (!listRequest.Contains(friend.FriendAccountID))
                    listRequest.Add(friend.FriendAccountID);
                }
                else if (friend.FriendAccountID == MyID)
                {
                    if (!listSendRequest.Contains(friend.MyAccountID))
                        listSendRequest.Add(friend.MyAccountID);
                }
            }      
        }

        //References.listFriendInfo = Friend_DAO.GetAllFriendInfo(listFriend);
        //References.listRequestInfo = Friend_DAO.GetAllFriendInfo(listRequest);

    }

    public void SortRank()
    {
        list = Account_DAO.GetAllAccount();
        list.Sort((s1, s2) => s2.Power.CompareTo(s1.Power)); //descending order
    }

    public void Close()
    {
        Destroy();
        BXHPanel.SetActive(false);
        Game_Manager.Instance.IsBusy = false;
    }

    public void SendMailRank()
    {
        for(var i = 0; i < 3 ; ++i)
        {
            if (!MailBox_DAO.CheckSentMailRank(list[i].ID)) MailBox_DAO.AddMailbox(list[i].ID, References.listMail[i+1].ID, false);
        }
    }

}
