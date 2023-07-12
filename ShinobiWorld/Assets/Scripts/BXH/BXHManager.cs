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

    public Button OpenBtn, CLoseBtn;
    public static BXHManager Instance;

    private void Awake()
    {
        Instance = this;
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
        var list = Account_DAO.GetAllAccount();
        list.Sort((s1, s2) => s2.Power.CompareTo(s1.Power)); //descending order


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

    public void Reload()
    {
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
        BXHPanel.SetActive(false);
        Game_Manager.Instance.IsBusy = false;
    }

}
