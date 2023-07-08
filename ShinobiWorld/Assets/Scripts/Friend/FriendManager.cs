using Assets.Scripts.Friend;
using Assets.Scripts.Database.DAO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FriendManager : MonoBehaviour
{
    public GameObject FriendPanel, FriendItemPrefab, RequestItemPrefab, FriendMessage;
    public Transform Content;

    public Button FriendBtn, RequestBtn;

    bool isFriend;
    public static FriendManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        FriendMessage.SetActive(false);
        Game_Manager.Instance.IsBusy = true;

        Init();
        OnFriendClick();
        FriendPanel.SetActive(true);
    }

    public void Destroy()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }
    }

    public void Init()
    {
        var list = Friend_DAO.GetAll(References.accountRefer.ID);
        var listFriend = new List<string>();
        var listRequest = new List<string>();

        var MyID = References.accountRefer.ID;
        foreach (var friend in list)
        {
            if (friend.IsFriend && (friend.MyAccountID == MyID || friend.FriendAccountID == MyID))
            {
                var FriendAccountID = (friend.FriendAccountID + friend.MyAccountID).Replace(MyID, "");
                if (!listFriend.Contains(FriendAccountID)) listFriend.Add(FriendAccountID);
            }
            else
            if ((!friend.IsFriend && friend.MyAccountID == MyID)
                && (!listRequest.Contains(friend.FriendAccountID)))
                listRequest.Add(friend.FriendAccountID);
        }

        References.listFriend = Friend_DAO.GetAllFriendInfo(listFriend);
        References.listRequest = Friend_DAO.GetAllFriendInfo(listRequest);

    }

    public void OnFriendClick()
    {
        isFriend = true;
        GetList(References.listFriend);

        var image = FriendBtn.GetComponent<Image>().color;
        FriendBtn.GetComponent<Image>().color = new Color(image.r, image.g, image.b, 1f);

        image = RequestBtn.GetComponent<Image>().color;
        RequestBtn.GetComponent<Image>().color = new Color(image.r, image.g, image.b, Mathf.InverseLerp(0, 255, 230));
    }

    public void OnRequestClick()
    {
        isFriend = false;
        GetList(References.listRequest);

        var image = RequestBtn.GetComponent<Image>().color;
        RequestBtn.GetComponent<Image>().color = new Color(image.r, image.g, image.b, 1f);

        image = FriendBtn.GetComponent<Image>().color;
        FriendBtn.GetComponent<Image>().color = new Color(image.r, image.g, image.b, Mathf.InverseLerp(0, 255, 230));
    }

    public void GetList(List<FriendInfo> list)
    {
        Destroy();

        if (list.Count <= 0) FriendMessage.SetActive(true);
        else
        {
            FriendMessage.SetActive(false);

            if (isFriend)
            {
                foreach (var friend in list)
                {
                    Instantiate(FriendItemPrefab, Content)
                        .GetComponent<FriendItem>().Setup(friend);
                }
            }
            else
            {
                foreach (var friend in list)
                {
                    Instantiate(RequestItemPrefab, Content)
                        .GetComponent<FriendItem>().Setup(friend);
                }
            }

        }

    }

    public void Reload()
    {
        if (isFriend) FriendMessage.SetActive((References.listFriend.Count <= 0));
        else FriendMessage.SetActive((References.listRequest.Count <= 0));
    }

    public void ResetColor()
    {
        foreach (Transform child in Content)
        {
            child.gameObject.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        }
    }

    public void Close()
    {
        Destroy();
        FriendPanel.SetActive(false);
        Game_Manager.Instance.IsBusy = false;
    }
}
