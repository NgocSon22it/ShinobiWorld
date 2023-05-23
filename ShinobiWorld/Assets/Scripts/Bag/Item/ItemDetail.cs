using Assets.Scripts.Database.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using static UnityEditor.Progress;

namespace Assets.Scripts.Bag.Item
{
    public class ItemDetail : MonoBehaviour
    {
        public Image Image;
        public TMP_Text Name, Own, Price, Description;
        public TMP_InputField Amount;
        public Button MinBtn, PlusBtn, UseBtn, SellBtn;

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
            price = Convert.ToInt32(item.BuyCost * 0.8);
            Price.text = price.ToString();
            Amount.text = "1";
            Description.text = item.Description;

            isUpdatePrice = true;

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

            BagItemManager.Instance.Reload(ItemID);
        }

        public void Use()
        {
            AccountItem_DAO.UseItem(References.accountRefer.ID, ItemID);

            BagItemManager.Instance.Reload(ItemID);
        }
    }
}
