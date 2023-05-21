using Assets.Scripts.Bag.Equipment;
using Assets.Scripts.Bag.Item;
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
    }
}
