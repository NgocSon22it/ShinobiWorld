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
        int index;

        public void OnClick()
        {
            MailBoxManager.Instance.ResetColor();
            GetComponent<Image>().color = References.ItemColorSelected;
            MailboxDetail.Instance.ShowDetail(selectedMailbox, selectedMail, isSystem);
            
            References.listMailBox[index].IsRead = true;
            MailBoxManager.Instance.CheckNotify();

            MailBox_DAO.Read(selectedMailbox.ID,
                                    selectedMailbox.AccountID, selectedMailbox.MailID);
        }

        public void Setup(MailBox_Entity mailbox, bool isShow, int index, bool isSystem = true)
        {
            this.isSystem = isSystem;
            this.selectedMailbox = mailbox;
            this.index = index;
            GetComponent<Image>().color = References.MailColorDefaul;
            selectedMail = References.listMail.Find(obj => obj.ID == mailbox.MailID);

            Title.text = string.Format(selectedMail.Title,
                            mailbox.AddDate.Month.ToString(), mailbox.AddDate.Year.ToString());
            
            if (mailbox.IsRead) GetComponent<Image>().color = References.ItemColorDefaul;

            if (isShow) OnClick();
        }
    }
}
