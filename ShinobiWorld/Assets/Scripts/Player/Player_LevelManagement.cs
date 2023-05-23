using Assets.Scripts.Database.DAO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class Player_LevelManagement : MonoBehaviour
{
    public Account_Entity AccountEntity = new Account_Entity();

    int ExpercienceToNextLevel;

    public void AddExperience(int Amount)
    {
        if (AccountEntity != null && AccountEntity.Level < 30)
        {
            AccountEntity.Exp += Amount;
            while (AccountEntity.Exp >= ExpercienceToNextLevel)
            {
                AccountEntity.Level++;
                AccountEntity.Exp -= ExpercienceToNextLevel;
                ExpercienceToNextLevel = AccountEntity.Level * 100;
                LevelUpReward();
            }
        }
    }

    public void SetUpAccountEntity(Account_Entity AccountEntity)
    {
        this.AccountEntity = AccountEntity;
        ExpercienceToNextLevel = (AccountEntity.Level * 100);
    }

    public void LevelUpReward()
    {
        Account_DAO.BonusLevelUp(AccountEntity.ID, References.Uppercent_Account);
    }
}
