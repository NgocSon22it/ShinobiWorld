using Assets.Scripts.Database.DAO;
using Assets.Scripts.MailBox;
using Assets.Scripts.Mission;
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

    public GameObject MailBoxPanel;
    public GameObject SystemPrefab;
    public GameObject BXHPrefab;
    public GameObject MessageTxt;
    public GameObject ScrollView;
    public GameObject Detail;
    public GameObject DeleteReadBtn;
    public Transform Content;
    public GameObject ConfirmDeletePanel;
    public TMP_Text ConfirmDeleteMessage;

    private void Awake()
    {
        Instance = this;
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
        References.listAccountMailBox = AccountMailBox_DAO.GetAllByUserID(References.accountRefer.ID);
        var list = References.listAccountMailBox;

        for (var i = 0; i < list.Count; ++i)
        {
            if (list[i].MailBoxID.Contains(References.MailSystem))
                Instantiate(SystemPrefab, Content)
                    .GetComponent<MailBoxItem>()
                    .Setup(list[i], (list[i].ID == ID)? true: (i == 0));
            else Instantiate(BXHPrefab, Content).GetComponent<MailBoxItem>().Setup(list[i], (list[i].ID == ID) ? true : (i == 0), false);
        }

        if (References.listAccountMailBox.Count <= 0)
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
            
            if(child.gameObject.GetComponent<MailBoxItem>().accountMail.IsRead)
                child.gameObject.GetComponent<Image>().color = new Color32(110, 80, 60, 150);
        }
    }
    public void ConfirmDelete()
    {
        ConfirmDeletePanel.SetActive(true);

        var isClaim = References.listAccountMailBox.Any(obj => !obj.IsClaim);

        if (isClaim) ConfirmDeleteMessage.text = Message.MailboxDeleteNotReceivedBonus;
        else ConfirmDeleteMessage.text = Message.MailboxDelete;
    }

    public void CloseConfirmDelete()
    {
        ConfirmDeletePanel.SetActive(false);
    }

    public void DeleteReadAndReceivedBonus()
    {
        AccountMailBox_DAO.DeleteReadAndReceivedBonus(References.accountRefer.ID);
        CloseConfirmDelete();
        Reload(0);
    }

    public void DeleteReadAll()
    {
        AccountMailBox_DAO.DeleteReadAll(References.accountRefer.ID); 
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
