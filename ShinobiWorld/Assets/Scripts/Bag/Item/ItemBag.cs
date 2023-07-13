using Assets.Scripts.Bag.Item;
using Assets.Scripts.Shop;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBag : MonoBehaviour
{
    public string ID;
    public Image Image;
    public TMP_Text Name, Own;

    public void OnClick()
    {
        ItemDetail.Instance.ShowDetail(ID);
        BagManager.Instance.ResetColor();
        GetComponent<Image>().color = References.ItemColorSelected;
    }
}
