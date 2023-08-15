using Assets.Scripts.Bag;
using Assets.Scripts.Bag.Equipment;
using Assets.Scripts.Bag.Item;
using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Assets.Scripts.MailBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shop
{
    public class BagManager : MonoBehaviour
    {
        public GameObject Panel, prefabItemBag, prefabItemDetail, 
                            prefabEquipmentBag, prefabEquipmentDetail, MessageError;
        public Transform Content;
        public Button ItemBtn, EquipmentBtn;

        public List<HasItem_Entity> listHasItem;
        public List<BagEquipment_Entity> listBagEquipment;

        public static BagManager Instance;

        public EquipmentDetail EquipmentDetailInstance;
        public ItemDetail ItemDetailInstance;

        public Intention Intention;

        public GameObject reloadItem;

        private void Awake()
        {
            Instance = this;
            EquipmentDetailInstance = prefabEquipmentDetail.GetComponent<EquipmentDetail>();
            ItemDetailInstance = prefabItemDetail.GetComponent<ItemDetail>();
        }

        public void OnBagBtnClick()
        {
            Game_Manager.Instance.IsBusy = true;
            Panel.SetActive(true);
            OnItemBtnClick();
        }

        public void  ResetColorBtn()
        {
            EquipmentBtn.GetComponent<Image>().color = References.ButtonColorDefaul;
            ItemBtn.GetComponent<Image>().color = References.ButtonColorDefaul;
        }

        public void OnItemBtnClick()
        {
            if(!ItemBtn.IsUnityNull())ItemBtn.GetComponent<Image>().color = References.ButtonColorSelected;
            if(!EquipmentBtn.IsUnityNull()) EquipmentBtn.GetComponent<Image>().color = References.ButtonColorDefaul;

            Game_Manager.Instance.IsBusy = true;
            //References.listHasItem = HasItem_DAO.GetAllByUserID(References.accountRefer.ID);
            CloseMessage();
            CloseDetail();
            prefabItemDetail.SetActive(true);
            DestroyContent();
            GetListItem();
            if (listHasItem.Count <= 0) { ShowMessage(); }
            else
            {
                ItemDetailInstance.ShowDetail(listHasItem[0].ItemID);
                
            }
        }

        public void OnEquipmentBtnClick()
        {
            EquipmentBtn.GetComponent<Image>().color = References.ButtonColorSelected;
            if (!ItemBtn.IsUnityNull()) ItemBtn.GetComponent<Image>().color = References.ButtonColorDefaul;

            Game_Manager.Instance.IsBusy = true;
            References.listBagEquipment = BagEquipment_DAO.GetAllByUserID(References.accountRefer.ID);
            CloseMessage();
            CloseDetail();
            prefabEquipmentDetail.SetActive(true);
            DestroyContent();
            GetListEquipment();
            if (listBagEquipment.Count <= 0) { ShowMessage(); }
            else
            {
                EquipmentDetailInstance.ShowDetail(listBagEquipment[0].ID, listBagEquipment[0].EquipmentID);
                
            }
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
            Game_Manager.Instance.IsBusy = false;
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

        public void GetListItem(bool isReload = false, string ID = null)
        {
            References.listHasItem = HasItem_DAO.GetAllByUserID(References.accountRefer.ID);

            listHasItem = References.listHasItem.FindAll(obj => obj.Amount > 0);

            var isFirst = true;
            foreach (var HasItem in listHasItem)
            {
                var item = References.listItem.Find(obj => obj.ID == HasItem.ItemID);
                reloadItem = Instantiate(prefabItemBag, Content);
                reloadItem.GetComponent<ItemBag>().Setup(item, HasItem.Amount, ItemDetailInstance, isFirst);
                if (isReload && ID == item.ID) reloadItem.GetComponent<ItemBag>().OnClick();
                isFirst = false;
            }
        }

        public void GetListEquipment()
        {
            listBagEquipment = References.listBagEquipment = BagEquipment_DAO.GetAllByUserID(References.accountRefer.ID);

            if (Intention == Intention.Sell) listBagEquipment = References.listBagEquipment.FindAll(obj => obj.IsUse == false);
            var isFirst = true;
            foreach (var BagEquipment in listBagEquipment)
            {
                Instantiate(prefabEquipmentBag, Content)
                   .GetComponent<EquipmentBag>()
                   .Setup(BagEquipment, EquipmentDetailInstance, isFirst);
                isFirst = false;
            }
        }

        public void ReloadItem(string ID)
        {
            DestroyContent();
            GetListItem(true, ID);
            if (listHasItem.Count <= 0) { ShowMessage(); }
            else
            {
                var HasItem = listHasItem.Find(obj => obj.ItemID == ID);

                if (HasItem != null) ItemDetailInstance.ShowDetail(ID);
                else ItemDetailInstance.ShowDetail(listHasItem[0].ItemID);
            }
        }

        public void ReloadEquipment(int ID, string EquipmentID, bool isReloadContent = false)
        {
            if (isReloadContent)
            {
                DestroyContent();
                GetListEquipment();
            }
            else listBagEquipment = References.listBagEquipment = BagEquipment_DAO.GetAllByUserID(References.accountRefer.ID);

            if (listBagEquipment.Count <= 0) { ShowMessage(); }
            else
            {
                var BagEquipment = listBagEquipment.Find(obj => obj.ID == ID &&
                                                                            obj.EquipmentID == EquipmentID);

                if (BagEquipment != null) EquipmentDetailInstance.ShowDetail(ID, EquipmentID);
                else EquipmentDetailInstance.ShowDetail(listBagEquipment[0].ID, listBagEquipment[0].EquipmentID);
            }
        }

        public void ResetColor()
        {
            foreach (Transform child in Content)
            {
                child.gameObject.GetComponent<Image>().color = References.ItemColorDefaul;
            }
        }
    }
}
