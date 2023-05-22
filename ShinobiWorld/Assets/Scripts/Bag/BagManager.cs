using Assets.Scripts.Bag.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Bag
{
    public class BagManager : MonoBehaviour 
    {
        public GameObject BagPannel;
        public Button ItemBtn, EquipmentBtn, BagBtn;
        public GameObject ItemDetail, EquipmentDetail;
        public GameObject MessageError;
        GameObject instantiatedItemDetail, instantiatedEquipmentDetail;

        public static BagManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void OnBagBtnClick()
        {
            BagPannel.SetActive(true);
            OnItemBtnClick();
        }

        public void OnItemBtnClick()
        {
            CloseMessage();
            DestroyDetail();
            instantiatedItemDetail = Instantiate(ItemDetail, BagPannel.transform);
            BagItemManager.Instance.Open();
        }

        public void OnEquipmentBtnClick()
        {
            CloseMessage();
            DestroyDetail();
            instantiatedEquipmentDetail = Instantiate(EquipmentDetail, BagPannel.transform);
            BagEquipmentManager.Instance.Open();
        }

        public void DestroyDetail()
        {
            Destroy(instantiatedItemDetail);
            Destroy(instantiatedEquipmentDetail);
        }

        public void OnClose()
        {
            BagPannel.SetActive(false);
        }

        public void ShowMessage()
        {
            MessageError.SetActive(true);
            Destroy(instantiatedItemDetail);
            Destroy(instantiatedEquipmentDetail);
        }

        public void CloseMessage()
        {
            MessageError.SetActive(false);
        }
    }
}
