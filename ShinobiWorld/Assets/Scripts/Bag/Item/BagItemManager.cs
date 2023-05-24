using Assets.Scripts.Bag;
using Assets.Scripts.Bag.Item;
using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
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

    public static BagItemManager Instance;

    public List<AccountItem_Entity> list;

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        DestroyItem();
        GetListItem();
        if (list.Count <= 0) { BagManager.Instance.ShowMessage(); }
        else ItemDetail.Instance.ShowDetail(list[0].ItemID);
    }

    public void Reload(string ID)
    {
        DestroyItem();
        GetListItem();
        if (list.Count <= 0) { BagManager.Instance.ShowMessage(); }
        else
        {
            var accountItem = list.Find(obj => obj.ItemID == ID);

            if (accountItem != null) ItemDetail.Instance.ShowDetail(ID);
            else ItemDetail.Instance.ShowDetail(list[0].ItemID);
        }
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
        References.listAccountItem = AccountItem_DAO.GetAllByUserID(References.accountRefer.ID);

        list = References.listAccountItem.FindAll(obj => obj.Amount > 0);

        foreach (var accountItem in list)
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

    
