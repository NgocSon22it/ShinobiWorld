using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Assets.Scripts.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
        public GameObject BagManagerObj;

        public Image Image;
        public TMP_Text Name, Own, Price, Description;
        public TMP_InputField Amount;
        public Button MinBtn, PlusBtn;

        public int price;
        public bool isUpdatePrice = true;

        public Item_Entity item;

        public static ItemDetail Instance;
        public BagManager BagManagerInstance;
        private void Awake()
        {
            Instance = this;
            BagManagerInstance = BagManagerObj.GetComponent<BagManager>();
        }

        public void ShowDetail(string ID)
        {

            item = References.listItem.Find(obj => obj.ID == ID);
            var HasItem = References.listHasItem.Find(obj => obj.ItemID == ID);

            Image.sprite = Resources.Load<Sprite>(item.Image);
            Name.text = item.Name;
            Own.text = HasItem.Amount.ToString();
            if (!Price.IsUnityNull())
            {
                price = Convert.ToInt32(item.BuyCost * 0.8);
                Price.text = price.ToString();
                isUpdatePrice = true;
            }
            if (!Amount.IsUnityNull()) Amount.text = "1";
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

        public void OnSellBtnClick()
        {
            ShopManager.Instance.typeSell = TypeSell.Item;
            ShopManager.Instance.ConfirmPanel.SetActive(true);
        }

        public void Sell()
        {
            HasItem_DAO.SellItem(References.accountRefer.ID, item.ID,
                                    int.Parse(Amount.text), int.Parse(Price.text));

            References.AddCoin(int.Parse(Price.text));

            BagManagerInstance.ReloadItem(item.ID);
        }

        public void Use()
        {
            References.UpdateAccountToDB();

            HasItem_DAO.UseItem(References.accountRefer.ID, item.ID);

            var health = References.accountRefer.CurrentHealth + item.HealthBonus;
            var chakra = References.accountRefer.CurrentChakra + item.ChakraBonus;
            var strength = References.accountRefer.CurrentStrength + item.StrengthBonus;

            References.accountRefer.CurrentHealth
                = (health >= References.accountRefer.Health) ? References.accountRefer.Health : health;

            References.accountRefer.CurrentChakra
                = (chakra >= References.accountRefer.Chakra) ? References.accountRefer.Chakra : chakra;

            References.accountRefer.CurrentStrength
                = (strength >= References.accountRefer.Strength) ? References.accountRefer.Strength : strength;

            Game_Manager.Instance.ReloadPlayerProperties();

            BagManagerInstance.ReloadItem(item.ID);
        }
    }
}
