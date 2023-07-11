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
        public Mail_Entity selectedMail;
        public MailBox_Entity selectedMailbox;
        public bool isSystem = true;

        public void OnClick()
        {
            MailBoxManager.Instance.ResetColor();
            GetComponent<Image>().color = new Color32(190, 140, 10, 255);
            MailboxDetail.Instance.ShowDetail(selectedMailbox, selectedMail, isSystem);

            MailBox_DAO.Read(selectedMailbox.ID,
                                    selectedMailbox.AccountID, selectedMailbox.MailID);
        }

        public void Setup(MailBox_Entity mailbox, bool isShow, bool isSystem = true)
        {
            this.isSystem = isSystem;
            this.selectedMailbox = mailbox;
            selectedMail = References.listMail.Find(obj => obj.ID == mailbox.MailID);

            Title.text = string.Format(selectedMail.Title,
                            mailbox.DateAdd.Month.ToString(), mailbox.DateAdd.Year.ToString());
            
            if (mailbox.IsRead) GetComponent<Image>().color = new Color32(110, 80, 60, 150);
            if (isShow) OnClick();
        }
    }
}
