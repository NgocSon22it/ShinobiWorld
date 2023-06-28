using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MailBox
{
    public class MailBoxItem : MonoBehaviour
    {
        public TMP_Text Title;
        public MailBox_Entity mail;
        public AccountMailBox_Entity accountMail;
        public bool isSystem = true;

        public void OnClick()
        {
            MailBoxManager.Instance.ResetColor();
            GetComponent<Image>().color = new Color32(190, 140, 10, 255);
            MailboxDetail.Instance.ShowDetail(accountMail, mail, isSystem);

            AccountMailBox_DAO.Read(accountMail.ID,
                                    accountMail.AccountID, accountMail.MailBoxID);
        }

        public void Setup(AccountMailBox_Entity accountMail, bool isFirst, bool isSystem = true)
        {
            this.isSystem = isSystem;
            this.accountMail = accountMail;
            mail = References.listMailBox.Find(obj => obj.ID == accountMail.MailBoxID);

            Title.text = string.Format(mail.Title,
                            accountMail.DateAdd.Month.ToString(), accountMail.DateAdd.Year.ToString());

            if(isFirst) OnClick();
        }
    }
}
