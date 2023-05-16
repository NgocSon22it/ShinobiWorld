using Assets.Scripts.Shop;
using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
    public GameObject ItemTemplate;
    public Transform Content;

    public static ShopManager Instance;

    [Header("Detail")]
    public Image Image;
    public TMP_Text Name, Cost, Limit, Description, Message;
    public TMP_InputField Amount;
    public Button MinBtn, PlusBtn, BuyBtn;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in References.listItem)
        {
            var itemManager = ItemTemplate.GetComponent<ItemManager>();
            itemManager.ID = item.ID;
            itemManager.Image.sprite = Resources.Load<Sprite>(item.Image);
            itemManager.Cost.text = item.BuyCost.ToString();
            itemManager.Name.text = item.Name;
            Instantiate(ItemTemplate, Content);
        }
    }

    public void ShowDetail(string ID)
    {
        var item = References.listItem.Find(obj => obj.ID == ID);
        Image.sprite = Resources.Load<Sprite>(item.Image);
        Name.text = item.Name;
        Cost.text = item.BuyCost.ToString();
        Limit.text = item.Limit.ToString();
        Description.text = item.Description;

    }
}
