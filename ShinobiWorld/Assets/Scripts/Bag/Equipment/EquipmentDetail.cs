using Assets.Scripts.Bag.Item;
using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
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

namespace Assets.Scripts.Bag.Equipment
{
    public class EquipmentDetail : MonoBehaviour
    {
        public Image Image;
        public TMP_Text Name, Level, Price, UpgradeCost, Description, MessageError, UseBtn_Text;

        public static EquipmentDetail Instance;
        public AccountEquipment_Entity accountEquipment;

        private void Awake()
        {
            Instance = this;
        }

        public void ShowDetail(string ID)
        {
            var equipment = References.listEquipment.Find(obj => obj.ID == ID);
            accountEquipment = References.listAccountEquipment.Find(obj => obj.EquipmentID == ID);

            MessageError.text = "";
            Image.sprite = Resources.Load<Sprite>(equipment.Image);
            Name.text = equipment.Name;
            Level.text = accountEquipment.Level.ToString();
            if (!Price.IsUnityNull()) Price.text = equipment.SellCost.ToString();
            if (!UpgradeCost.IsUnityNull()) UpgradeCost.text = equipment.UpgradeCost.ToString();
            Description.text = equipment.Description;

            if (!UseBtn_Text.IsUnityNull()) UseBtn_Text.text = "Sử dụng";
      
            if (accountEquipment.IsUse)
            {
                if (!UseBtn_Text.IsUnityNull()) UseBtn_Text.text = "Gỡ";
                MessageError.text = Message.IsUsing.ToString();
            }
        }

        public void OnSellBtnClick()
        {
            ShopManager.Instance.typeSell = TypeSell.Equipment;
            ShopManager.Instance.ConfirmPanel.SetActive(true);
        }

        public void Sell()
        {
            AccountEquipment_DAO.SellEquipment(References.accountRefer.ID, accountEquipment.EquipmentID, int.Parse(Price.text));
            BagManager.Instance.ReloadEquipment(accountEquipment.EquipmentID);
        }

        public void OnUseBtnClick()
        {
            if (accountEquipment.IsUse) Remove();
            else Use();
        }

        public void Use()
        {
            var type = References.listEquipment.Find(obj => obj.ID == accountEquipment.EquipmentID).TypeEquipmentID;
            var list = References.listEquipment.FindAll(obj => obj.TypeEquipmentID == type);

            var equip = References.listAccountEquipment.Find(obj => list.Any(filter => filter.ID == obj.EquipmentID) && obj.IsUse == true);

            if(equip !=  null)
            {
                AccountEquipment_DAO.RemoveEquipment(References.accountRefer.ID, equip.EquipmentID);
            }
            AccountEquipment_DAO.UseEquipment(References.accountRefer.ID, accountEquipment.EquipmentID);
            BagManager.Instance.ReloadEquipment(accountEquipment.EquipmentID);
        }

        public void Remove()
        {
            AccountEquipment_DAO.RemoveEquipment(References.accountRefer.ID, accountEquipment.EquipmentID);
            BagManager.Instance.ReloadEquipment(accountEquipment.EquipmentID);
        }
    }
}
