using Assets.Scripts.Friend;
using Assets.Scripts.Database.DAO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FriendManager : MonoBehaviour
{
    public GameObject FriendPanel, FriendItemPrefab, RequestItemPrefab, FriendMessage, Notify;
    public Transform Content;

    public Button FriendBtn, RequestBtn, Openbtn, Closebtn;


    bool isFriend;
    public static FriendManager Instance;

    List<string> listFriend, listRequest;

    private void Awake()
    {
        Instance = this;

        Openbtn.onClick.AddListener(Open);

        Closebtn.onClick.AddListener(Close);

        FriendBtn.onClick.AddListener(OnFriendClick);

        RequestBtn.onClick.AddListener(OnRequestClick);

    }

    public void Start()
    {
        Init();
        Notify.SetActive((listRequest.Count > 0));
    }


    public void Open()
    {
        FriendMessage.SetActive(false);
        Game_Manager.Instance.IsBusy = true;

        Init();
        OnFriendClick();
        FriendPanel.SetActive(true);
    }

    public void Init()
    {
        var list = References.listAllFriend = Friend_DAO.GetAll(References.accountRefer.ID);
        listFriend = new List<string>();
        listRequest = new List<string>();

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

        //References.listFriendInfo = Friend_DAO.GetAllFriendInfo(listFriend);
        //References.listRequestInfo = Friend_DAO.GetAllFriendInfo(listRequest);

    }

    public void OnFriendClick()
    {
        isFriend = true;
        References.listFriendInfo = Friend_DAO.GetAllFriendInfo(listFriend);

        GetList(References.listFriendInfo);

        FriendBtn.GetComponent<Image>().color = References.ButtonColorSelected;

        RequestBtn.GetComponent<Image>().color = References.ButtonColorDefaul;
    }

    public void OnRequestClick()
    {
        isFriend = false;
        References.listRequestInfo = Friend_DAO.GetAllFriendInfo(listRequest);

        GetList(References.listRequestInfo);

        RequestBtn.GetComponent<Image>().color = References.ButtonColorSelected;

        FriendBtn.GetComponent<Image>().color = References.ButtonColorDefaul;
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
        Init();
        if (isFriend) FriendMessage.SetActive((References.listFriendInfo.Count <= 0));
        else
        {
            FriendMessage.SetActive((References.listRequestInfo.Count <= 0));
            Notify.SetActive((References.listRequestInfo.Count > 0));
        }
    }

    public void Destroy()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }
    }

    public void ResetColor()
    {
        foreach (Transform child in Content)
        {
            child.gameObject.GetComponent<Image>().color = References.ItemColorDefaul;
        }
    }

    public void Close()
    {
        Destroy();
        FriendPanel.SetActive(false);
        Game_Manager.Instance.IsBusy = false;
    }
}
