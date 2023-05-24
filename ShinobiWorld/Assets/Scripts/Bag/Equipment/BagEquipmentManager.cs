using Assets.Scripts.Bag.Item;
using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Bag.Equipment
{
    public class BagEquipmentManager : MonoBehaviour
    {
        public GameObject EquipmentTemplate;
        public Transform Content;

        public static BagEquipmentManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void Open()
        {
            DestroyEquipment();
            GetListEquidment();
            if (References.listAccountEquipment.Count <= 0) { BagManager.Instance.ShowMessage(); }
            else
            {
                EquipmentDetail.Instance.ShowDetail(References.listAccountEquipment[0].EquipmentID);
            }
        }

        public void Reload(string ID)
        {
            DestroyEquipment();
            GetListEquidment();
            if (References.listAccountEquipment.Count <= 0) { BagManager.Instance.ShowMessage(); }
            else
            {
                var accountEquipment = References.listAccountEquipment.Find(obj => obj.EquipmentID == ID);

                if (accountEquipment != null) EquipmentDetail.Instance.ShowDetail(ID);
                else EquipmentDetail.Instance.ShowDetail(References.listAccountEquipment[0].EquipmentID);
            }
        }

        public void DestroyEquipment()
        {
            foreach (Transform child in Content)
            {
                Destroy(child.gameObject);
            }
        }

        public void GetListEquidment()
        {
            References.listAccountEquipment = AccountEquipment_DAO.GetAllByUserID(References.accountRefer.ID);
            
            foreach (var accountEquipment in References.listAccountEquipment)
            {
                var equipment = References.listEquipment.Find(obj => obj.ID == accountEquipment.EquipmentID);
                var equipmentManager = EquipmentTemplate.GetComponent<EquipmentBag>();
                equipmentManager.ID = equipment.ID;
                equipmentManager.Image.sprite = Resources.Load<Sprite>(equipment.Image);
                equipmentManager.Name.text = equipment.Name;
                Instantiate(EquipmentTemplate, Content);
            }
        }
    }
}
