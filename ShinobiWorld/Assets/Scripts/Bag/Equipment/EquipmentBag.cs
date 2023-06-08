using Assets.Scripts.Bag.Equipment;
using Assets.Scripts.Bag.Item;
using Assets.Scripts.Shop;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentBag : MonoBehaviour
{
    public string ID;
    public Image Image;
    public TMP_Text Name;

    public void OnClick()
    {
        EquipmentDetail.Instance.ShowDetail(ID);
        BagManager.Instance.ResetColor();
        GetComponent<Image>().color = new Color32(190, 140, 10, 255);
    }
}
