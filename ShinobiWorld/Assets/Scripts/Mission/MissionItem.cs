using Assets.Scripts.Bag.Equipment;
using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public string ID;
        public TMP_Text Content, Target, requiedStrength, Trophi;
        public Button MissionBtn;
        public StatusMission status;

        public void OnClick()
        {
            MissionManager.Instance.ResetColor();
            GetComponent<Image>().color = new Color32(190, 140, 10, 255);
        }

        public void Setup(Mission_Entity mission)
        {
            ID = mission.ID;
            Content.text = mission.Content;
            requiedStrength.text = mission.RequiredStrength.ToString();
            Target.text = mission.Target.ToString();
            Trophi.text = References.listTrophy.Find(obj => obj.ID == mission.TrophiesID).Name;

            status = References.listAccountMission.Find(obj => obj.MissionID == ID).Status;

            MissionBtn.GetComponentInChildren<TMP_Text>().text = References.BtnMission[status.ToString()];
            MissionBtn.interactable = false;

            var strength = References.accountRefer.CurrentStrength;
            if (strength >= mission.RequiredStrength && string.IsNullOrEmpty(MissionManager.Instance.HavingMissionID))
                MissionBtn.interactable = true;

            if (mission.ID == MissionManager.Instance.HavingMissionID)
            {
                MissionBtn.interactable = true;
            }
        }

        public void OnMissionBtnClick()
        {
            var index = 0;
            switch (status)
            {
                case StatusMission.None:
                    //Take mission
                    References.accountRefer.CurrentStrength -= Convert.ToInt32(requiedStrength.text);
                    AccountMission_DAO.ChangeStatusMission(References.accountRefer.ID, ID, StatusMission.Doing);

                    MissionManager.Instance.HavingMissionID = ID;
                    index = References.listAccountMission.FindIndex(obj => obj.MissionID == ID);
                    References.listAccountMission[index].Status = StatusMission.Doing;

                    Player_AllUIManagement.Instance.LoadStrengthUI(References.accountRefer.Strength, References.accountRefer.CurrentStrength);
                    
                    break;
                case StatusMission.Doing:
                    //Cancel mission
                    AccountMission_DAO.ChangeStatusMission(References.accountRefer.ID, ID, StatusMission.None);
                    
                    MissionManager.Instance.HavingMissionID = string.Empty;
                    index = References.listAccountMission.FindIndex(obj => obj.MissionID == ID);
                    References.listAccountMission[index].Status = StatusMission.None;

                    break;
                case StatusMission.Claim:
                    // Take bonus mission when finished mission
                    break;
                case StatusMission.Done:
                    // Done mission
                    break;
            }

            MissionManager.Instance.Reload();
        }


    }
}
