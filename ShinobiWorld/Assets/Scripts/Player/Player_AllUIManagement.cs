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

    [Header("Name")]
    [SerializeField] TMP_Text NickNameTxt;

    [Header("Health Chakra")]
    [SerializeField] Image CurrentHealth_UI;
    [SerializeField] Image CurrentChakra_UI;

    public void SetUpExperienceUI(int Level, int CurrentExp, int NextLevelExp)
    {
        CurrentLevel.text = Level.ToString();
        ExperienceTxt.text = CurrentExp.ToString() + " / " + NextLevelExp.ToString();
        CurrentExpBar.fillAmount = (float)CurrentExp / (float)NextLevelExp;
    }

    public void SetUpNameUI(string Name)
    {
        NickNameTxt.text = Name;
    }

    public void SetUpHealthNChakraUI(float Health, float Chakra)
    {
        CurrentHealth_UI.fillAmount = Health;
        CurrentChakra_UI.fillAmount = Chakra;
    }


}
