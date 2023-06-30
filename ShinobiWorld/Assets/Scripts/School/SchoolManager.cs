using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.School
{
    public class SchoolManager : MonoBehaviour
    {
        public GameObject ConfirmPanel;

        public static SchoolManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void Open()
        {
            Game_Manager.Instance.IsBusy = true;
            ConfirmPanel.SetActive(true);
        }

        public void OpenSkillPanel() {
            Close();
            Skill_Manager.Instance.OpenSkillPanel();
        }

        public void OpenTrophyPanel()
        {
            Close();
            TrophyManager.Instance.Open();
        }

        public void Close()
        {
            Game_Manager.Instance.IsBusy = false;
            ConfirmPanel.SetActive(false);
        }
    }
}
