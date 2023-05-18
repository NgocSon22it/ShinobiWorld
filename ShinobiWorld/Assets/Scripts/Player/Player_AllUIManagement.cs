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

    [SerializeField] TMP_Text HealthNumberTxt;
    [SerializeField] TMP_Text ChakraNumberTxt;

    [Header("Name")]
    [SerializeField] TMP_Text CoinTxt;

    [Header("Strength")]    
    [SerializeField] TMP_Text StrengthTxt;

    [Header("Power")]
    [SerializeField] TMP_Text PowerTxt;

    public void SetUpCoinUI(int Coin)
    {
        CoinTxt.text = Coin.ToString();
    }

    public void LoadPowerUI(int Power)
    {
        PowerTxt.text = Power.ToString();
    }

    public void LoadStrengthUI(int Strength)
    {
        StrengthTxt.text = Strength.ToString() + " / 100";
    }

    public void LoadExperienceUI(int Level, int CurrentExp, int NextLevelExp)
    {
        CurrentLevel.text = Level.ToString();
        ExperienceTxt.text = CurrentExp.ToString() + " / " + NextLevelExp.ToString();
        CurrentExpBar.fillAmount = (float)CurrentExp / (float)NextLevelExp;
    }

    public void LoadNameUI(string Name)
    {
        NickNameTxt.text = Name;
    }

    public void LoadHealthNChakraUI(float TotalHealth, float CurrentHealth, float TotalChakra, float CurrentChakra)
    {
        CurrentHealth_UI.fillAmount = (float)CurrentHealth / (float)TotalHealth;
        CurrentChakra_UI.fillAmount = (float)CurrentChakra / (float)TotalChakra;

        HealthNumberTxt.text = CurrentHealth + " / " + TotalHealth;
        ChakraNumberTxt.text = CurrentChakra + " / " + TotalChakra;
    }



}
