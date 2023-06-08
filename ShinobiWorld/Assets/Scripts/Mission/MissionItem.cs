using Assets.Scripts.Bag.Equipment;
using Assets.Scripts.Database.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Assets.Scripts.Mission
{
    public class MissionItem : MonoBehaviour
    {
        public int ID;
        public TMP_Text Content, Target, requiedStrength, Trophi;
        public Button TakeBtn;
        public bool status;

        public void OnClick()
        {
            MissionManager.Instance.ResetColor();
            GetComponent<Image>().color = new Color32(190, 140, 10, 255);
        }

        public void OnTakeBtnClick()
        {
            if (status)
            {
                AccountMission_DAO.ChangeStatusMission(References.accountRefer.ID, ID, false);
                MissionManager.Instance.HavingMissionID = 0;
            }
            else
            {
                References.accountRefer.CurrentStrength -= Convert.ToInt32(requiedStrength.text);
                AccountMission_DAO.ChangeStatusMission(References.accountRefer.ID, ID, true);
                MissionManager.Instance.HavingMissionID = ID;
                Player_AllUIManagement.Instance.LoadStrengthUI(References.accountRefer.Strength, References.accountRefer.CurrentStrength);
            }
            MissionManager.Instance.Reload();
        }
    }
}
