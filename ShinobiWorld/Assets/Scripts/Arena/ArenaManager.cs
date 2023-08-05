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
    MapType MapTypeSelected;
    Color currentColor;


    [Header("Practice Room")]
    [SerializeField] GameObject CreatePracticePanel;
    [SerializeField] TMP_Dropdown Boss_Dropdown;
    [SerializeField] List<GameObject> ListMap_Practice = new List<GameObject>();

    [Header("PK Room")]
    [SerializeField] GameObject CreatePKPanel;
    [SerializeField] TMP_Dropdown Bet_Dropdown;
    [SerializeField] TMP_Text Message;
    [SerializeField] List<GameObject> ListMap_PK = new List<GameObject>();

    [Header("Arena Room")]
    [SerializeField] GameObject CreateArenaPanel, CanPanel, CanNotPanel;
    [SerializeField] TMP_Text ArenaMessage, BossTxt, CurrentTrophy, NextTrophy, MapNameTxt;
    [SerializeField] Image MapImage;
    [SerializeField] List<Sprite> ListMapImage = new List<Sprite>();
    string boss, map;

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
        Practice_InitValue();      
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
        References.bossArenaType = BossArenaType.Practice;
        References.IsInvite = false;
        PhotonNetwork.IsMessageQueueRunning = false;
        PhotonNetwork.LoadLevel(References.MapInvite);
    }

    public void SelectedMap_Practice(int index)
    {
        foreach (GameObject map in ListMap_Practice)
        {
            currentColor = map.GetComponent<Image>().color;
            currentColor.a = 0;
            map.GetComponent<Image>().color = currentColor;
        }

        SelectedSquare(ListMap_Practice[index], SceneType.BossArena_);
    }

    public void Practice_InitValue()
    {
        SelectedSquare(ListMap_Practice[0], SceneType.BossArena_);
        if (Boss_Dropdown.options.Count > 0)
        {
            Practice_OnDropdownValueChanged(0);
            Boss_Dropdown.value = 0;
        }
    }

    public void Practice_OnDropdownValueChanged(int index)
    {
        string selectedOption = Boss_Dropdown.options[index].text;
        References.BossNameInvite = selectedOption;
    }
    #endregion

    #region PK
    public void Open_CreatePKPanel()
    {
        PK_InitValue();
        CreatePKPanel.SetActive(true);
    }

    public void Close_CreatePKPanel()
    {
        CreatePKPanel.SetActive(false);
    }

    public void SelectedMap_PK(int index)
    {
        foreach (GameObject map in ListMap_PK)
        {
            currentColor = map.GetComponent<Image>().color;
            currentColor.a = 0;
            map.GetComponent<Image>().color = currentColor;
        }

        SelectedSquare(ListMap_PK[index], SceneType.PK_);
    }
    public void PK_InitValue()
    {
        SelectedSquare(ListMap_PK[0], SceneType.PK_);

        if (Bet_Dropdown.options.Count > 0)
        {
            PK_OnDropdownValueChanged(0);
            Bet_Dropdown.value = 0;
        }
    }

    public void PK_OnDropdownValueChanged(int index)
    {
        string selectedOption = Bet_Dropdown.options[index].text;
        References.PKBet = Convert.ToInt32(selectedOption);

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
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
            References.IsInvite = false;
            PhotonNetwork.IsMessageQueueRunning = false;
            PhotonNetwork.LoadLevel(References.MapInvite);
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
                NextTrophy.text = "Hạ đẳng";
                MapImage.sprite = ListMapImage[0];
                MapNameTxt.text = "Rừng rậm";
                boss = BossName.Iruka.ToString();
                map = MapType.Forest.ToString();
                References.TrophyRegister = TrophyID.Trophy_Genin;
                BossTxt.text = boss;
                break;
            case "Trophy_Genin":
                CanPanel.SetActive(true);
                CurrentTrophy.text = "Hạ đẳng";
                NextTrophy.text = "Trung đẳng";
                MapImage.sprite = ListMapImage[1];
                MapNameTxt.text = "Bờ biển";
                boss = BossName.Asuma.ToString();
                map = MapType.Beach.ToString();
                References.TrophyRegister = TrophyID.Trophy_Chunin;
                BossTxt.text = boss;
                break;
            case "Trophy_Chunin":
                CanPanel.SetActive(true);
                CurrentTrophy.text = "Trung đẳng";
                NextTrophy.text = "Thượng đẳng";
                MapImage.sprite = ListMapImage[2];
                MapNameTxt.text = "Đồng bằng";
                boss = BossName.Kakashi.ToString();
                map = MapType.Delta.ToString();
                References.TrophyRegister = TrophyID.Trophy_Jonin;
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
            References.BossNameInvite = boss;
            References.MapInvite = SceneType.BossArena_.ToString() + map;
            ArenaMessage.text = "";
        }
        

    }

    public void Enter_ArenaRoom()
    {
        if (References.accountRefer.HasTicket)
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
            References.bossArenaType = BossArenaType.Official;
            References.IsInvite = false;
            PhotonNetwork.IsMessageQueueRunning = false;
            PhotonNetwork.LoadLevel(References.MapInvite);
        }

    }

    #endregion

    //Selected Box, Map
    public void SelectedSquare(GameObject currentMap, SceneType sceneType)
    {
        currentColor = currentMap.GetComponent<Image>().color;
        currentColor.a = 255;
        currentMap.GetComponent<Image>().color = currentColor;

        MapTypeSelected = currentMap.GetComponent<MapItem>().Type;
        References.MapInvite = sceneType.ToString() + MapTypeSelected;

    }


}
