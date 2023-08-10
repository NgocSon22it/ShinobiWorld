using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Portal_Manager : MonoBehaviour
{
    [SerializeField] GameObject PortalPanel;

    public TMP_Dropdown Teleport_DropDown;
    public string SelectedArea;
    public static Portal_Manager Instance;
    public List<string>  ListArea = new List<string>();

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        Game_Manager.Instance.IsBusy = true;
        InitDropdown();
        PortalPanel.SetActive(true);
    }

    public void Close()
    {
        PortalPanel.SetActive(false);
        Game_Manager.Instance.IsBusy = false;
    }

    public void InitDropdown()
    {
        ListArea.Clear();
        
        foreach (string area in References.ListAllArea.Keys)
        {
            if (!References.ListAllArea[area].Equals(Game_Manager.Instance.currentAreaName))
            {
                ListArea.Add(area);
            }
        }

        Teleport_DropDown.ClearOptions();
        Teleport_DropDown.AddOptions(ListArea);

        if (Teleport_DropDown.options.Count > 0)
        {
            OnDropdownValueChanged(0);
            Teleport_DropDown.value = 0;
        }
    }
    public void OnDropdownValueChanged(int index)
    {
        string selectedAreaName = Teleport_DropDown.options[index].text;
        CurrentAreaName selectedAreaValue = References.ListAllArea[selectedAreaName];

        SelectedArea = selectedAreaValue.ToString();
    }

    public void TeleportToSelectArea()
    {
        References.InitSaveValue();
        Game_Manager.Instance.IsBusy = false;    
        References.PlayerSpawnPosition = References.AreaAddress[SelectedArea.ToString()];
        PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(SelectedArea);
    }
}
