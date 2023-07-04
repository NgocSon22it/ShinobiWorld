using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace Assets.Scripts.BXH
{
    public class BXHItem : MonoBehaviour
    {
        public GameObject BXH;
        public Image RankImg;
        public TMP_Text RankTxt, Name, Level, Trophy, Power;

        public void Setup(int index, Account_Entity account)
        {
            RankTxt.text = index.ToString();
            --index;

            if(index >= 3)
            {
                index = 3;
            }else RankTxt.text = string.Empty;

            RankImg.sprite = Resources.Load<Sprite>(References.Rank.ElementAt(index).Key);
            BXH.GetComponent<Image>().color = References.Rank.ElementAt(index).Value;

            Name.text = account.Name;
            Level.text = account.Level.ToString();
            Trophy.text = References.listTrophy.Find(obj => obj.ID == account.TrophiesID).Name;
            Power.text = account.Power.ToString();

        }
    }
}
