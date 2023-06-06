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
        public GameObject Message;
        public House house;
        private bool isOpen = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O) && isOpen)
            {
                Debug.Log("Open O");
                Debug.Log(house);

                switch (house)
                {
                    case House.Hokage:
                        Panel.GetComponent<MissionManager>().Open();
                        break;
                    case House.Shop:
                        Panel.GetComponent<ShopManager>().OnShopBtnClick();
                        break;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Message.SetActive(true);
                isOpen = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Message.SetActive(false);
                isOpen = false;
            }
        }

    }
}
