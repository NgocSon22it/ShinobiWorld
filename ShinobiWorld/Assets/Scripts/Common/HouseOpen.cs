using Assets.Scripts.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Mission
{
    public class HouseOpen : MonoBehaviour
    {
        public House house;
        private bool isOpen = false;

        public string HouseName;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O) && isOpen)
            {
                switch (house)
                {
                    case House.Hokage:
                        MissionManager.Instance.Open();
                        break;
                    case House.Shop:
                        ShopManager.Instance.OnShopBtnClick();
                        break;
                    case House.School:
                        SchoolManager.Instance.Open();
                        break;
                    case House.Arena:
                        ArenaManager.Instance.Open();
                        break;
                    case House.Portal:
                        Portal_Manager.Instance.Open();
                        break;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.GetComponent<PlayerBase>().PlayerAllUIInstance != null)
                {
                    collision.GetComponent<PlayerBase>().PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().ShowHouseMessage(HouseName);
                    isOpen = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.GetComponent<PlayerBase>().PlayerAllUIInstance != null)
                {
                    collision.GetComponent<PlayerBase>().PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().CloseHouseMessage();
                    isOpen = false;
                }
            }
        }

    }
}
