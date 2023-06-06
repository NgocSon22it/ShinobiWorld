using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Hospital
{
    public class Hospital : MonoBehaviour
    {
        public Image uiFillImage;
        public TMP_Text uiText;
        public GameObject DiePanel;

        public int Duration;

        private int remainingDuration;

        public static Hospital Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void ResetTimer()
        {
            uiText.text = "00:00";
            uiFillImage.fillAmount = 0f;
        }


        public Hospital SetDuration(int seconds)
        {
            Duration = remainingDuration = seconds;
            return this;
        }

        public void Begin()
        {
            ResetTimer();
            DiePanel.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(UpdateTimer());
        }

        private IEnumerator UpdateTimer()
        {
            while (remainingDuration > 0)
            {
                UpdateUI(remainingDuration);
                remainingDuration--;
                yield return new WaitForSeconds(1f);
            }
            End();
        }

        private void UpdateUI(int seconds)
        {
            uiText.text = string.Format("{0:D2}:{1:D2}", seconds / 60, seconds % 60);
            uiFillImage.fillAmount = Mathf.InverseLerp(0, Duration, seconds);
        }

        public void End()
        {
            StopAllCoroutines();
            DiePanel.SetActive(false);
            References.accountRefer.CurrentHealth = References.accountRefer.Health;
            Game_Manager.Instance.SetupPlayer(References.HouseAddress[House.Hospital.ToString()]);
        }

        public void OnRespawnClick()
        {
            References.accountRefer.Coin -= References.RespawnCost;
            End();
        }
    }
}
