using Assets.Scripts.Database.DAO;
using Assets.Scripts.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace Assets.Scripts.Bag.Item
{
    public class ItemDetail : MonoBehaviour
    {
        public Image Image;
        public TMP_Text Name, Own, Price, Description;
        public TMP_InputField Amount;
        public Button MinBtn, PlusBtn;

        public int price;
        public bool isUpdatePrice = true;
        public string ItemID;

        public static ItemDetail Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void ShowDetail(string ID)
        {
            ItemID = ID;
            var item = References.listItem.Find(obj => obj.ID == ID);
            var accountItem = References.listAccountItem.Find(obj => obj.ItemID == ID);

            Image.sprite = Resources.Load<Sprite>(item.Image);
            Name.text = item.Name;
            Own.text = accountItem.Amount.ToString();
            if (!Price.IsUnityNull())
            {
                price = Convert.ToInt32(item.BuyCost * 0.8);
                Price.text = price.ToString();
                isUpdatePrice = true;
            }
            if (!Amount.IsUnityNull())  Amount.text = "1";
            Description.text = item.Description;
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
                if (value > int.Parse(Own.text)) Amount.text = Own.text;
            }
        }

        public void UpdatePrice()
        {
            if (isUpdatePrice && !Amount.text.IsNullOrEmpty())
            {
                Price.text = (price * int.Parse(Amount.text)).ToString();
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

        public void Sell()
        {
            AccountItem_DAO.SellItem(References.accountRefer.ID, ItemID,
                                    int.Parse(Amount.text), int.Parse(Price.text));

            BagManager.Instance.ReloadItem(ItemID);
        }

        public void Use()
        {
            AccountItem_DAO.UseItem(References.accountRefer.ID, ItemID);

            BagManager.Instance.ReloadItem(ItemID);
        }
    }
}
