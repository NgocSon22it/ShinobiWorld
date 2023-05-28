﻿using Assets.Scripts.Database.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace Assets.Scripts.Shop
{
    public class BuyItemManager : MonoBehaviour
    {
        public static BuyItemManager Instance;

        public GameObject ItemTemplate;

        public Transform Content;

        [Header("Detail")]
        public Image Image;
        public TMP_Text Name, Cost, Limit, Description, MessageError;
        public TMP_InputField Amount;
        public Button MinBtn, PlusBtn, BuyItemBtn;

        public int cost;
        public string ItemID;

        private void Awake()
        {
            Instance = this;
        }

        public void Open()
        {
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

        public void GetListItem()
        {
            foreach (var item in References.listItem)
            {
                var itemManager = ItemTemplate.GetComponent<BuyItem>();
                itemManager.ID = item.ID;
                itemManager.Image.sprite = Resources.Load<Sprite>(item.Image);
                itemManager.Cost.text = item.BuyCost.ToString();
                itemManager.Name.text = item.Name;
                Instantiate(ItemTemplate, Content);
            }
            if (References.DateUpdate.Day != DateTime.Now.Day)
            {
                References.DateUpdate = DateTime.Now;
                AccountItem_DAO.ResetLimitBuyItem(References.accountRefer.ID);
            }
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
            BuyItemBtn.interactable = true;
            MinBtn.interactable = true;
            PlusBtn.interactable = true;
            Amount.interactable = true;

            cost = item.BuyCost; //cost of a item 

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
                BuyItemBtn.interactable = false;
                MinBtn.interactable = false;
                PlusBtn.interactable = false;
                Amount.interactable = false;
            } else
            {
                UpdateCost();
            } 
        }

        public void CheckAmount()
        {
            int value = 0;
            if (!Amount.text.IsNullOrEmpty() && (!int.TryParse(Amount.text, out value) || (value <= 0)))
            {
                Amount.text = "1";
            }
            else
            {
                if (value > int.Parse(Limit.text)) Amount.text = Limit.text;
            }
        }

        public void UpdateCost()
        {
            if (!Amount.text.IsNullOrEmpty())
            {
                Cost.text = (cost * int.Parse(Amount.text)).ToString();
            }

            if (int.Parse(Cost.text) <= References.accountRefer.Coin)
            {
                BuyItemBtn.interactable = true;
                MessageError.text = "";
            }
            else
            {
                MessageError.text = Message.NotEnoughMoney.ToString();
                BuyItemBtn.interactable = false;
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

        public void Buy()
        {
            References.accountRefer.Coin -= int.Parse(Cost.text);
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

        public void Close()
        {
            DestroyItem();
            ShopManager.Instance.CloseBuyPanel();
        }
    }
}