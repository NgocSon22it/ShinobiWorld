using Assets.Scripts.Database.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Friend
{
    public class FriendItem : MonoBehaviour
    {
        public TMP_Text Name, Trophy;
        public GameObject Online;
        public GameObject MySelf;
        FriendInfo selectedfriend;

        public void OnClick()
        {
            FriendManager.Instance.ResetColor();
            GetComponent<Image>().color = References.ColorSelected;
        }

        public void Setup(FriendInfo friend)
        {
            selectedfriend = friend;
            Name.text = friend.Name;
            Trophy.text = References.listTrophy.Find(obj => obj.ID == friend.TrophyID).Name;
            Online.SetActive(friend.IsOnline);

            GetComponent<Image>().color = new Color32(0, 0, 0, 0);
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
    }
}
