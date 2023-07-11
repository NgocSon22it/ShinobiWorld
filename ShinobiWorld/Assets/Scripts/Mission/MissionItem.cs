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
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Assets.Scripts.Mission
{
    public class MissionItem : MonoBehaviour
    {
        public TMP_Text Content, Target, requiedStrength, Trophi;
        public Button MissionBtn;
        public StatusMission status;
        Mission_Entity selected;

        public void OnClick()
        {
            MissionManager.Instance.ResetColor();
            GetComponent<Image>().color = new Color32(190, 140, 10, 255);
        }

        public void Setup(Mission_Entity mission)
        {
            selected = mission;
            Content.text = mission.Content;
            requiedStrength.text = mission.RequiredStrength.ToString();
            Target.text = mission.Target.ToString();
            Trophi.text = References.listTrophy.Find(obj => obj.ID == mission.TrophyID).Name;

            status = References.listHasMission.Find(obj => obj.MissionID == mission.ID).Status;

            MissionBtn.GetComponentInChildren<TMP_Text>().text = References.BtnMission[status.ToString()];
            MissionBtn.interactable = false;

            var strength = References.accountRefer.CurrentStrength;
            if ((strength >= mission.RequiredStrength //Enough strength
                    && MissionManager.Instance.HavingMission == null //and None mission
                    && status != StatusMission.Done) //Mission not done
                || status == StatusMission.Claim //Done mission
                || (MissionManager.Instance.HavingMission != null
                    &&  mission.ID == MissionManager.Instance.HavingMission.ID ))  // Have this mission
                        MissionBtn.interactable = true;
        }

        public void OnMissionBtnClick()
        {
            switch (status)
            {
                case StatusMission.None:
                    //Take mission
                    HasMission_DAO.ChangeStatusMission(References.accountRefer.ID, selected.ID, StatusMission.Doing);
                    MissionManager.Instance.TakeMission(selected);

                    break;
                case StatusMission.Doing:
                    //Cancel mission
                    HasMission_DAO.ChangeStatusMission(References.accountRefer.ID, selected.ID, StatusMission.None);
                    MissionManager.Instance.CancelMission();
                    
                    break;
                case StatusMission.Claim:
                    // Take bonus mission when finished mission
                    HasMission_DAO.ChangeStatusMission(References.accountRefer.ID, selected.ID, StatusMission.Done);
                    MissionManager.Instance.TakeBonusMission(selected);
                    break;
            }

        }
    }
}
