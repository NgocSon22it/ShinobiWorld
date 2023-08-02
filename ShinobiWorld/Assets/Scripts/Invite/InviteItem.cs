using Assets.Scripts.Database.Entity;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InviteItem : MonoBehaviour
{
    [SerializeField] TMP_Text NameTxt;
    Account_Entity account_Entity;

    public void SetUp(Account_Entity account_Entity)
    {
        this.account_Entity = account_Entity;   
        NameTxt.text = account_Entity.Name;
    }

    public void OnclickInvite()
    {
        InviteManager.Instance.SendInvite(account_Entity.Name);
    }
}
