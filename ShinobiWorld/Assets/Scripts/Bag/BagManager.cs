using Assets.Scripts.Bag;
using Assets.Scripts.Bag.Equipment;
using Assets.Scripts.Bag.Item;
using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shop
{
    public class BagManager : MonoBehaviour
    {
        public GameObject Panel, prefabItemBag, prefabItemDetail, 
                            prefabEquipmentBag, prefabEquipmentDetail, MessageError;
        public Transform Content;

        public List<AccountItem_Entity> listItem;
        public List<AccountEquipment_Entity> listEquipment;

        public static BagManager Instance;

        public Intention Intention;
        private void Awake()
        {
            Instance = this;
        }

        public void OnBagBtnClick()
        {
            Panel.SetActive(true);
            OnItemBtnClick();
        }

        public void OnItemBtnClick()
        {
            References.listAccountItem = AccountItem_DAO.GetAllByUserID(References.accountRefer.ID);
            CloseMessage();
            CloseDetail();
            prefabItemDetail.SetActive(true);
            DestroyContent();
            GetListItem();
            if (listItem.Count <= 0) { ShowMessage(); }
            else ItemDetail.Instance.ShowDetail(listItem[0].ItemID);
        }

        public void OnEquipmentBtnClick()
        {
            References.listAccountEquipment = AccountEquipment_DAO.GetAllByUserID(References.accountRefer.ID);
            CloseMessage();
            CloseDetail();
            prefabEquipmentDetail.SetActive(true);
            DestroyContent();
            GetListEquipment();
            if (listEquipment.Count <= 0) { ShowMessage(); }
            else EquipmentDetail.Instance.ShowDetail(listEquipment[0].EquipmentID);
        }

        public void CloseDetail()
        {
            prefabEquipmentDetail.SetActive(false);
            prefabItemDetail.SetActive(false);
        }

        public void OnCloseBtnClick()
        {
            Panel.SetActive(false);
            CloseDetail();
            DestroyContent();
        }

        public void ShowMessage()
        {
            MessageError.SetActive(true);
            CloseDetail();
        }

        public void CloseMessage()
        {
            MessageError.SetActive(false);
        }

        public void DestroyContent()
        {
            foreach (Transform child in Content)
            {
                Destroy(child.gameObject);
            }
        }

        public void GetListItem()
        {
            //References.listAccountItem = AccountItem_DAO.GetAllByUserID(References.accountRefer.ID);

            listItem = References.listAccountItem.FindAll(obj => obj.Amount > 0);

            foreach (var accountItem in listItem)
            {
                var item = References.listItem.Find(obj => obj.ID == accountItem.ItemID);
                var itemManager = prefabItemBag.GetComponent<ItemBag>();
                itemManager.ID = item.ID;
                itemManager.Image.sprite = Resources.Load<Sprite>(item.Image);
                itemManager.Name.text = item.Name;
                itemManager.Own.text = accountItem.Amount.ToString();
                Instantiate(prefabItemBag, Content);
            }
        }

        public void GetListEquipment()
        {
            //References.listAccountEquipment = AccountEquipment_DAO.GetAllByUserID(References.accountRefer.ID);

            listEquipment = References.listAccountEquipment;

            if(Intention == Intention.Sell) listEquipment = References.listAccountEquipment.FindAll(obj => obj.IsUse == false);

            foreach (var accountEquipment in listEquipment)
            {
                var equipment = References.listEquipment.Find(obj => obj.ID == accountEquipment.EquipmentID);
                var equipmentManager = prefabEquipmentBag.GetComponent<EquipmentBag>();
                equipmentManager.ID = equipment.ID;
                equipmentManager.Image.sprite = Resources.Load<Sprite>(equipment.Image);
                equipmentManager.Name.text = equipment.Name;
                Instantiate(prefabEquipmentBag, Content);
            }
        }

        public void ReloadItem(string ID)
        {
            DestroyContent();
            GetListItem();
            if (listItem.Count <= 0) { ShowMessage(); }
            else
            {
                var accountItem = listItem.Find(obj => obj.ItemID == ID);

                if (accountItem != null) ItemDetail.Instance.ShowDetail(ID);
                else ItemDetail.Instance.ShowDetail(listItem[0].ItemID);
            }
        }

        public void ReloadEquipment(string ID)
        {
            DestroyContent();
            GetListEquipment();
            if (listEquipment.Count <= 0) { ShowMessage(); }
            else
            {
                var accountEquipment = listEquipment.Find(obj => obj.EquipmentID == ID);

                if (accountEquipment != null) EquipmentDetail.Instance.ShowDetail(ID);
                else EquipmentDetail.Instance.ShowDetail(listEquipment[0].EquipmentID);
            }
        }

        public void ResetColor()
        {
            foreach (Transform child in Content)
            {
                child.gameObject.GetComponent<Image>().color = new Color32(110, 80, 60, 255);
            }
        }
    }
}
