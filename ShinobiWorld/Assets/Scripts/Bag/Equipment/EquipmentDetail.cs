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

            if (!UpgradeBtn.IsUnityNull()) UpgradeBtn.interactable = true;
            if (!DowngradeBtn.IsUnityNull()) DowngradeBtn.interactable = true;

            if (accountEquipment.Level  >= 30 ) 
                if (!UpgradeBtn.IsUnityNull()) UpgradeBtn.interactable = false;

            if(accountEquipment.IsUse || accountEquipment.Level <= 1) 
                if (!DowngradeBtn.IsUnityNull()) DowngradeBtn.interactable = false;
        }

        public void OnSellBtnClick()
        {
            ShopManager.Instance.typeSell = TypeSell.Equipment;
            ShopManager.Instance.ConfirmPanel.SetActive(true);
        }

        public void Sell()
        {
            AccountEquipment_DAO.SellEquipment(References.accountRefer.ID, accountEquipment.EquipmentID, int.Parse(Price.text));
            
            References.accountRefer.Coin += int.Parse(Price.text);
            Player_AllUIManagement.Instance.SetUpCoinUI(References.accountRefer.Coin);

            var index = References.listAccountEquipment.FindIndex(obj => obj.EquipmentID == accountEquipment.EquipmentID);
            References.listAccountEquipment[index].Delete = true;

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
                LoadUI(equip.Health * (-1), equip.Chakra * (-1));
            }

            AccountEquipment_DAO.UseEquipment(References.accountRefer.ID, accountEquipment.EquipmentID);

            var index = References.listAccountEquipment.FindIndex(obj => obj.EquipmentID == accountEquipment.EquipmentID);
            References.listAccountEquipment[index].IsUse = true;

            LoadUI(accountEquipment.Health, accountEquipment.Chakra);

            BagManager.Instance.ReloadEquipment(accountEquipment.EquipmentID);
        }

        public void Remove()
        {
            AccountEquipment_DAO.RemoveEquipment(References.accountRefer.ID, accountEquipment.EquipmentID);

            var index = References.listAccountEquipment.FindIndex(obj => obj.EquipmentID == accountEquipment.EquipmentID);
            References.listAccountEquipment[index].IsUse = false;

            LoadUI(accountEquipment.Health*(-1), accountEquipment.Chakra * (-1));

            BagManager.Instance.ReloadEquipment(accountEquipment.EquipmentID);
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
            LevelUpgrade.text = (accountEquipment.Level + 1).ToString();

            if(accountEquipment.Health != 0)
            {
                HealthBonus = Convert.ToInt32(accountEquipment.Health * (1 + References.Uppercent_Equipment / 100f));
                HealthObj.SetActive(true);
                HealthCurrent.text = accountEquipment.Health.ToString();
                HealthUpgrade.text = HealthBonus.ToString();
            }

            if (accountEquipment.Chakra != 0)
            {
                ChakraBonus = Convert.ToInt32(accountEquipment.Chakra * (1 + References.Uppercent_Equipment / 100f));
                ChakraObj.SetActive(true);
                ChakraCurrent.text = accountEquipment.Chakra.ToString();
                ChakraUpgrade.text = ChakraBonus.ToString();
            }

            if (accountEquipment.Damage != 0)
            {
                DamageBonus = Convert.ToInt32(accountEquipment.Damage * (1 + References.Uppercent_Equipment / 100f));
                DamageObj.SetActive(true);
                DamageCurrent.text = accountEquipment.Damage.ToString();
                DamageUpgrade.text = DamageBonus.ToString();
            }
        }

        public void Upgrade()
        {
            if(accountEquipment.IsUse)
            {
                AccountEquipment_DAO.RemoveEquipment(References.accountRefer.ID, accountEquipment.EquipmentID);
                References.accountRefer.Health -= accountEquipment.Health;
                References.accountRefer.Charka -= accountEquipment.Chakra;

                AccountEquipment_DAO.UpgradeEquipment(References.accountRefer.ID, accountEquipment.EquipmentID,
                                                    DamageBonus, HealthBonus, ChakraBonus);
                AccountEquipment_DAO.UseEquipment(References.accountRefer.ID, accountEquipment.EquipmentID);
            } 
            else AccountEquipment_DAO.UpgradeEquipment(References.accountRefer.ID, accountEquipment.EquipmentID,
                                                    DamageBonus, HealthBonus, ChakraBonus);

            References.accountRefer.Coin -= int.Parse(CostUpgrade.text);
            Player_AllUIManagement.Instance.SetUpCoinUI(References.accountRefer.Coin);

            var index = References.listAccountEquipment.FindIndex(obj => obj.EquipmentID == accountEquipment.EquipmentID);
            References.listAccountEquipment[index].Health = HealthBonus;
            References.listAccountEquipment[index].Damage = DamageBonus;
            References.listAccountEquipment[index].Chakra = ChakraBonus;

            BagManager.Instance.ReloadEquipment(accountEquipment.EquipmentID);
            LoadUI(accountEquipment.Health, accountEquipment.Chakra);

            UpgradePanel.SetActive(false);
        }

        public void OnDowngradeBtnClick()
        {
            DowngradePanel.SetActive(true);
            var costreturn = Convert.ToInt32((int.Parse(Level.text) - 1) * (int.Parse(UpgradeCost.text)) * 0.8f);
            CostReturn.text = costreturn.ToString();
        }

        public void Downgrade()
        {
            var equipment = References.listEquipment.Find(obj => obj.ID == accountEquipment.EquipmentID);

            AccountEquipment_DAO.DowngradeEquipment(References.accountRefer.ID, accountEquipment.EquipmentID,
                                                    equipment.Damage, equipment.Health, equipment.Chakra);

            References.accountRefer.Coin += int.Parse(CostReturn.text);
            Player_AllUIManagement.Instance.SetUpCoinUI(References.accountRefer.Coin);

            var index = References.listAccountEquipment.FindIndex(obj => obj.EquipmentID == accountEquipment.EquipmentID);
            References.listAccountEquipment[index].Health = equipment.Health;
            References.listAccountEquipment[index].Damage = equipment.Damage;
            References.listAccountEquipment[index].Chakra = equipment.Chakra;

            BagManager.Instance.ReloadEquipment(accountEquipment.EquipmentID);

            DowngradePanel.SetActive(false);
        }

        public void LoadUI(int Health, int Charka)
        {
            References.accountRefer.Health += Health;
            References.accountRefer.Charka += Charka;
            Game_Manager.Instance.ReloadPlayerProperties();

            Player_AllUIManagement.Instance
                    .LoadHealthUI(References.accountRefer.Health, References.accountRefer.CurrentHealth);
            Player_AllUIManagement.Instance
                    .LoadChakraUI(References.accountRefer.Charka, References.accountRefer.CurrentCharka);
        }
    }
}
