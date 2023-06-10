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

    [Header("Mission")]
    public GameObject MissionItemPrefab;
    public GameObject MissionPanel;
    public Transform Content;

    [Header("Status")]
    public GameObject MissionUIObject;
    public TMP_Text MissionContent;
    public TMP_Text Status;

    public Mission_Entity HavingMission = null;
    public AccountMission_Entity CurrentMission = null;

    public List<Mission_Entity> listMission;

    private void Awake()
    {
        Instance = this;
    }

    public void Setup()
    {
        var filterlist = References.listAccountMission = AccountMission_DAO.GetAllByUserID(References.accountRefer.ID);
        listMission = References.listMission.FindAll(obj => filterlist.Any(filter => filter.MissionID == obj.ID));

        if (filterlist.Any(obj => obj.Status == StatusMission.Doing))
        {
            CurrentMission = filterlist.Find(obj => obj.Status == StatusMission.Doing);
            HavingMission = References.listMission.Find(obj => obj.ID == CurrentMission.MissionID);
        }
    }

    public void Open()
    {

        Setup();
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

    public void LoadMissionUI()
    {
        MissionUIObject.SetActive(true);
        MissionContent.text = HavingMission.Content;
        LoadStatusUI();
    }

    public void LoadStatusUI()
    {
        if (CurrentMission.Current == CurrentMission.Target) Status.text = Message.MissionFinish;
        else Status.text = string.Format("{0}/{1}", CurrentMission.Current, CurrentMission.Target);
    }

    public void TakeMission(Mission_Entity selected)
    {
        HavingMission = selected;

        References.accountRefer.CurrentStrength -= selected.RequiredStrength;
        Player_AllUIManagement.Instance
            .LoadStrengthUI(References.accountRefer.Strength, References.accountRefer.CurrentStrength);

        var index = References.listAccountMission.FindIndex(obj => obj.MissionID == selected.ID);
        References.listAccountMission[index].Status = StatusMission.Doing;
        CurrentMission = References.listAccountMission[index];

        LoadMissionUI();
        Reload();
    }

    public void CancelMission()
    {
        var index = References.listAccountMission.FindIndex(obj => obj.MissionID == HavingMission.ID);
        References.listAccountMission[index].Status = StatusMission.None;

        HavingMission = null;
        CurrentMission = null;

        MissionUIObject.SetActive(false);
        Reload();
    }

    public void DoingMission(string BossID)
    {
        if(HavingMission != null && BossID == HavingMission.BossID)
        {
            ++CurrentMission.Current;
            LoadStatusUI();
            if(CurrentMission.Current >= CurrentMission.Target)
            {
                AccountMission_DAO.ChangeStatusMission(References.accountRefer.ID, HavingMission.ID, 
                                                            StatusMission.Claim);
                
                HavingMission = null;
                CurrentMission = null;
            }
        }
    }

    public void TakeBonusMission(Mission_Entity selected)
    {
        AccountMission_DAO.TakeBonus(References.accountRefer.ID, selected.ExpBonus, selected.CoinBonus);
        References.accountRefer.Coin += selected.CoinBonus;
        References.accountRefer.Exp += selected.ExpBonus;

        var index = References.listAccountMission.FindIndex(obj => obj.MissionID == selected.ID);
        References.listAccountMission[index].Status = StatusMission.Done;

        Player_AllUIManagement.Instance.SetUpCoinUI(References.accountRefer.Coin);

        Player_AllUIManagement.Instance
            .LoadExperienceUI(References.accountRefer.Level, References.accountRefer.Exp, 
                                References.accountRefer.Level * 100);

        Reload();
    }

    public void Check()
    {
        DoingMission(HavingMission.BossID);
    }

}
