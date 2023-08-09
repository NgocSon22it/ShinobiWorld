using Assets.Scripts.Database.DAO;
using Assets.Scripts.Friend;
using Photon.Chat;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.BXH
{
    public class BXHItem : MonoBehaviour
    {
        public GameObject BXH;
        public Image RankImg;
        public TMP_Text RankTxt, Name, Level, Trophy, Power;
        public GameObject AddFriendBtn, InfoBtn;
        public Account_Entity selectedAccount;

        public void Setup(int index, Account_Entity account)
        {
            selectedAccount = account;

            RankTxt.text = index.ToString();
            --index;

            if (index >= 3)
            {
                index = 3;
            }
            else RankTxt.text = string.Empty;

            RankImg.sprite = Resources.Load<Sprite>(References.Rank.ElementAt(index).Key);
            BXH.GetComponent<Image>().color = References.Rank.ElementAt(index).Value;

            Name.text = account.Name;
            Level.text = account.Level.ToString();
            Trophy.text = References.listTrophy.Find(obj => obj.ID == account.TrophyID).Name;
            Power.text = account.Power.ToString();

            AddFriendBtn.GetComponent<Button>().onClick.AddListener(SendFriendRequest);
            InfoBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                Player_Info.Instance.Open(selectedAccount.ID);
            });

            References.listAllFriend = Friend_DAO.GetAll(References.accountRefer.ID);
            AddFriendBtn.SetActive(
                !References.listAllFriend
                    .Any(obj => (obj.MyAccountID + obj.FriendAccountID).Contains(References.accountRefer.ID) 
                                 && (obj.MyAccountID + obj.FriendAccountID).Contains(selectedAccount.ID)));
        }

        public void SendFriendRequest()
        {
            Friend_DAO.AddFriend(References.accountRefer.ID, selectedAccount.ID);

            AddFriendBtn.SetActive(false);

            if (Account_DAO.StateOnline(selectedAccount.ID))
            {
                ChatManager.Instance.chatClient
                    .SendPrivateMessage(selectedAccount.Name, 
                        string.Format(Message.PrivateMessage, TypePrivateMessage.FriendRequest.ToString(), ""));
            }
        }
    }
}
