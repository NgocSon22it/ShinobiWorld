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
        [Header("Detail")]
        public Image Image;
        public TMP_Text Name, Level, Price, UpgradeCost, Description, MessageError, UseBtn_Text;
        public Button UpgradeBtn, DowngradeBtn;

        [Header("Upgrade")]
        public GameObject UpgradePanel;
        public GameObject LevelObj;
        public GameObject HealthObj;
        public GameObject ChakraObj;
        public GameObject DamageObj;
        public TMP_Text LevelCurrent, HealthCurrent, ChakraCurrent, DamageCurrent;
        public TMP_Text LevelUpgrade, HealthUpgrade, ChakraUpgrade, DamageUpgrade;
        public TMP_Text CostUpgrade;
        int HealthBonus, ChakraBonus, DamageBonus;

        [Header("Downgrade")]
        public GameObject DowngradePanel;
        public TMP_Text CostReturn;

        public static EquipmentDetail Instance;
        public BagEquipment_Entity BagEquipment;

        private void Awake()
        {
            Instance = this;
        }

        public void ShowDetail(int ID, string EquipmentID)
        {
            var equipment = References.listEquipment.Find(obj => obj.ID == EquipmentID);
            BagEquipment = References.listBagEquipment.Find(obj => obj.ID == ID &&
                                                                        obj.EquipmentID == EquipmentID);

            MessageError.text = "";
            Image.sprite = Resources.Load<Sprite>(equipment.Image);
            Name.text = equipment.Name;
            Level.text = BagEquipment.Level.ToString();
            if (!Price.IsUnityNull()) Price.text = equipment.SellCost.ToString();
            if (!UpgradeCost.IsUnityNull()) UpgradeCost.text = equipment.UpgradeCost.ToString();
            Description.text = equipment.Description;

            if (!UseBtn_Text.IsUnityNull()) UseBtn_Text.text = "Sử dụng";

            if (BagEquipment.IsUse)
            {
                if (!UseBtn_Text.IsUnityNull()) UseBtn_Text.text = "Gỡ";
                MessageError.text = Message.IsUsing.ToString();
            }

            if (!UpgradeBtn.IsUnityNull()) UpgradeBtn.interactable = true;
            if (!DowngradeBtn.IsUnityNull()) DowngradeBtn.interactable = true;

            if (BagEquipment.Level >= 30)
                if (!UpgradeBtn.IsUnityNull()) UpgradeBtn.interactable = false;

            if (BagEquipment.IsUse || BagEquipment.Level <= 1)
                if (!DowngradeBtn.IsUnityNull()) DowngradeBtn.interactable = false;
        }

        public void OnSellBtnClick()
        {
            ShopManager.Instance.typeSell = TypeSell.Equipment;
            ShopManager.Instance.ConfirmPanel.SetActive(true);
        }

        public void Sell()
        {
            BagEquipment_DAO.SellEquipment(BagEquipment.ID, References.accountRefer.ID, BagEquipment.EquipmentID, int.Parse(Price.text));

            References.AddCoin(int.Parse(Price.text));

            BagManager.Instance.ReloadEquipment(BagEquipment.ID, BagEquipment.EquipmentID, true);
        }

        public void OnUseBtnClick()
        {
            if (BagEquipment.IsUse) Remove();
            else Use();
        }

        public void Use()
        {
            References.UpdateAccountToDB();

            var type = References.listEquipment.Find(obj => obj.ID == BagEquipment.EquipmentID).TypeEquipmentID;
            var list = References.listEquipment.FindAll(obj => obj.TypeEquipmentID == type);

            var equip = References.listBagEquipment.Find(obj => list.Any(filter => filter.ID == obj.EquipmentID) && obj.IsUse == true);


            if (equip != null) BagEquipment_DAO.RemoveEquipment(equip.ID, References.accountRefer.ID, equip.EquipmentID);

            BagEquipment_DAO.UseEquipment(BagEquipment.ID, References.accountRefer.ID, BagEquipment.EquipmentID);

            Account_DAO.GetAccountPowerByID(References.accountRefer.ID);

            References.LoadAccount();
            Game_Manager.Instance.ReloadPlayerProperties();

            //References.listBagEquipment = BagEquipment_DAO.GetAllByUserID(References.accountRefer.ID);
            BagManager.Instance.ReloadEquipment(BagEquipment.ID, BagEquipment.EquipmentID);
        }

        public void Remove()
        {
            References.UpdateAccountToDB();

            BagEquipment_DAO.RemoveEquipment(BagEquipment.ID, References.accountRefer.ID, BagEquipment.EquipmentID);

            Account_DAO.GetAccountPowerByID(References.accountRefer.ID);

            References.LoadAccount();
            Game_Manager.Instance.ReloadPlayerProperties();

            //References.listBagEquipment = BagEquipment_DAO.GetAllByUserID(References.accountRefer.ID);
            BagManager.Instance.ReloadEquipment(BagEquipment.ID, BagEquipment.EquipmentID);
        }

        public void OnCloseBtnClick()
        {
            UpgradePanel.SetActive(false);
            DowngradePanel.SetActive(false);
        }

        public void OnUpgradeBtnClick()
        {
            UpgradePanel.SetActive(true);
            HealthObj.SetActive(false);
            ChakraObj.SetActive(false);
            DamageObj.SetActive(false);

            CostUpgrade.text = UpgradeCost.text;

            LevelCurrent.text = Level.text;
            LevelUpgrade.text = (BagEquipment.Level + 1).ToString();

            if (BagEquipment.Health != 0)
            {
                HealthBonus = Convert.ToInt32(BagEquipment.Health * (1 + References.Uppercent_Equipment / 100f));
                HealthObj.SetActive(true);
                HealthCurrent.text = BagEquipment.Health.ToString();
                HealthUpgrade.text = HealthBonus.ToString();
            }

            if (BagEquipment.Chakra != 0)
            {
                ChakraBonus = Convert.ToInt32(BagEquipment.Chakra * (1 + References.Uppercent_Equipment / 100f));
                ChakraObj.SetActive(true);
                ChakraCurrent.text = BagEquipment.Chakra.ToString();
                ChakraUpgrade.text = ChakraBonus.ToString();
            }

            if (BagEquipment.Damage != 0)
            {
                DamageBonus = Convert.ToInt32(BagEquipment.Damage * (1 + References.Uppercent_Equipment / 100f));
                DamageObj.SetActive(true);
                DamageCurrent.text = BagEquipment.Damage.ToString();
                DamageUpgrade.text = DamageBonus.ToString();
            }
        }

        public void Upgrade()
        {
            if (BagEquipment.IsUse)
            {
                BagEquipment_DAO.RemoveEquipment(BagEquipment.ID, References.accountRefer.ID, BagEquipment.EquipmentID);
                References.accountRefer.Health -= BagEquipment.Health;
                References.accountRefer.Chakra -= BagEquipment.Chakra;

                References.UpdateAccountToDB();

                BagEquipment_DAO.UpgradeEquipment(BagEquipment.ID, References.accountRefer.ID, BagEquipment.EquipmentID,
                                                    DamageBonus, HealthBonus, ChakraBonus);
                BagEquipment_DAO.UseEquipment(BagEquipment.ID, References.accountRefer.ID, BagEquipment.EquipmentID);
            }
            else BagEquipment_DAO.UpgradeEquipment(BagEquipment.ID, References.accountRefer.ID, BagEquipment.EquipmentID,
                                                    DamageBonus, HealthBonus, ChakraBonus);

            References.AddCoin(-int.Parse(CostUpgrade.text));

            //References.listBagEquipment = BagEquipment_DAO.GetAllByUserID(References.accountRefer.ID);

            BagManager.Instance.ReloadEquipment(BagEquipment.ID, BagEquipment.EquipmentID);
            OnUpgradeBtnClick();
            //UpgradePanel.SetActive(false);
        }

        public void OnDowngradeBtnClick()
        {
            DowngradePanel.SetActive(true);
            var costreturn = Convert.ToInt32((int.Parse(Level.text) - 1) * (int.Parse(UpgradeCost.text)) * 0.8f);
            CostReturn.text = costreturn.ToString();
        }

        public void Downgrade()
        {
            var equipment = References.listEquipment.Find(obj => obj.ID == BagEquipment.EquipmentID);

            BagEquipment_DAO
                .DowngradeEquipment(BagEquipment.ID, References.accountRefer.ID, BagEquipment.EquipmentID,
                                                    equipment.Damage, equipment.Health, equipment.Chakra);

            References.AddCoin(int.Parse(CostReturn.text));

            //References.listBagEquipment = BagEquipment_DAO.GetAllByUserID(References.accountRefer.ID);

            BagManager.Instance.ReloadEquipment(BagEquipment.ID, BagEquipment.EquipmentID);

            DowngradePanel.SetActive(false);
        }
    }
}
