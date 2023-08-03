using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Portal_Manager : MonoBehaviour
{
    [SerializeField] GameObject PortalPanel;

    public TMP_Dropdown Teleport_DropDown;
    public string SelectedArea;
    public List<string> listArea = new List<string>();
    public static Portal_Manager Instance;

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
        listArea.Clear();
        foreach (string area in References.AllScenes)
        {
            if (!area.Equals(Game_Manager.Instance.currentAreaName.ToString()))
            {
                listArea.Add(area);
            }
        }

        Teleport_DropDown.ClearOptions();
        Teleport_DropDown.AddOptions(listArea);

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
        References.PlayerSpawnPosition = References.AreaAddress[SelectedArea.ToString()];
        PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(SelectedArea);
    }
}
