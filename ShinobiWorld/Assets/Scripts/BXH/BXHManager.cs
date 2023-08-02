using Assets.Scripts.BXH;
using Assets.Scripts.Database.DAO;
using Assets.Scripts.Mission;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BXHManager : MonoBehaviour
{
    public GameObject BXHPanel, BXHItemPrefab, BXHMessage;
    public Transform Content;

    public Button OpenBtn, CloseBtn;
    public static BXHManager Instance;

    List<Account_Entity> list = new List<Account_Entity> ();

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
            for (var i = 0; i < list.Count;  ++i)
            {
                Instantiate(BXHItemPrefab, Content).GetComponent<BXHItem>().Setup(i+1, list[i]);
            }
        }

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
