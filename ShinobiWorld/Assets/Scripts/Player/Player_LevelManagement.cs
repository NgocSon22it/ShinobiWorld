using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class Player_LevelManagement : MonoBehaviour
{
    public void AddExperience(int Amount, int Experience, int ExpercienceToNextLevel, int Level)
    {
        Experience += Amount;
        while (Experience >= ExpercienceToNextLevel)
        {
            Level++;
            Experience -= ExpercienceToNextLevel;
            ExpercienceToNextLevel = Level * 100;
            LevelUpReward();
        }
        //SetUpExperienceUI();
    }

    public void LevelUpReward()
    {

    }
}
