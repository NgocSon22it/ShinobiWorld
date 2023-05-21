using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Bag.Item
{
    public class ItemDetail : MonoBehaviour
    {
        public Image Image;
        public TMP_Text Name, Own, Price, Description, MessageError;
        public TMP_InputField Amount;
        public Button MinBtn, PlusBtn, UseBtn, SellBtn;

        public int cost;
        public bool isUpdateCost = true;

        public static ItemDetail Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void ShowDetail(string ID)
        {
            var item = References.listItem.Find(obj => obj.ID == ID);
            var accountItem = References.listAccountItem.Find(obj => obj.ItemID == ID);

            Image.sprite = Resources.Load<Sprite>(item.Image);
            Name.text = item.Name;
            Own.text = accountItem.Amount.ToString();
            Price.text = ((int)item.BuyCost * 0.8).ToString();
            Amount.text = "1";
            Description.text = item.Description;
        }
    }
}
