using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_AllUIManagement : MonoBehaviour
{
    public static Player_AllUIManagement Instance;

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

    [Header("Mission")]
    [SerializeField] GameObject Mission;
    [SerializeField] TMP_Text MissionTxt;

    private void Awake()
    {
        Instance = this;
    }

    public void SetUpCoinUI(int Coin)
    {
        CoinTxt.text = Coin.ToString();
    }

    public void LoadPowerUI(int Power)
    {
        PowerTxt.text = Power.ToString();
    }

    public void LoadStrengthUI(int Strength, int CurrentStrengh)
    {
        StrengthTxt.text = CurrentStrengh.ToString() + " / " + Strength.ToString();
    }

    public void LoadExperienceUI(int Level, int CurrentExp, int NextLevelExp)
    {
        CurrentLevel.text = Level.ToString();
        if (Level < 30)
        {
            ExperienceTxt.text = CurrentExp.ToString() + " / " + NextLevelExp.ToString();
            CurrentExpBar.fillAmount = (float)CurrentExp / (float)NextLevelExp;
        }
        else
        {
            ExperienceTxt.text = "Cấp tối đa!";
            CurrentExpBar.fillAmount = 1f;
        } 
    }

    public void LoadNameUI(string Name)
    {
        NickNameTxt.text = Name;
    }

    public void LoadHealthUI(float TotalHealth, float CurrentHealth)
    {
        CurrentHealth_UI.fillAmount = (float)CurrentHealth / (float)TotalHealth;
        HealthNumberTxt.text = CurrentHealth + " / " + TotalHealth;
    }

    public void LoadChakraUI(float TotalChakra, float CurrentChakra)
    {
        CurrentChakra_UI.fillAmount = (float)CurrentChakra / (float)TotalChakra;
        ChakraNumberTxt.text = CurrentChakra + " / " + TotalChakra;
    }

    public void ShowDetailInfo()
    {
        Player_Info.Instance.OnAvatarBtnClick();
    }

    public void ShowMission(string content)
    {
        Mission.SetActive(true);
        MissionTxt.text = content;
    }

    public void CloseMission()
    {
        Mission.SetActive(false);
        MissionTxt.text = string.Empty;
    }
}
