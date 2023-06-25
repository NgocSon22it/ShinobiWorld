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
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using WebSocketSharp;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("Mission")]
    public GameObject MissionItemPrefab;
    public GameObject MissionPanel;
    public GameObject MissionMessage;
    public Transform Content;
    public List<ButtonTrophies> BtnTrophies;

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

    public void GetCurrentMission()
    {
        var filterlist = References.listAccountMission = AccountMission_DAO.GetAllByUserID(References.accountRefer.ID);
        listMission = References.listMission.FindAll(obj => filterlist.Any(filter => filter.MissionID == obj.ID));

        if (filterlist.Any(obj => obj.Status == StatusMission.Doing))
        {
            CurrentMission = filterlist.Find(obj => obj.Status == StatusMission.Doing);
            HavingMission = References.listMission.Find(obj => obj.ID == CurrentMission.MissionID);
        }
    }

    public void ResetColorBtnTrophies()
    {
        foreach (ButtonTrophies button in BtnTrophies)
        {
            button.Btn.GetComponent<Image>().color = new Color32(185, 183, 183, 255);
        }
    }

    public void SelectedColorBtnTrophies(ButtonTrophies button)
    {
        ResetColorBtnTrophies();
        button.Btn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        GetList(button.ID);
    }

    public void Open()
    {
        MissionMessage.SetActive(false);
        Game_Manager.Instance.IsBusy = true;

        foreach (ButtonTrophies button in BtnTrophies)
        {
            button.Btn.GetComponentInChildren<TMP_Text>().text = References.BtnTrophies[button.ID.ToString()];

            button.Btn.onClick.AddListener(() =>
            {
                Debug.Log(button.ID.ToString());
                Debug.Log(References.accountRefer.TrophiesID);
                SelectedColorBtnTrophies(button);
            });
        }

        GetCurrentMission();
        ResetColorBtnTrophies();

        TrophiesID trophiesID = (TrophiesID) Enum.Parse(typeof(TrophiesID), References.accountRefer.TrophiesID);
        BtnTrophies[(int)trophiesID].Btn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);  
        GetList(trophiesID);

        MissionPanel.SetActive(true);
    }

    public void Destroy()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }
    }

    public void GetList(TrophiesID TrophiesID)
    {
        Destroy();
        var list = listMission.FindAll(obj => obj.TrophiesID == TrophiesID.ToString());
        
        if(list.Count <= 0) MissionMessage.SetActive(true);
        else 
        {
            MissionMessage.SetActive(false);
            foreach (var mission in list)
            {
                Instantiate(MissionItemPrefab, Content).GetComponent<MissionItem>().Setup(mission);
            }
        }
        
    }

    public void Reload() {
        GetCurrentMission();
        GetList((TrophiesID)Enum.Parse(typeof(TrophiesID), References.accountRefer.TrophiesID));
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
        Game_Manager.Instance.IsBusy = false;
    }

    public void LoadProgress()
    {
        Game_Manager.Instance.IsBusy = true;
        ProgressPanel.SetActive(true);
        if (HavingMission == null) GetCurrentMission();

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
        Game_Manager.Instance.IsBusy = false;
    }

    public void ShowBonus(int Coin, int Exp, string Name, string Image)
    {
        BonusPanel.SetActive(true);
        CoinTxt.text = Coin.ToString();
        ExpTxt.text = Exp.ToString();
        EquipmentTxt.text = Name;
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
            AccountMission_DAO.DoingMission(References.accountRefer.ID, HavingMission.ID, CurrentMission.Current);
            if (CurrentMission.Current >= CurrentMission.Target)
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
        var equip = References.RandomEquipmentBonus(selected.CategoryEquipmentID);
       
        if(References.accountRefer.TrophiesID == References.TrophyID_RemakeMission)
        {
            AccountMission_DAO.TakeBonus(References.accountRefer.ID, selected.ID, (int)StatusMission.None, equip.ID);
        }
        else AccountMission_DAO.TakeBonus(References.accountRefer.ID, selected.ID, (int) StatusMission.Done, equip.ID);

        References.accountRefer.Coin += selected.CoinBonus;
        Player_AllUIManagement.Instance.SetUpCoinUI(References.accountRefer.Coin);
        References.AddExperience(selected.ExpBonus);

        Reload();

        ShowBonus(selected.CoinBonus, selected.ExpBonus, equip.Name, equip.Image);
    }

    public void Check()
    {
        DoingMission(HavingMission.BossID);
    }

}
[System.Serializable]
public struct ButtonTrophies
{
    public TrophiesID ID;
    public Button Btn;
}