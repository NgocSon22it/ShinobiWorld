using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Assets.Scripts.Mission;
using Assets.Scripts.Shop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    public GameObject MissionItemPrefab, MissionPanel;
    public Transform Content;

    public string AccountID;

    List<Mission_Entity> listMission;

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        References.accountRefer = Account_DAO.GetAccountByID("vRsLqEXrnhMpK48YRLlYMNBElTf1");

        var filterlist = References.listAccountMission = AccountMission_DAO.GetAllByUserID(References.accountRefer.ID);
        listMission = References.listMission.FindAll(obj => filterlist.Any(filter => filter.MissionID == obj.ID));
        
        MissionPanel.SetActive(true);
        GetList();
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
        foreach (var item in listMission)
        {
            var itemManager = MissionItemPrefab.GetComponent<MissionItem>();
            itemManager.ID = item.ID;
            itemManager.Content.text = item.Content;
            itemManager.requiedStrength.text = item.RequiredStrength.ToString();
            itemManager.Trophi.text = References.listTrophy.Find(obj => obj.ID == item.TrophiesID).Name;
            Instantiate(MissionItemPrefab, Content);
        }
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
        MissionPanel.SetActive(false);
    }
}
