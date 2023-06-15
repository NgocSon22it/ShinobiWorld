﻿using Assets.Scripts.Database.DAO;
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
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using WebSocketSharp;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("Mission")]
    public GameObject MissionItemPrefab;
    public GameObject MissionPanel;
    public Transform Content;

    [Header("Progress")]
    public GameObject ProgressPanel;
    public TMP_Text ContentTxt;
    public TMP_Text CurrentTxt;
    public Button ProgressBtn;

    [Header("Bonus")]
    public GameObject BonusPanel;
    public TMP_Text CoinTxt;
    public TMP_Text ExpTxt;
    public TMP_Text EquipmentTxt;
    public Image EquipmentImg;

    [Header("BonusEquipDupli")]
    public GameObject BonusEquipDupliPanel;
    public TMP_Text MessageTxt;
    public TMP_Text SellCostTxt;
    public Image EquipmentDupliImg;

    public Mission_Entity HavingMission = null;
    public AccountMission_Entity CurrentMission = null;

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
        {
            CurrentMission = filterlist.Find(obj => obj.Status == StatusMission.Doing);
            HavingMission = References.listMission.Find(obj => obj.ID == CurrentMission.MissionID);
        }

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

    public void LoadProgress()
    {
        ProgressPanel.SetActive(true);

        if (HavingMission != null)
        {
            ContentTxt.text = HavingMission.Content;

            if (CurrentMission.Current == CurrentMission.Target) CurrentTxt.text = Message.MissionFinish;
            else CurrentTxt.text = string.Format(Message.MissionProgress, CurrentMission.Current, CurrentMission.Target);
        }else
        {
            CurrentTxt.text = "";
            ContentTxt.text = Message.MissionNone;
        }
    }

    public void CloseProgress()
    {
        ProgressPanel.SetActive(false);
    }

    public void ShowBonus(int Coin, int Exp, string Name, string Image)
    {
        BonusPanel.SetActive(true);
        CoinTxt.text = Coin.ToString();
        ExpTxt.text = Exp.ToString();
        EquipmentTxt.text = Name;
        Debug.Log(Image);
        EquipmentImg.sprite = Resources.Load<Sprite>(Image);
    }

    public void CloseBonus()
    {
        BonusPanel.SetActive(false);
    }

    public void ShowMessageEquipmetDuplicate(int Coin, string Name, string Image)
    {
        BonusEquipDupliPanel.SetActive(true);
        MessageTxt.text = string.Format(Message.MissionBonusEquipDupli, Name);
        SellCostTxt.text = Coin.ToString();
        Debug.Log(Image);

        EquipmentDupliImg.sprite = Resources.Load<Sprite>(Image);
    }

    public void CloseMessageEquipmetDuplicate()
    {
        BonusEquipDupliPanel.SetActive(false);
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

        Reload();
    }

    public void CancelMission()
    {
        var index = References.listAccountMission.FindIndex(obj => obj.MissionID == HavingMission.ID);
        References.listAccountMission[index].Status = StatusMission.None;

        HavingMission = null;
        CurrentMission = null;

        Reload();
    }

    public void DoingMission(string BossID)
    {
        if(HavingMission != null && BossID == HavingMission.BossID)
        {
            ++CurrentMission.Current;
            if(CurrentMission.Current >= CurrentMission.Target)
            {
                AccountMission_DAO.ChangeStatusMission(References.accountRefer.ID, HavingMission.ID, 
                                                            StatusMission.Claim);
                LoadProgress();
                HavingMission = null;
                CurrentMission = null;
            }
        }
    }

    public void TakeBonusMission(Mission_Entity selected)
    {
        References.listAccountEquipment = AccountEquipment_DAO.GetAllByUserID(References.accountRefer.ID);
        var equip = References.RandomEquipmentBonus(selected.CategoryEquipmentID, out int SellCost);
       
        AccountMission_DAO.TakeBonus(References.accountRefer.ID, selected.ExpBonus, selected.CoinBonus+SellCost, equip.ID);

        References.accountRefer.Coin += selected.CoinBonus;
        References.accountRefer.Exp += selected.ExpBonus;
        

        var index = References.listAccountMission.FindIndex(obj => obj.MissionID == selected.ID);
        References.listAccountMission[index].Status = StatusMission.Done;

        Player_AllUIManagement.Instance.SetUpCoinUI(References.accountRefer.Coin);

        Player_AllUIManagement.Instance
            .LoadExperienceUI(References.accountRefer.Level, References.accountRefer.Exp, 
                                References.accountRefer.Level * 100);

        Reload();

        ShowBonus(selected.CoinBonus, selected.ExpBonus, equip.Name, equip.Image);
            
        if (SellCost > 0)
        {
            ShowMessageEquipmetDuplicate(SellCost, equip.Name, equip.Image);
        }
    }

    public void Check()
    {
        DoingMission(HavingMission.BossID);
    }

}
