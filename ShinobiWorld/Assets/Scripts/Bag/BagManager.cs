using Assets.Scripts.Bag.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Bag
{
    public class BagManager : MonoBehaviour 
    {
        public GameObject BagPannel;
        public Button ItemBtn, EquipmentBtn, BagBtn;
        public GameObject ItemDetail, EquipmentDetail;

        public void OnBagBtnClick()
        {
            BagPannel.SetActive(true);
            OnEquipmentBtnClick();
        }

        public void OnItemBtnClick()
        {
            AddItemDetail();
            BagItemManager.Instance.Open();
        }

        public void OnEquipmentBtnClick()
        {
            AddEquipmentDetail();
            BagEquipmentManager.Instance.Open();
        }

        public void AddItemDetail()
        {
            Destroy(EquipmentDetail.gameObject.transform);
            Instantiate(ItemDetail, BagPannel.transform);
        }

        public void AddEquipmentDetail()
        {
            Destroy(ItemDetail.gameObject.transform);
            Instantiate(EquipmentDetail, BagPannel.transform);
        }

        public void OnClose()
        {
            BagPannel.SetActive(false);
        }
    }
}
