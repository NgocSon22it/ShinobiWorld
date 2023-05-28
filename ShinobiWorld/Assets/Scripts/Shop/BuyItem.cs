using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shop
{
    public class BuyItem : MonoBehaviour
    {
        public string ID;
        public Image Image;
        public TMP_Text Name, Cost;

        public void OnClick()
        {
            BuyItemManager.Instance.ShowDetail(ID);
        }
    }
}
