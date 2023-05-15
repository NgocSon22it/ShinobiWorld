using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
    public ItemTemplate ItemTemplate;
    public Transform Content;
    // Start is called before the first frame update
    void Start()
    {
        for (var i =0; i < 18; ++i)
        {
            ItemTemplate.Name.text = i.ToString();
            //ItemTemplate.Image.sprite = "";
            Instantiate(ItemTemplate.Item, Content);
        }
    }
}

[System.Serializable]
public struct ItemTemplate
{
    public GameObject Item;
    public Image Image;
    public TMP_Text Name;
}
