using Assets.Scripts.Database.DAO;
using Assets.Scripts.Shop;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using WebSocketSharp;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
    public GameObject ItemTemplate, ShopPannel;
    public Transform Content;
    public TMP_Text Coin;

    public static ShopManager Instance;

    [Header("Detail")]
    public Image Image;
    public TMP_Text Name, Cost, Limit, Description, MessageError;
    public TMP_InputField Amount;
    public Button MinBtn, PlusBtn, BuyBtn, ShopBtn, CloseBtn;


    public int cost;
    public bool isUpdateCost = true;
    public string ItemID;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenShop()
    {
        ShopPannel.SetActive(true);
        GetListItem();
        ShowDetail(References.listItem[0].ID);
    }

    public void DestroyItem()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }
    }

    public void CloseShop()
    {
        DestroyItem();
        ShopPannel.SetActive(false);
    }

    public void GetListItem()
    {
        foreach (var item in References.listItem)
        {
            var itemManager = ItemTemplate.GetComponent<ItemShop>();
            itemManager.ID = item.ID;
            itemManager.Image.sprite = Resources.Load<Sprite>(item.Image);
            itemManager.Cost.text = item.BuyCost.ToString();
            itemManager.Name.text = item.Name;
            Instantiate(ItemTemplate, Content);
        }
        References.accountRefer = Account_DAO.GetAccountByID("piENbG5OaZZn4WN0jNHQWhP4ZaA3");
        Coin.text = References.accountRefer.Coin.ToString();
        References.listAccountItem = AccountItem_DAO.GetAllByUserID(References.accountRefer.ID);
    }

    public void ShowDetail(string ID)
    {
        ItemID = ID;
        var item = References.listItem.Find(obj => obj.ID == ID);
        Image.sprite = Resources.Load<Sprite>(item.Image);
        Name.text = item.Name;
        Cost.text = item.BuyCost.ToString();
        Limit.text = item.Limit.ToString();
        Amount.text = "1";
        Description.text = item.Description;

        //set active btn
        BuyBtn.interactable = true;
        MinBtn.interactable = true;
        PlusBtn.interactable = true;
        Amount.interactable = true;

        cost = item.BuyCost; //cost of a item 
        isUpdateCost = true; // update cost*amount

        //Get limit buy items in a day of the account
        var accountItem = References.listAccountItem.Find(obj => obj.ItemID == ID);
        if (accountItem != null)
        {
            Limit.text = accountItem.Limit.ToString();
        }

        //Show message overlimit buy item in a day of the account
        if (Limit.text == "0")
        {
            MessageError.text = Message.OverLimit;
            BuyBtn.interactable = false;
            MinBtn.interactable = false;
            PlusBtn.interactable = false;
            Amount.interactable = false;
        }
        else
        {
            //Check coin current of the account
            CheckBuy();
        }
       
    }

    public void CheckAmount()
    {
        int value = 0;
        if (!Amount.text.IsNullOrEmpty() && (!int.TryParse(Amount.text, out value) ||(value <= 0)))
        {
            Amount.text = "1";
        } else
        {
            if(value > int.Parse(Limit.text)) Amount.text = Limit.text;
        }
    }

    public void UpdateCost()
    {
        if (isUpdateCost && !Amount.text.IsNullOrEmpty())
        {
            Cost.text = (cost * int.Parse(Amount.text)).ToString();
        }   
    }

    public void Plus()
    {
        if (Amount.text.IsNullOrEmpty())
            Amount.text = "1";
        else
        {
            var value = int.Parse(Amount.text);
            Amount.text = (++value).ToString();
        }
    }

    public void Min()
    {
        if (Amount.text.IsNullOrEmpty())
            Amount.text = "1";
        else
        {
            var value = int.Parse(Amount.text);
            Amount.text = (--value).ToString();
        }
    }

    public void CheckBuy()
    {
        if (int.Parse(Cost.text) <= References.accountRefer.Coin)
        {
            BuyBtn.interactable = true;
            MessageError.text = "";
        }
        else
        {
            MessageError.text = Message.NotEnoughMoney.ToString();
            BuyBtn.interactable = false;
        }
                
    }

    public void Buy()
    {
        AccountItem_DAO.BuyItem(References.accountRefer.ID, ItemID,
                                int.Parse(Amount.text), int.Parse(Cost.text));
        ReLoad();
    }

    public void ReLoad()
    {
        DestroyItem();
        GetListItem();
        ShowDetail(ItemID);
    }

}
