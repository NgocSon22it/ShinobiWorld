
using Assets.Scripts.Bag;
using Assets.Scripts.Bag.Equipment;
using Assets.Scripts.Bag.Item;
using Assets.Scripts.Database.DAO;
using Assets.Scripts.Shop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;
using WebSocketSharp;

public class ShopManager : MonoBehaviour
{
    public GameObject BuyPanel, SellPanel, ShopPanel, ConfirmPanel;

    public static ShopManager Instance;
    public TypeSell typeSell;

    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        CloseShopPanel();
        BuyPanel.SetActive(false);
        CloseConfirmPanel();
        SellPanel.SetActive(false);
    }

    public void OnShopBtnClick()
    {
        Init();
        Game_Manager.Instance.IsBusy = true;
        ShopPanel.SetActive(true);
    }

    public void OnBuyBtnClick()
    {
        Init();
        BuyPanel.SetActive(true);
        BuyItemManager.Instance.Open();
    }

    public void OnSellBtnClick()
    {
        Init();
        SellPanel.SetActive(true);
        BagManager.Instance.OnItemBtnClick();  
    }

    public void CloseConfirmPanel() { ConfirmPanel.SetActive(false); }

    public void CloseShopPanel() 
    { 
        ShopPanel.SetActive(false); 
        Game_Manager.Instance.IsBusy = false;
    }

    public void OnConfirmSellBtnClick()
    {
        switch (typeSell)
        {
            case TypeSell.Item:
                ItemDetail.Instance.Sell();
                CloseConfirmPanel();
                break;
            case TypeSell.Equipment:
                EquipmentDetail.Instance.Sell();
                CloseConfirmPanel();
                break;
        }
    }
}
