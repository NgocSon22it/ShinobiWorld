using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Assets.Scripts.Mission;
using Assets.Scripts.Shop;
using Photon.Pun;
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
    public List<ButtonTrophy> BtnTrophy;

    [Header("Progress")]
    public GameObject ProgressPanel;
    public TMP_Text ContentTxt;
    public TMP_Text CurrentTxt;
    public Button ProgressBtn;

    [Header("Teleport")]
    public GameObject TeleportPanel;
    public TMP_Text Teleport_ContentTxt;
    public TMP_Text Teleport_CurrentTxt;
    public TMP_Dropdown Teleport_DropDown;
    public List<Area_Entity> listArea = new List<Area_Entity>();
    public Area_Entity SelectedArea;

    [Header("Bonus")]
    public GameObject BonusPanel;
    public TMP_Text CoinTxt;
    public TMP_Text ExpTxt;
    public TMP_Text EquipmentTxt;
    public Image EquipmentImg;

    public Mission_Entity HavingMission = null;
    public HasMission_Entity CurrentMission = null;

    public List<Mission_Entity> listMission;

    TrophyID TrophyID;

    private void Awake()
    {
        Instance = this;
    }

    public void GetCurrentMission()
    {
        Player_AllUIManagement.Instance.CloseMission();

        var filterlist = References.listHasMission = HasMission_DAO.GetAllByUserID(References.accountRefer.ID);
        listMission = References.listMission.FindAll(obj => filterlist.Any(filter => filter.MissionID == obj.ID));

        if (filterlist.Any(obj => obj.Status == StatusMission.Doing))
        {
            CurrentMission = filterlist.Find(obj => obj.Status == StatusMission.Doing);
            HavingMission = References.listMission.Find(obj => obj.ID == CurrentMission.MissionID);
            Player_AllUIManagement.Instance.ShowMission(HavingMission.Content);
        }
    }

    public void ResetColorBtnTrophy()
    {
        foreach (ButtonTrophy button in BtnTrophy)
        {
            button.Btn.GetComponent<Image>().color = new Color32(185, 183, 183, 255);
        }
    }

    public void SelectedColorBtnTrophy(ButtonTrophy button)
    {
        ResetColorBtnTrophy();
        button.Btn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        TrophyID = button.ID;
        GetList(button.ID);
    }

    public void Open()
    {
        MissionMessage.SetActive(false);
        Game_Manager.Instance.IsBusy = true;

        foreach (ButtonTrophy button in BtnTrophy)
        {
            button.Btn.GetComponentInChildren<TMP_Text>().text = References.BtnTrophy[button.ID.ToString()];

            button.Btn.onClick.AddListener(() =>
            {
                SelectedColorBtnTrophy(button);
            });
        }

        GetCurrentMission();
        ResetColorBtnTrophy();

        TrophyID = (TrophyID)Enum.Parse(typeof(TrophyID), References.accountRefer.TrophyID);
        BtnTrophy[(int)TrophyID].Btn.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        GetList(TrophyID);

        MissionPanel.SetActive(true);
    }

    public void Destroy()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }
    }

    public void GetList(TrophyID TrophyID)
    {
        Destroy();
        var list = listMission.FindAll(obj => obj.TrophyID == TrophyID.ToString());

        if (list.Count <= 0) MissionMessage.SetActive(true);
        else
        {
            MissionMessage.SetActive(false);
            foreach (var mission in list)
            {
                Instantiate(MissionItemPrefab, Content).GetComponent<MissionItem>().Setup(mission);
            }
        }

    }

    public void Reload()
    {
        GetCurrentMission();
        GetList(TrophyID);
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

            if (CurrentMission.Current == CurrentMission.Target)
            {
                CurrentTxt.text = Message.MissionFinish;
                Player_AllUIManagement.Instance.CloseMission();
            }
            else CurrentTxt.text = string.Format(Message.MissionProgress, CurrentMission.Current, CurrentMission.Target);
        }
        else
        {
            CurrentTxt.text = "";
            ContentTxt.text = Message.MissionNone;
        }
    }

    public void InitDropdown()
    {
        listArea = Area_DAO.GetAllAreaByMissionID(CurrentMission.MissionID);
        List<string> options = new List<string>();
        options.Clear();

        foreach (Area_Entity area in listArea)
        {
            options.Add(area.Name);
        }

        Teleport_DropDown.ClearOptions();
        Teleport_DropDown.AddOptions(options);

        if (Teleport_DropDown.options.Count > 0)
        {
            OnDropdownValueChanged(0);
            Teleport_DropDown.value = 0;
        }
    }

    public void OnDropdownValueChanged(int index)
    {
        if (index >= 0 && index < listArea.Count)
        {
            SelectedArea = listArea[index];
        }
    }
    public void TeleportToSelectArea()
    {
        Game_Manager.Instance.IsBusy = false;
        References.PlayerSpawnPosition = new Vector3(SelectedArea.XPosition, SelectedArea.YPosition, 0);
        PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(SelectedArea.MapID);
    }

    public void LoadTeleportMission()
    {
        Game_Manager.Instance.IsBusy = true;
        TeleportPanel.SetActive(true);
        if (HavingMission == null) GetCurrentMission();

        if (HavingMission != null)
        {
            InitDropdown();
            Teleport_ContentTxt.text = HavingMission.Content;

            if (CurrentMission.Current == CurrentMission.Target)
            {
                Teleport_CurrentTxt.text = Message.MissionFinish;
                Player_AllUIManagement.Instance.CloseMission();
            }
            else Teleport_CurrentTxt.text = string.Format(Message.MissionProgress, CurrentMission.Current, CurrentMission.Target);
        }
        else
        {
            Teleport_CurrentTxt.text = "";
            Teleport_ContentTxt.text = Message.MissionNone;
        }
    }

    public void CloseProgress()
    {
        ProgressPanel.SetActive(false);
        Game_Manager.Instance.IsBusy = false;
    }
    public void CloseTeleportMission()
    {
        TeleportPanel.SetActive(false);
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

    public void TakeMission(Mission_Entity selected)
    {
        HavingMission = selected;

        References.accountRefer.CurrentStrength -= selected.RequiredStrength;
        Player_AllUIManagement.Instance
            .LoadStrengthUI(References.accountRefer.Strength, References.accountRefer.CurrentStrength);

        var index = References.listHasMission.FindIndex(obj => obj.MissionID == selected.ID);
        References.listHasMission[index].Status = StatusMission.Doing;
        CurrentMission = References.listHasMission[index];

        Reload();
    }

    public void CancelMission()
    {
        var index = References.listHasMission.FindIndex(obj => obj.MissionID == HavingMission.ID);
        References.listHasMission[index].Status = StatusMission.None;

        HavingMission = null;
        CurrentMission = null;

        Reload();
    }

    public void DoingMission(string EnemyID)
    {
        if (HavingMission != null && EnemyID == HavingMission.EnemyID)
        {
            ++CurrentMission.Current;
            HasMission_DAO.DoingMission(References.accountRefer.ID, HavingMission.ID, CurrentMission.Current);
            if (CurrentMission.Current >= CurrentMission.Target)
            {
                HasMission_DAO.ChangeStatusMission(References.accountRefer.ID, HavingMission.ID,
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

        if (References.accountRefer.TrophyID == References.TrophyID_RemakeMission)
        {
            HasMission_DAO.TakeBonus(References.accountRefer.ID, selected.ID, (int)StatusMission.None, equip.ID);
        }
        else HasMission_DAO.TakeBonus(References.accountRefer.ID, selected.ID, (int)StatusMission.Done, equip.ID);

        References.accountRefer.Coin += selected.CoinBonus;
        Player_AllUIManagement.Instance.SetUpCoinUI(References.accountRefer.Coin);
        References.AddExperience(selected.ExpBonus);

        Reload();

        ShowBonus(selected.CoinBonus, selected.ExpBonus, equip.Name, equip.Image);
    }

    public void Check()
    {
        DoingMission(HavingMission.EnemyID);
    }

}
[System.Serializable]
public struct ButtonTrophy
{
    public TrophyID ID;
    public Button Btn;
}