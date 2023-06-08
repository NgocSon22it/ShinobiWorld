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

    public int HavingMissionID;

    public List<Mission_Entity> listMission;

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        var filterlist = References.listAccountMission = AccountMission_DAO.GetAllByUserID(References.accountRefer.ID);
        listMission = References.listMission.FindAll(obj => filterlist.Any(filter => filter.MissionID == obj.ID));

        if (filterlist.Any(obj => obj.Status == true))
            HavingMissionID = filterlist.Find(obj => obj.Status == true).MissionID;

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
        var strength = References.accountRefer.CurrentStrength;

        foreach (var mission in listMission)
        {
            var missionManager = MissionItemPrefab.GetComponent<MissionItem>();
            missionManager.ID = mission.ID;
            missionManager.Content.text = mission.Content;
            missionManager.requiedStrength.text = mission.RequiredStrength.ToString();
            missionManager.Trophi.text = References.listTrophy.Find(obj => obj.ID == mission.TrophiesID).Name;

            missionManager.TakeBtn.interactable = true;
            if (strength < mission.RequiredStrength || HavingMissionID != 0) missionManager.TakeBtn.interactable = false;
            
            missionManager.status = false;
            missionManager.TakeBtn.GetComponentInChildren<TMP_Text>().text = "Nhận";

            if (mission.ID == HavingMissionID) 
            { 
                missionManager.status = true;
                missionManager.TakeBtn.interactable = true;
                missionManager.TakeBtn.GetComponentInChildren<TMP_Text>().text = "Hủy bỏ"; 
            }

            
            Instantiate(MissionItemPrefab, Content);
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
