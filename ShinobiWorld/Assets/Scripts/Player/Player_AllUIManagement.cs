using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_AllUIManagement : MonoBehaviour
{
    public static Player_AllUIManagement Instance;

    [Header("Player")]
    [SerializeField] PlayerBase Player;

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

    [Header("Skill Bar")]
    [Header("Skill One")]
    [SerializeField] Image SkillOne_Cooldown;
    [SerializeField] Image SkillOne_Image;
    [SerializeField] TMP_Text SkillOne_CostChakra;
    [SerializeField] TMP_Text SkillOne_CooldownNumber;

    [Header("Skill Two")]
    [SerializeField] Image SkillTwo_Cooldown;
    [SerializeField] Image SkillTwo_Image;
    [SerializeField] TMP_Text SkillTwo_CostChakra;
    [SerializeField] TMP_Text SkillTwo_CooldownNumber;

    [Header("Skill Three")]
    [SerializeField] Image SkillThree_Cooldown;
    [SerializeField] Image SkillThree_Image;
    [SerializeField] TMP_Text SkillThree_CostChakra;
    [SerializeField] TMP_Text SkillThree_CooldownNumber;

    string image;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Player != null)
        {
            SkillOne();
            SkillTwo();
            SkillThree();
        }
    }

    public void SetUpPlayer(PlayerBase player)
    {
        Player = player;
        if (player.SkillOne_Entity != null)
        {
            image = References.ListSkill.Find(obj => obj.ID == player.SkillOne_Entity.SkillID).Image;
            SkillOne_Image.sprite = Resources.Load<Sprite>(image);
            SkillOne_CostChakra.text = player.SkillOne_Entity.Chakra.ToString();
        }
        if (player.SkillTwo_Entity != null)
        {
            image = References.ListSkill.Find(obj => obj.ID == player.SkillTwo_Entity.SkillID).Image;
            SkillTwo_Image.sprite = Resources.Load<Sprite>(image);
            SkillTwo_CostChakra.text = player.SkillTwo_Entity.Chakra.ToString();
        }
        if (player.SkillThree_Entity != null)
        {
            image = References.ListSkill.Find(obj => obj.ID == player.SkillThree_Entity.SkillID).Image;
            SkillThree_Image.sprite = Resources.Load<Sprite>(image);
            SkillThree_CostChakra.text = player.SkillThree_Entity.Chakra.ToString();
        }
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

    public void SkillOne()
    {
        if (Player.SkillOne_Entity != null)
        {

            if (Player.SkillOneCooldown_Current > 0)
            {
                SkillOne_CooldownNumber.gameObject.SetActive(true);

                SkillOne_Cooldown.fillAmount = Player.SkillOneCooldown_Current / Player.SkillOneCooldown_Total;
                SkillOne_CooldownNumber.text = Player.SkillOneCooldown_Current.ToString("F1");
            }
            else
            {
                SkillOne_Cooldown.fillAmount = Player.SkillOneCooldown_Current / Player.SkillOneCooldown_Total;
                SkillOne_CooldownNumber.gameObject.SetActive(false);

                if (Player.AccountEntity.CurrentChakra < Player.SkillOne_Entity.Chakra)
                {

                    SkillOne_Cooldown.fillAmount = 1f;
                }
            }
        }
        else
        {
            SkillOne_Cooldown.fillAmount = 1f;
        }
    }

    public void SkillTwo()
    {
        if (Player.SkillTwo_Entity != null)
        {

            if (Player.SkillTwoCooldown_Current > 0)
            {
                SkillTwo_CooldownNumber.gameObject.SetActive(true);

                SkillTwo_Cooldown.fillAmount = Player.SkillTwoCooldown_Current / Player.SkillTwoCooldown_Total;
                SkillTwo_CooldownNumber.text = Player.SkillTwoCooldown_Current.ToString("F1");
            }
            else
            {
                SkillTwo_Cooldown.fillAmount = Player.SkillTwoCooldown_Current / Player.SkillTwoCooldown_Total;
                SkillTwo_CooldownNumber.gameObject.SetActive(false);

                if (Player.AccountEntity.CurrentChakra < Player.SkillTwo_Entity.Chakra)
                {

                    SkillTwo_Cooldown.fillAmount = 1f;
                }
            }
        }
        else
        {
            SkillTwo_Cooldown.fillAmount = 1f;
        }
    }
    public void SkillThree()
    {
        if (Player.SkillThree_Entity != null)
        {

            if (Player.SkillThreeCooldown_Current > 0)
            {
                SkillThree_CooldownNumber.gameObject.SetActive(true);

                SkillThree_Cooldown.fillAmount = Player.SkillThreeCooldown_Current / Player.SkillThreeCooldown_Total;
                SkillThree_CooldownNumber.text = Player.SkillThreeCooldown_Current.ToString("F1");
            }
            else
            {
                SkillThree_Cooldown.fillAmount = Player.SkillThreeCooldown_Current / Player.SkillThreeCooldown_Total;
                SkillThree_CooldownNumber.gameObject.SetActive(false);

                if (Player.AccountEntity.CurrentChakra < Player.SkillThree_Entity.Chakra)
                {

                    SkillThree_Cooldown.fillAmount = 1f;
                }
            }
        }
        else
        {
            SkillThree_Cooldown.fillAmount = 1f;
        }
    }

}
