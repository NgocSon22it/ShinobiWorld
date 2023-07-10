using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TrophyManager : MonoBehaviourPunCallbacks
{
    public GameObject TrophyPanel;
    public TMP_Text Name, Level, TrophyName, Cost, MessageTxt;
    public Button RegisterUpgradeBtn, GetNewTrophyBtn;

    Trophy_Entity NextTrophy;

    public static TrophyManager Instance;

    //public TrophyID NextTrophy;

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        Game_Manager.Instance.IsBusy = true;

        //References.ReLoadUIAccount();

        TrophyPanel.SetActive(true);
        MessageTxt.text = string.Empty;
        Cost.color = Color.black;
        Level.color = Color.black;

        var currentTrophy = (int)Enum.Parse(typeof(TrophyID), References.accountRefer.TrophyID);
        var nextTrophy = ((TrophyID)((currentTrophy + 1 >= 4) ? currentTrophy : ++currentTrophy)).ToString();
        NextTrophy = References.listTrophy.Find(obj => obj.ID.Equals(nextTrophy));

        Name.text = PhotonNetwork.NickName;
        Level.text = References.accountRefer.Level.ToString();
        TrophyName.text = References.listTrophy.Find(obj => obj.ID.Equals(References.accountRefer.TrophyID)).Name;
        Cost.text = NextTrophy.Cost.ToString();

        GetNewTrophyBtn.interactable = References.accountRefer.IsUpgradeTrophy;

        var check = true;

        if (References.accountRefer.Coin < NextTrophy.Cost)
        {
            check = false;
            Cost.text += string.Format(" ({0})", Message.NotEnoughMoney);
            Cost.color = Color.red;
            MessageTxt.text = Message.TrophyUpgradeError.ToString();
        }

        if (References.accountRefer.Level < NextTrophy.ContraitLevelAccount)
        {
            check = false;
            Level.text += string.Format(" ({0})", string.Format(Message.NotEnoughLevel, NextTrophy.ContraitLevelAccount));
            Level.color = Color.red;
            MessageTxt.text = Message.TrophyUpgradeError.ToString();
        }

        RegisterUpgradeBtn.interactable = check;

    }

    public void RegisterUpgradeTrophy()
    {
        References.AddCoin((-1)* NextTrophy.Cost);
        Account_DAO.UpgradeTrophyRegister(References.accountRefer.ID, true);
        References.LoadAccount();
        Close();
    }

    public void GetNewTrophy()
    {
        References.UpdateAccountToDB();
        Account_DAO.UpgradeTrophy(References.accountRefer.ID, NextTrophy.ID);
        References.LoadAccount();
        Close();
    }

    public void Close()
    {
        Game_Manager.Instance.IsBusy = false;
        TrophyPanel.SetActive(false);
    }

}
