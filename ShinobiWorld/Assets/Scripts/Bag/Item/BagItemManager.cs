using Assets.Scripts.Bag.Item;
using Assets.Scripts.Database.DAO;
using Assets.Scripts.Shop;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagItemManager : MonoBehaviour
{
    public GameObject ItemTemplate;
    public Transform Content;
    public TMP_Text Coin;

    public static BagItemManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        DestroyItem();
        GetListItem();
        ItemDetail.Instance.ShowDetail(References.listAccountItem[0].ItemID);
    }

    public void DestroyItem()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }
    }

    public void GetListItem()
    {
        References.accountRefer = Account_DAO.GetAccountByID("piENbG5OaZZn4WN0jNHQWhP4ZaA3");
        Coin.text = References.accountRefer.Coin.ToString();
        References.listAccountItem = AccountItem_DAO.GetAllByUserID(References.accountRefer.ID);

        foreach (var accountItem in References.listAccountItem)
        {
            var item = References.listItem.Find(obj => obj.ID == accountItem.ItemID);
            var itemManager = ItemTemplate.GetComponent<ItemBag>();
            itemManager.ID = item.ID;
            itemManager.Image.sprite = Resources.Load<Sprite>(item.Image);
            itemManager.Name.text = item.Name;
            itemManager.Own.text = accountItem.Amount.ToString();
            Instantiate(ItemTemplate, Content);
        }
    }
}

    
