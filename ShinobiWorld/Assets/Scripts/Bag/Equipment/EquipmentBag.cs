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
    public int AccountEquipmentID;
    public string ID;
    public Image Image;
    public TMP_Text Name;

    public void OnClick()
    {
        EquipmentDetail.Instance.ShowDetail(AccountEquipmentID, ID);
        BagManager.Instance.ResetColor();
        GetComponent<Image>().color = new Color32(190, 140, 10, 255);
    }

    public void Setup(AccountEquipment_Entity accountEquipment)
    {
        var equipment = References.listEquipment.Find(obj => obj.ID == accountEquipment.EquipmentID);
        AccountEquipmentID = accountEquipment.ID;
        ID = equipment.ID;
        Image.sprite = Resources.Load<Sprite>(equipment.Image);
        Name.text = equipment.Name;
    }
}
