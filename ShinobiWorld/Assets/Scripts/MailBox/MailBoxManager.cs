using Assets.Scripts.Database.DAO;
using Assets.Scripts.MailBox;
using Assets.Scripts.Mission;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MailBoxManager : MonoBehaviour
{
    public static MailBoxManager Instance;

    [Header("Mailbox")]
    public GameObject MailBoxPanel;
    public GameObject SystemPrefab;
    public GameObject BXHPrefab;
    public GameObject MessageTxt;
    public GameObject ScrollView;
    public GameObject Detail;
    public GameObject DeleteReadBtn;
    public Button CloseBtn, OpenBtn;
    public Transform Content;


    [Header("ConfirmDelete")]
    public GameObject ConfirmDeletePanel;
    public TMP_Text ConfirmDeleteMessage;
    public Button CancelBtn, CloseConfirmBtn, DeleteReceivedBtn, DeleteReadAllBtn;


    private void Awake()
    {
        Instance = this;
        OpenBtn.onClick.AddListener(Open);
        
        DeleteReadBtn.GetComponent<Button>().onClick.AddListener(ConfirmDelete);
        CloseBtn.onClick.AddListener(Close);

        CancelBtn.GetComponent<Button>().onClick.AddListener(CloseConfirmDelete);
        CloseConfirmBtn.GetComponent<Button>().onClick.AddListener(CloseConfirmDelete);
        DeleteReceivedBtn.GetComponent<Button>().onClick.AddListener(DeleteReadAndReceivedBonus );
        DeleteReadAllBtn.GetComponent<Button>().onClick.AddListener(DeleteReadAll);

    }

    public void Open()
    {
        Game_Manager.Instance.IsBusy = true;
        MessageTxt.SetActive(false);
        MailBoxPanel.SetActive(true);
        ScrollView.SetActive(true);
        Detail.SetActive(true);
        DeleteReadBtn.SetActive(true);
        GetList(0);
    }

    public void Destroy()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }
    }

    public void GetList(int ID)
    {
        var list = References.listMailBox = MailBox_DAO.GetAllByUserID(References.accountRefer.ID);

        for (var i = 0; i < list.Count; ++i)
        {
            if (list[i].MailID.Contains(References.MailSystem))
                Instantiate(SystemPrefab, Content)
                    .GetComponent<MailBoxItem>()
                    .Setup(list[i], (list[i].ID == ID) ? true : (i == 0));
            else Instantiate(BXHPrefab, Content).GetComponent<MailBoxItem>()
                    .Setup(list[i], (list[i].ID == ID) ? true : (i == 0), false);
        }

        if (list.Count <= 0)
        {
            MessageTxt.SetActive(true);
            ScrollView.SetActive(false);
            Detail.SetActive(false);
            DeleteReadBtn.SetActive(false);
        }
    }

    public void Reload(int ID)
    {
        Destroy();
        GetList(ID);
    }

    public void ResetColor()
    {
        foreach (Transform child in Content)
        {
            child.gameObject.GetComponent<Image>().color = new Color32(110, 80, 60, 255);

            if (child.gameObject.GetComponent<MailBoxItem>().selectedMailbox.IsRead)
                child.gameObject.GetComponent<Image>().color = new Color32(110, 80, 60, 150);
        }
    }
    public void ConfirmDelete()
    {
        ConfirmDeletePanel.SetActive(true);

        var isClaim = References.listMailBox.Any(obj => !obj.IsClaim);

        if (isClaim) ConfirmDeleteMessage.text = Message.MailboxDeleteNotReceivedBonus;
        else ConfirmDeleteMessage.text = Message.MailboxDelete;
    }

    public void CloseConfirmDelete()
    {
        ConfirmDeletePanel.SetActive(false);
    }

    public void DeleteReadAndReceivedBonus()
    {
        MailBox_DAO.DeleteReadAndReceivedBonus(References.accountRefer.ID);
        CloseConfirmDelete();
        Reload(0);
    }

    public void DeleteReadAll()
    {
        MailBox_DAO.DeleteReadAll(References.accountRefer.ID);
        CloseConfirmDelete();
        Reload(0);
    }

    public void Close()
    {
        Destroy();
        MailBoxPanel.SetActive(false);
        Game_Manager.Instance.IsBusy = false;
    }
}
