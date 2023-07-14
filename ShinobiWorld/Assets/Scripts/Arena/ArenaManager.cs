using Assets.Scripts.School;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaManager : MonoBehaviour
{
    public GameObject ConfirmPanel;
    public Button ArenaBtn, PKBtn, CloseBtn;

    public static ArenaManager Instance;

    private void Awake()
    {
        Instance = this;
        ArenaBtn.onClick.AddListener(OpenArenaRoom);
        PKBtn.onClick.AddListener(OpenPKRoom);

        CloseBtn.onClick.AddListener(Close);
    }

    public void Open()
    {
        Game_Manager.Instance.IsBusy = true;
        ConfirmPanel.SetActive(true);
    }

    public void OpenArenaRoom()
    {
        Close();
        Debug.Log("Arena Room Opened");
        ///
    }

    public void OpenPKRoom()
    {
        Close();
        Debug.Log("PK Room Opened");

    }

    public void Close()
    {
        Game_Manager.Instance.IsBusy = false;
        ConfirmPanel.SetActive(false);
    }
}
