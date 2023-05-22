using Assets.Scripts.Bag.Item;
using Assets.Scripts.Database.DAO;
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
            EquipmentDetail.Instance.ShowDetail(References.listAccountEquipment[0].EquipmentID);
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
            References.accountRefer = Account_DAO.GetAccountByID("piENbG5OaZZn4WN0jNHQWhP4ZaA3");
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
