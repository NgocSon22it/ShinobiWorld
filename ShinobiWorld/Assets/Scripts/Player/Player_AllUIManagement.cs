using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Assets.Scripts.Hospital;
using Photon.Pun.Demo.SlotRacer;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player_AllUIManagement : MonoBehaviour
{
    public static Player_AllUIManagement Instance;

    [Header("Player")]
    public PlayerBase Player;

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

    [Header("Avatar")]
    [SerializeField] Image Avatar;

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
    [SerializeField] TMP_Text SkillOne_KeyCode;

    [Header("Skill Two")]
    [SerializeField] Image SkillTwo_Cooldown;
    [SerializeField] Image SkillTwo_Image;
    [SerializeField] TMP_Text SkillTwo_CostChakra;
    [SerializeField] TMP_Text SkillTwo_CooldownNumber;
    [SerializeField] TMP_Text SkillTwo_Keycode;

    [Header("Skill Three")]
    [SerializeField] Image SkillThree_Cooldown;
    [SerializeField] Image SkillThree_Image;
    [SerializeField] TMP_Text SkillThree_CostChakra;
    [SerializeField] TMP_Text SkillThree_CooldownNumber;
    [SerializeField] TMP_Text SkillThree_Keycode;

    [Header("House Open")]
    public GameObject House_Message;
    [SerializeField] TMP_Text HouseTxt;

    [Header("Full Map")]
    [SerializeField] GameObject MapPanel;

    [Header("Custom Key")]
    [SerializeField] List<TMP_Text> ListSkillTxt;

    [SerializeField] GameObject CustomKeyPanel;

    [SerializeField] TMP_Text CustomKeyMessage;

    [Header("Separate Status")]
    [SerializeField] List<GameObject> UI_Normal;
    [SerializeField] List<GameObject> UI_ArenaPK;

    [Header("Hospital")]
    public GameObject HospitalPanel;

    [Header("UpdateTrophy")]
    public GameObject Ticket;

    int IndexKey;
    string KeyboardExtension = "/Keyboard/";

    private bool isWaitingForKeyPress = false;
    [Header("Setup")]
    public GameObject BackgroundPanel;
    string image, skillValue;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadPlayerKey()
    {
        if (Player != null)
        {
            SetUp_LoadKey(0, "SkillOne");
            SetUp_LoadKey(1, "SkillTwo");
            SetUp_LoadKey(2, "SkillThree");
        }

    }

    public void ToggleFullMap(bool value)
    {
        Game_Manager.Instance.IsBusy = value;
        MapPanel.SetActive(value);
    }

    public void OpenCustomKeyPanel()
    {
        CustomKeyPanel.SetActive(true);
        LoadPlayerKey();
    }
    public void CloseCustomKeyPanel()
    {
        CustomKeyPanel.SetActive(false);
        isWaitingForKeyPress = false;
    }

    public void SelectKey(int Key)
    {
        LoadPlayerKey();
        ListSkillTxt[Key].text = "Nhấn vào phím muốn thay đổi";
        IndexKey = Key;
        isWaitingForKeyPress = true;
        CustomKeyMessage.text = "";
    }

    public void ChangeKey(int Key, string NewKey)
    {
        if (IsThatKeyInUse(NewKey))
        {
            CustomKeyMessage.text = "Phím đó sử dụng rồi!";
            isWaitingForKeyPress = true;
        }
        else
        {
            CustomKeyMessage.text = "";

            switch (Key)
            {
                case 0:
                    SetUp_ChangeKey("SkillOne", NewKey);
                    break;
                case 1:
                    SetUp_ChangeKey("SkillTwo", NewKey);
                    break;
                case 2:
                    SetUp_ChangeKey("SkillThree", NewKey);
                    break;
            }

            ListSkillTxt[IndexKey].text = ShowKey(NewKey);
            Game_Manager.Instance.ReloadPlayerProperties();
        }
    }

    public string ShowKey(string Key)
    {
        return Key.Replace(KeyboardExtension, "").ToUpper();
    }

    public void ShowHouseMessage(string HouseName)
    {
        House_Message.SetActive(true);
        HouseTxt.text = Message.OpenHouse + HouseName;
    }

    public void CloseHouseMessage()
    {
        House_Message.SetActive(false);
        HouseTxt.text = string.Empty;
    }

    private void Update()
    {
        if (Player != null)
        {
            SkillOne();
            SkillTwo();
            SkillThree();

            if (isWaitingForKeyPress)
            {
                foreach (var device in InputSystem.devices)
                {
                    foreach (var control in device.allControls)
                    {
                        if (control is KeyControl keyControl && keyControl.wasPressedThisFrame)
                        {
                            isWaitingForKeyPress = false;
                            ChangeKey(IndexKey, keyControl.path);
                            return;
                        }
                    }
                }
            }
        }
    }

    public void SetDefaultKey()
    {
        SetUp_DefaultKey(0, "SkillOne");
        SetUp_DefaultKey(1, "SkillTwo");
        SetUp_DefaultKey(2, "SkillThree");

        Game_Manager.Instance.ReloadPlayerProperties();
        CustomKeyMessage.text = "";
        isWaitingForKeyPress = false;
    }
    public void SetUp_LoadKey(int Key, string KeyName)
    {
        skillValue = Player.AccountEntity.CustomSettings.Find(obj => obj.SettingID == "Key_" + KeyName).Value;
        ListSkillTxt[Key].text = ShowKey(skillValue);
    }
    public void SetUp_DefaultKey(int Key, string KeyName)
    {
        skillValue = References.listSetting.Find(obj => obj.ID == "Key_" + KeyName).Value;
        ListSkillTxt[Key].text = ShowKey(skillValue);
        Player.playerInput.actions[KeyName].ApplyBindingOverride(skillValue);
        Account_DAO.ChangeKey(Player.AccountEntity.ID, "Key_" + KeyName, skillValue);
    }
    public void SetUp_ChangeKey(string KeyName, string NewKey)
    {
        Player.playerInput.actions[KeyName].ApplyBindingOverride(NewKey);
        Account_DAO.ChangeKey(Player.AccountEntity.ID, "Key_" + KeyName, NewKey);
    }
    public bool IsThatKeyInUse(string NewKey)
    {
        foreach (CustomSetting_Entity customSetting_Entity in Player.AccountEntity.CustomSettings)
        {
            if (NewKey.Equals(customSetting_Entity.Value))
            {
                return true;
            }
        }

        return false;
    }
    public void SetUp_SetUpPlayer(HasSkill_Entity skill, Image skillImage, TMP_Text skillcost, TMP_Text skillkey, string skillName)
    {
        if (skill != null)
        {
            image = References.listSkill.Find(obj => obj.ID == skill.SkillID).Image;
            skillImage.sprite = Resources.Load<Sprite>(image);
            skillcost.text = skill.Chakra.ToString();

            skillValue = Player.AccountEntity.CustomSettings.Find(obj => obj.SettingID == "Key_" + skillName).Value;
            skillkey.text = ShowKey(skillValue);
            Player.playerInput.actions[skillName].ApplyBindingOverride(skillValue);
        }
    }
    public void SetUpPlayer(PlayerBase player)
    {
        Player = player;

        switch (player.accountStatus)
        {
            case AccountStatus.Normal:
                SetUp_UI(true, false);
                break;
            default:
                SetUp_UI(false, true);
                break;

        }
        if (Player != null)
        {
            SetUp_SetUpPlayer(player.SkillOne_Entity, SkillOne_Image, SkillOne_CostChakra, SkillOne_KeyCode, "SkillOne");
            SetUp_SetUpPlayer(player.SkillTwo_Entity, SkillTwo_Image, SkillTwo_CostChakra, SkillTwo_Keycode, "SkillTwo");
            SetUp_SetUpPlayer(player.SkillThree_Entity, SkillThree_Image, SkillThree_CostChakra, SkillThree_Keycode, "SkillThree");
        }

    }

    public void SetUp_UI(bool NormalUI, bool ArenaPkUI)
    {
        foreach(GameObject a in UI_Normal)
        {
            a.SetActive(NormalUI);
        }
        foreach (GameObject a in UI_ArenaPK)
        {
            a.SetActive(ArenaPkUI);
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

    public void LoadAvatarUI(string avatarPath)
    {
        Avatar.sprite = Resources.Load<Sprite>("Player/Avatar/" + avatarPath);
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
        Player_Info.Instance.Open();
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

    public void ShowDiePanel(int timeRespawn)
    {
        HospitalPanel.GetComponent<Hospital>().SetDuration(timeRespawn).Begin();
    }
}
