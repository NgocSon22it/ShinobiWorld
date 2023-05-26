using Assets.Scripts.Bag;
using Assets.Scripts.Database.DAO;
using Assets.Scripts.Shop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Windows;
using WebSocketSharp;

public class ShopManager : MonoBehaviour
{
    public GameObject Shop, BuyPanel, SellPanel, PopupPanel;
    public Button BuyBtn, SellBtn;

    GameObject instantiatedBuyPanel, instantiatedSellPanel;

    public static ShopManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OnShopBtnClick()
    {
        PopupPanel.SetActive(true);
    }

    public void OnBuyBtnClick()
    {
        PopupPanel.SetActive(false);
        instantiatedBuyPanel = Instantiate(BuyPanel, Shop.transform);
        BuyItemManager.Instance.Open();
    }

    public void OnSellBtnClick()
    {
        PopupPanel.SetActive(false);
        instantiatedSellPanel = Instantiate(SellPanel, Shop.transform);
        BagManager.Instance.OnItemBtnClick();  
    }

    public void Close()
    {
        PopupPanel.SetActive(false);
    }

    public void CloseBuyPanel()
    {
        Destroy(instantiatedBuyPanel);
    }

    public void CloseSellPanel()
    {
        Destroy(instantiatedSellPanel);
    }
}
