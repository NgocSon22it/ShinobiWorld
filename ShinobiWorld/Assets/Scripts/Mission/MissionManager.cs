using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Assets.Scripts.Mission;
using Assets.Scripts.Shop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    public GameObject MissionItemPrefab, MissionPanel;
    public Transform Content;

    public string HavingMissionID = string.Empty;

    public List<Mission_Entity> listMission;

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        var filterlist = References.listAccountMission = AccountMission_DAO.GetAllByUserID(References.accountRefer.ID);
        listMission = References.listMission.FindAll(obj => filterlist.Any(filter => filter.MissionID == obj.ID));

        if (filterlist.Any(obj => obj.Status == StatusMission.Doing))
            HavingMissionID = filterlist.Find(obj => obj.Status == StatusMission.Doing).MissionID;

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
        foreach (var mission in listMission)
        {
            Instantiate(MissionItemPrefab, Content).GetComponent<MissionItem>().Setup(mission);
        }
    }

    public void Reload() {
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
        MissionPanel.SetActive(false);
    }
}
