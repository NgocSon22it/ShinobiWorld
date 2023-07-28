using Assets.Scripts.Bag.Equipment;
using Assets.Scripts.Bag.Item;
using Assets.Scripts.Database.Entity;
using Assets.Scripts.Shop;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemBag : MonoBehaviour
{
    public string ID;
    public Image Image;
    public TMP_Text Name, Own;
    ItemDetail ItemDetail;
    public void OnClick()
    {
        ItemDetail.ShowDetail(ID);
        ItemDetail.BagManagerInstance.ResetColor();
        GetComponent<Image>().color = References.ItemColorSelected;
    }

    public void Setup(Item_Entity item, int amount , ItemDetail detail, bool isFisrt)
    {
        if(isFisrt) GetComponent<Image>().color = References.ItemColorSelected;
        ItemDetail = detail;

        ID = item.ID;
        Image.sprite = Resources.Load<Sprite>(item.Image);
        Name.text = item.Name;
        Own.text = amount.ToString();
    }
}
