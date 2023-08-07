using Assets.Scripts.Database.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Friend
{
    public class FriendItem : MonoBehaviour
    {
        public TMP_Text Name, Trophy;
        public GameObject Online;
        public GameObject MySelf;
        public Button ChatBnt, InfoBtn, UnFriendBtn, AcceptBtn;
        FriendInfo selectedfriend;

        private void Awake()
        {
            GetComponent<Image>().color = References.ItemColorDefaul;

            if(!UnFriendBtn.IsUnityNull())UnFriendBtn.onClick.AddListener(() => DeleteFriend());
            if(!AcceptBtn.IsUnityNull()) AcceptBtn.onClick.AddListener(() => Accept());
            if(!InfoBtn.IsUnityNull()) InfoBtn.onClick.AddListener(ViewFriendInfo);
        }

        public void OnClick()
        {
            FriendManager.Instance.ResetColor();
            GetComponent<Image>().color = References.ItemColorSelected;
        }

        public void Setup(FriendInfo friend)
        {
            selectedfriend = friend;
            Name.text = friend.Name;
            Trophy.text = References.listTrophy.Find(obj => obj.ID == friend.TrophyID).Name;
            Online.SetActive(friend.IsOnline);
        }

        public void Accept()
        {
            Friend_DAO.AcceptFriend(References.accountRefer.ID, selectedfriend.ID);

            Destroy(MySelf);

            References.listRequestInfo.Remove(selectedfriend);
            References.listFriendInfo.Add(selectedfriend);

            FriendManager.Instance.Reload();
        }

        public void DeleteFriend()
        {
            Friend_DAO.DeleteFriend(References.accountRefer.ID, selectedfriend.ID);

            Destroy(MySelf);
            References.listRequestInfo.Remove(selectedfriend);
            References.listFriendInfo.Remove(selectedfriend);

            FriendManager.Instance.Reload();
        }

       
        public void ViewFriendInfo()
        {
            Player_Info.Instance.Open(selectedfriend.ID);
        }
    }
}
