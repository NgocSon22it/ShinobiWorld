using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [SerializeField] GameObject SettingPanel;

    public void ToggleSettingPanel(bool value)
    {
        Game_Manager.Instance.IsBusy = value;
        SettingPanel.SetActive(value);
    }

    public void OpenCustomSound()
    {
    }

    public void CloseCustomSound()
    {
    }

    public void OpenCustomKey()
    {
        Player_AllUIManagement.Instance.OpenCustomKeyPanel();
    }

    public void CloseCustomKey()
    {
        Player_AllUIManagement.Instance.CloseCustomKeyPanel();

    }

    public void BackToMenu()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.IsMessageQueueRunning = false;
            PhotonNetwork.LeaveRoom();
        }
        Game_Manager.Instance.IsBusy = false;
        PhotonNetwork.LoadLevel(Scenes.Login);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
