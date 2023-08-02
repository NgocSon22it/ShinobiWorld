using Assets.Scripts.Database.Entity;
using Assets.Scripts.School;
using Photon.Pun;
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

    public void Open_CreatePracticePanel()
    {
        InitDropdown();
        CreatePracticePanel.SetActive(true);
    }

    public void Close_CreatePracticePanel()
    {
        CreatePracticePanel.SetActive(false);
    }

    public void Enter_PracticeRoom()
    {
        if(PhotonNetwork.InRoom) 
        {
            PhotonNetwork.LeaveRoom();
        }
        PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.LoadLevel("BossArena_" + SelectedBoss);
    }

    public void Enter_PKRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.LoadLevel("PK");
    }

    public void InitDropdown()
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
            OnDropdownValueChanged(0);
            Boss_Dropdown.value = 0;
        }
    }

    public void OnDropdownValueChanged(int index)
    {
        if (index >= 0 && index < listBoss.Count)
        {
            SelectedBoss = listBoss[index];
        }
    }

    public void OpenPKRoom()
    {
        Close();
        Debug.Log("PK Room Opened");

    }


}
