using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_AllUIManagement : MonoBehaviour
{
    [Header("Level UI")]
    [SerializeField] TMP_Text CurrentLevel;
    [SerializeField] TMP_Text ExperienceTxt;
    [SerializeField] Image CurrentExpBar;

    public void SetUpExperienceUI(int Level, int CurrentExp, int NextLevelExp)
    {
        CurrentLevel.text = Level.ToString();
        ExperienceTxt.text = CurrentExp.ToString() + " / " + NextLevelExp.ToString();
        CurrentExpBar.fillAmount = (float)CurrentExp / (float)NextLevelExp;
    }




}
