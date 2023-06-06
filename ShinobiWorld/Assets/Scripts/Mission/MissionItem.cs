using Assets.Scripts.Bag.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Mission
{
    public class MissionItem : MonoBehaviour
    {
        public int ID;
        public TMP_Text Content, Target, requiedStrength, Trophi;

        public void OnClick()
        {
            MissionManager.Instance.ResetColor();
            GetComponent<Image>().color = new Color32(190, 140, 10, 255);
        }
    }
}
