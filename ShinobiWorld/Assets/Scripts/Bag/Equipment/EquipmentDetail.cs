using Assets.Scripts.Bag.Item;
using Assets.Scripts.Database.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Bag.Equipment
{
    public class EquipmentDetail : MonoBehaviour
    {
        public Image Image;
        public TMP_Text Name, Level, Price, UpgradeCost, Description, MessageError;
        public Button UseBtn, SellBtn, UpgradeBtn, DowngradeBtn;

        public static EquipmentDetail Instance;
        public string EquipmentID;

        private void Awake()
        {
            Instance = this;
        }

        public void ShowDetail(string ID)
        {
            EquipmentID = ID;
            var equipment = References.listEquipment.Find(obj => obj.ID == ID);
            var accountEquipment = References.listAccountEquipment.Find(obj => obj.EquipmentID == ID);

            Image.sprite = Resources.Load<Sprite>(equipment.Image);
            Name.text = equipment.Name;
            Level.text = accountEquipment.Level.ToString();
            Price.text = equipment.SellCost.ToString();
            UpgradeCost.text = equipment.UpgradeCost.ToString();
            Description.text = equipment.Description;
        }

        public void Sell()
        {
            AccountEquipment_DAO.SellEquipment(References.accountRefer.ID, EquipmentID, int.Parse(Price.text));

            BagEquipmentManager.Instance.Reload(EquipmentID);
        }
    }
}
