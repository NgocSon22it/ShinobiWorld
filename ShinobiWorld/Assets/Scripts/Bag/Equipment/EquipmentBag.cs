using Assets.Scripts.Bag.Equipment;
using Assets.Scripts.Bag.Item;
using Assets.Scripts.Database.Entity;
using Assets.Scripts.Shop;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentBag : MonoBehaviour
{
    public int BagEquipmentID;
    public string ID;
    public Image Image;
    public TMP_Text Name;
    EquipmentDetail equipmentDetail;

    public void OnClick()
    {
        equipmentDetail.ShowDetail(BagEquipmentID, ID);
        equipmentDetail.BagManagerInstance.ResetColor();
        GetComponent<Image>().color = References.ItemColorSelected;
    }

    public void Setup(BagEquipment_Entity BagEquipment, EquipmentDetail detail, bool isFirst)
    {
        GetComponent<Image>().color = References.ItemColorDefaul;

        if (isFirst) GetComponent<Image>().color = References.ItemColorSelected;
        equipmentDetail = detail;
        var equipment = References.listEquipment.Find(obj => obj.ID == BagEquipment.EquipmentID);
        BagEquipmentID = BagEquipment.ID;
        ID = equipment.ID;
        Image.sprite = Resources.Load<Sprite>(equipment.Image);
        Name.text = equipment.Name;
    }
}
