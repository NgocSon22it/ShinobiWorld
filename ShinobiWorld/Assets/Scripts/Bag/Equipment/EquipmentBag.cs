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

    public void OnClick()
    {
        EquipmentDetail.Instance.ShowDetail(BagEquipmentID, ID);
        BagManager.Instance.ResetColor();
        GetComponent<Image>().color = new Color32(190, 140, 10, 255);
    }

    public void Setup(BagEquipment_Entity BagEquipment)
    {
        var equipment = References.listEquipment.Find(obj => obj.ID == BagEquipment.EquipmentID);
        BagEquipmentID = BagEquipment.ID;
        ID = equipment.ID;
        Image.sprite = Resources.Load<Sprite>(equipment.Image);
        Name.text = equipment.Name;
    }
}
