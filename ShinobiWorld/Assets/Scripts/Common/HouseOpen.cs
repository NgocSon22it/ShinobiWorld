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
        public GameObject Panel;
        public House house;
        private bool isOpen = false;

        public string HouseName;

        public void OpenHousePanel()
        {
            Panel.SetActive(true);
        }

        public void CloseHousePanel()
        {
            Panel.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O) && isOpen)
            {
                OpenHousePanel();

                switch (house)
                {
                    case House.Hokage:
                        GetComponent<MissionManager>().Open();
                        break;
                    case House.Shop:
                        Panel.GetComponent<ShopManager>().OnShopBtnClick();
                        break;
                    case House.School:
                        GetComponent<SchoolManager>().Open();
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
