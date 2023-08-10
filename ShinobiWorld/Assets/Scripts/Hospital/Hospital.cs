using Assets.Scripts.Database.DAO;
using Photon.Pun;
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
        public Button ReSpawnBtn;

        public int Duration;

        private int remainingDuration;

        //public static Hospital Instance;

        private void Awake()
        {
            //Instance = this;
            ReSpawnBtn.onClick.AddListener(OnRespawnClick);
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
            ReSpawnBtn.GetComponentInChildren<TMP_Text>().text = $"Hồi sinh {References.RespawnCost}";
            StopAllCoroutines();
            StartCoroutine(UpdateTimer());
        }

        private IEnumerator UpdateTimer()
        {
            while (remainingDuration > 0)
            {
                UpdateUI(remainingDuration);
                remainingDuration--;
                References.accountRefer.TimeRespawn = remainingDuration;
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
            References.accountRefer.IsDead = false;
            References.accountRefer.TimeRespawn = 0;
            References.accountRefer.CurrentHealth = References.accountRefer.Health;
            References.accountRefer.CurrentChakra = References.accountRefer.Chakra;

            if (Game_Manager.Instance.currentAreaName == CurrentAreaName.Konoha)
            {
                Game_Manager.Instance.GoingOutHospital();
            }
            else
            {
                if (PhotonNetwork.InRoom)
                {
                    References.PlayerSpawnPosition = new Vector3(17, -27, 0);
                    ChatManager.Instance.DisconnectFromChat();
                    Game_Manager.Instance.IsBusy = false;
                    PhotonNetwork.IsMessageQueueRunning = false;
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel(Scenes.Konoha);
                }
            }
        }

        public void OnRespawnClick()
        {
            if (References.accountRefer.Coin >= References.RespawnCost)
            {
                References.accountRefer.Coin -= References.RespawnCost;
                End();
            }
        }
    }
}
