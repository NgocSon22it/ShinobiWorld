using Assets.Scripts.Database.Entity;
using Assets.Scripts.School;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaManager : MonoBehaviour
{
    public GameObject ConfirmPanel;


    [Header("Practice Room")]
    [SerializeField] GameObject CreatePracticePanel;
    [SerializeField] TMP_Dropdown Boss_Dropdown;
    public List<string> listBoss = new List<string>();
    string SelectedBoss;

    [Header("PK Room")]
    [SerializeField] GameObject CreatePKPanel;
    [SerializeField] TMP_Dropdown Bet_Dropdown;
    [SerializeField] TMP_Text Message;

    [Header("Arena Room")]
    [SerializeField] GameObject CreateArenaPanel, CanPanel, CanNotPanel;
    [SerializeField] TMP_Text ArenaMessage, BossTxt, CurrentTrophy;
    string boss;

    public static ArenaManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        Game_Manager.Instance.IsBusy = true;
        ConfirmPanel.SetActive(true);
    }
    public void Close()
    {
        Game_Manager.Instance.IsBusy = false;
        ConfirmPanel.SetActive(false);
    }

    #region Practice
    public void Open_CreatePracticePanel()
    {
        Practice_InitDropdown();
        CreatePracticePanel.SetActive(true);
    }

    public void Close_CreatePracticePanel()
    {
        CreatePracticePanel.SetActive(false);
    }

    public void Enter_PracticeRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.LoadLevel("BossArena_" + SelectedBoss);
    }
    public void Practice_InitDropdown()
    {
        listBoss.Clear();
        foreach (Trophy_Entity area in References.listTrophy)
        {
            if (!area.BossID.Equals("None"))
            {
                string boss = area.BossID.Replace("Boss_", "");
                listBoss.Add(boss);
            }
        }

        Boss_Dropdown.ClearOptions();
        Boss_Dropdown.AddOptions(listBoss);

        if (Boss_Dropdown.options.Count > 0)
        {
            Practice_OnDropdownValueChanged(0);
            Boss_Dropdown.value = 0;
        }
    }

    public void Practice_OnDropdownValueChanged(int index)
    {
        if (index >= 0 && index < listBoss.Count)
        {
            SelectedBoss = listBoss[index];
        }
    }
    #endregion

    #region PK
    public void Open_CreatePKPanel()
    {
        PK_InitDropdown();
        CreatePKPanel.SetActive(true);
    }

    public void Close_CreatePKPanel()
    {
        CreatePKPanel.SetActive(false);
    }

    public void PK_InitDropdown()
    {
        if (Bet_Dropdown.options.Count > 0)
        {
            PK_OnDropdownValueChanged(0);
            Bet_Dropdown.value = 0;
        }
    }

    public void PK_OnDropdownValueChanged(int index)
    {
        string selectedOptionText = Bet_Dropdown.options[index].text;
        References.PKBet = Convert.ToInt32(selectedOptionText);

        if (References.accountRefer.Coin < References.PKBet)
        {
            Message.text = "Bạn không đủ vàng!";
        }
        else
        {
            Message.text = "";
        }
    }

    public void Enter_PKRoom()
    {
        if (References.accountRefer.Coin >= References.PKBet)
        {
            References.AddCoin(-References.PKBet);

            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
            PhotonNetwork.IsMessageQueueRunning = false;
            PhotonNetwork.LoadLevel(Scenes.PK);
        }

    }

    #endregion

    #region Arena
    public void Open_CreateArenaPanel()
    {
        Arena_InitBossName();
        CreateArenaPanel.SetActive(true);
    }

    public void Close_CreateArenaPanel()
    {
        CreateArenaPanel.SetActive(false);
    }

    public void Arena_InitBossName()
    {
        switch (References.accountRefer.TrophyID)
        {
            case "Trophy_None":
                CanPanel.SetActive(true);
                CurrentTrophy.text = "Tập sự";
                boss = "Iruka";
                BossTxt.text = boss;
                break;
            case "Trophy_Genin":
                CanPanel.SetActive(true);
                CurrentTrophy.text = "Hạ đẳng";
                boss = "Asuma";
                BossTxt.text = boss;
                break;
            case "Trophy_Chunin":
                CanPanel.SetActive(true);
                CurrentTrophy.text = "Trung đẳng";
                boss = "Kakashi";
                BossTxt.text = boss;
                break;
            case "Trophy_Jonin":
                CanNotPanel.SetActive(true);
                break;
        }
        if (!References.accountRefer.HasTicket)
        {
            ArenaMessage.text = "Bạn chưa đăng kí nâng cấp danh hiệu\r\n(trường học)";
        }
        else
        {
            ArenaMessage.text = "";
        }

    }

    public void Enter_ArenaRoom()
    {
        if (References.accountRefer.HasTicket)
        {
            References.accountRefer.HasTicket = false;
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
            PhotonNetwork.IsMessageQueueRunning = false;
            PhotonNetwork.LoadLevel("BossArena_" + boss);
        }

    }

    #endregion

}
