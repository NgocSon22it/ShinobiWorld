﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.Database.Entity;
using Assets.Scripts.Database.DAO;

namespace Assets.Scripts.MailBox
{
    public class MailboxDetail : MonoBehaviour
    {
        [Header("Detail")]
        public TMP_Text Title;
        public TMP_Text Content;
        public GameObject ClaimBtn;

        [Header("Bonus")]
        public GameObject BonusPanel;
        public TMP_Text Coin, EquipmentTxt;
        public Image EquipmentImg;

        [Header("BonusDouble")]
        public GameObject BonusDoublePanel;
        public TMP_Text CoinDouble, EquipmentTxt1, EquipmentTxt2;
        public Image EquipmentImg1, EquipmentImg2;

        public static MailboxDetail Instance;

        public MailBox_Entity selectedMail;
        public AccountMailBox_Entity selectedAccountMail;

        private void Awake()
        {
            Instance = this;
        }

        public void ShowDetail(AccountMailBox_Entity accountMail, MailBox_Entity mail, bool isSystem)
        {
            selectedMail = mail;
            selectedAccountMail = accountMail;

            Title.text = string.Format(mail.Title,
                            accountMail.DateAdd.Month.ToString(), accountMail.DateAdd.Year.ToString());

            Content.text = string.Format(mail.Content,
                            accountMail.DateAdd.Month.ToString(), accountMail.DateAdd.Year.ToString());
            Debug.Log(accountMail.IsClaim.ToString());
            ClaimBtn.SetActive(true);

            if (isSystem || accountMail.IsClaim) ClaimBtn.SetActive(false);
        }

        public void ShowBonus()
        {
            var equip1 = References.RandomEquipmentBonus(selectedMail.CategoryEquipmentID);
            var equip2 = References.RandomEquipmentBonus(selectedMail.CategoryEquipmentID);

            if(equip1 != equip2)
            {
                BonusDoublePanel.SetActive(true);
                BonusPanel.SetActive(false);

                CoinDouble.text = selectedMail.CoinBonus.ToString();

                EquipmentTxt1.text = equip1.Name;
                EquipmentImg1.sprite = Resources.Load<Sprite>(equip1.Image);

                EquipmentTxt2.text = equip2.Name;
                EquipmentImg2.sprite = Resources.Load<Sprite>(equip2.Image);
            }
            else
            {
                BonusDoublePanel.SetActive(false);
                BonusPanel.SetActive(true);

                Coin.text = selectedMail.CoinBonus.ToString();

                EquipmentTxt.text = equip1.Name + " (x2)";
                EquipmentImg.sprite = Resources.Load<Sprite>(equip1.Image);
            }

            References.AddCoin(selectedMail.CoinBonus);

            AccountMailBox_DAO.TakeBonus(selectedAccountMail.ID,
                                        selectedAccountMail.AccountID, selectedAccountMail.MailBoxID,
                                        equip1.ID, equip2.ID);

            MailBoxManager.Instance.Reload();
        }

        public void Delete()
        {
            AccountMailBox_DAO.Delete(selectedAccountMail.ID,
                                        selectedAccountMail.AccountID, selectedAccountMail.MailBoxID);
            MailBoxManager.Instance.Reload();
        }

        public void Close()
        {
            BonusDoublePanel.SetActive(false);
            BonusPanel.SetActive(false);
        }
    }
}
