using Assets.Scripts.Database.DAO;
using Assets.Scripts.MailBox;
using Assets.Scripts.Mission;
using System;
using System.Collections;
using System.Collections.Generic;
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
    public Transform Content;

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        Game_Manager.Instance.IsBusy = true;
        MessageTxt.SetActive(false);
        MailBoxPanel.SetActive(true);
        GetList();
        
        if (References.listAccountMailBox.Count <= 0) MessageTxt.SetActive(true);
       
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
        References.listAccountMailBox = AccountMailBox_DAO.GetAllByUserID(References.accountRefer.ID);
        var list = References.listAccountMailBox;

        for (var i = 0; i < list.Count; ++i)
        {
            if (list[i].MailBoxID.Contains(References.MailSystem))
                Instantiate(SystemPrefab, Content)
                    .GetComponent<MailBoxItem>()
                    .Setup(list[i], (i == 0));
            else Instantiate(BXHPrefab, Content).GetComponent<MailBoxItem>().Setup(list[i], (i == 0), false);
        }

    }

    public void Reload()
    {
        Destroy();
        GetList();
    }

    public void ResetColor()
    {
        foreach (Transform child in Content)
        {
            child.gameObject.GetComponent<Image>().color = new Color32(110, 80, 60, 255);
        }
    }

    public void Close()
    {
        Destroy();
        MailBoxPanel.SetActive(false);
        Game_Manager.Instance.IsBusy = false;
    }
}
