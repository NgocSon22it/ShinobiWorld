using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Manager : MonoBehaviour
{
    [Header("Instance")]
    public static Skill_Manager Instance;

    [Header("This Panel")]
    [SerializeField] GameObject SkillPanel;


    [Header("Skill")]
    [SerializeField] GameObject Skill_Item;
    [SerializeField] Transform Skill_Content;

    [Header("Detail")]
    [SerializeField] Image SkillImage;
    [SerializeField] TMP_Text SkillNameTxt;
    [SerializeField] TMP_Text SkillCoinTxt;
    [SerializeField] Button Skill_BuyBtn;
    [SerializeField] TMP_Text SkillDescriptionTxt;
    [SerializeField] TMP_Text ErrorTxt;

    [Header("Status")]
    [SerializeField] GameObject BuyButtonPanel;
    [SerializeField] GameObject UpgradeButtonPanel;

    [Header("Upgrade Skill")]
    [SerializeField] GameObject UpgradePanel;
    [SerializeField] TMP_Text Upgrade_SkillNameTxt;
    [SerializeField] GameObject Upgrade_Cost;
    [SerializeField] Button Upgrade_Btn;
    [SerializeField] TMP_Text Upgrade_CostTxt;

    [SerializeField] TMP_Text Upgrade_CurrentLevelTxt;
    [SerializeField] TMP_Text Upgrade_CurrentDamageTxt;
    [SerializeField] TMP_Text Upgrade_CurrentCooldownTxt;
    [SerializeField] TMP_Text Upgrade_CurrentChakraTxt;

    [SerializeField] TMP_Text Upgrade_NextLevelTxt;
    [SerializeField] TMP_Text Upgrade_NextDamageTxt;
    [SerializeField] TMP_Text Upgrade_NextCooldownTxt;
    [SerializeField] TMP_Text Upgrade_NextChakraTxt;

    [SerializeField] TMP_Text UpgradeSkillErrorTxt;

    [SerializeField] GameObject CanUpgradePanel;

    int DamageBonus, ChakraBonus;
    double CooldownBonus;

    public Skill_Entity SkillSelected;
    public AccountSkill_Entity AccountSkill;



    private void Awake()
    {
        Instance = this;
    }

    public void SetUpSelectedSkill(Skill_Entity skill_Entity)
    {
        SkillSelected = skill_Entity;
        ResetErrorMessage();
        SkillImage.sprite = Resources.Load<Sprite>(skill_Entity.Image);
        SkillNameTxt.text = skill_Entity.Name;
        SkillCoinTxt.text = skill_Entity.BuyCost.ToString();
        SkillDescriptionTxt.text = skill_Entity.Description;

        if (!SkillSelected.RoleInGameID.Equals(References.accountRefer.RoleInGameID))
        {
            SetUpBuyPanel(false, false);
            ErrorTxt.text = Message.Skill_NotForRole;
        }
        else
        {
            if (References.accountRefer.Level < SkillSelected.LevelUnlock)
            {
                ErrorTxt.text = string.Format(Message.Skill_Unlock, SkillSelected.LevelUnlock);
                    //"Bạn cần đạt cấp độ " + SkillSelected.LevelUnlock + " để mở khóa!";
                SetUpBuyPanel(false, false);
            }
            else
            {

                if (CheckSkillOwned())
                {
                    SetUpBuyPanel(false, true);
                    ErrorTxt.text = "";
                }
                else
                {
                    SetUpBuyPanel(true,false);
                    ErrorTxt.text = "";
                }
            }
        }

        //LoadSkillList();
    }


    public void SetUpInformationUpgradeSkill()
    {
        AccountSkill = AccountSkill_DAO.GetAccountSkillByID(References.accountRefer.ID, SkillSelected.ID);
        if (AccountSkill != null)
        {
            ResetErrorMessage();
            UpgradePanel.SetActive(true);
            Upgrade_SkillNameTxt.text = SkillSelected.Name;

            Upgrade_CurrentLevelTxt.text = AccountSkill.Level.ToString();
            Upgrade_CurrentDamageTxt.text = AccountSkill.Damage.ToString("F2");
            Upgrade_CurrentCooldownTxt.text = AccountSkill.Cooldown.ToString("F2");
            Upgrade_CurrentChakraTxt.text = AccountSkill.Chakra.ToString("F2");

            Skill_BuyBtn.interactable = true;

            if (References.accountRefer.Coin < SkillSelected.BuyCost)
            {
                ErrorTxt.text = Message.NotEnoughMoney;
                Skill_BuyBtn.interactable = false;
            }

            SetUpStatusForUpgrade();
        }
    }
    public void SetUpStatusForUpgrade()
    {
        if (AccountSkill.Level < References.MaxUpgradeLevel)
        {
            SetUpUpgradePanel(true);
            Upgrade_NextLevelTxt.text = (AccountSkill.Level + 1).ToString();

            DamageBonus = Convert.ToInt32(AccountSkill.Damage * (1 + References.Uppercent_Skill_Damage / 100f));
            CooldownBonus = (AccountSkill.Cooldown * (1 - References.Uppercent_Skill_CoolDown / 100f));
            ChakraBonus = Convert.ToInt32((AccountSkill.Chakra * (1 - References.Uppercent_Skill_Chakra / 100f)));
            
            Upgrade_NextDamageTxt.text = DamageBonus.ToString("F2");
            Upgrade_NextCooldownTxt.text = CooldownBonus.ToString("F2");
            Upgrade_NextChakraTxt.text = ChakraBonus.ToString("F2");

            Upgrade_Btn.interactable = true;

            UpgradeSkillErrorTxt.text = string.Empty;
            Upgrade_CostTxt.text = SkillSelected.UpgradeCost.ToString();

            if (References.accountRefer.Coin < SkillSelected.UpgradeCost)
            {
                Upgrade_Btn.interactable = false;
                UpgradeSkillErrorTxt.text = Message.NotEnoughMoney;
            }
        }
        else
        {
            UpgradeSkillErrorTxt.text = Message.Skill_MaxLevel;
            SetUpUpgradePanel(false);
        }
    }

    public void CloseUpgradePanel()
    {
        UpgradePanel.SetActive(false);

    }

    public void LoadSkillList()
    {
        foreach (Transform trans in Skill_Content)
        {
            Destroy(trans.gameObject);
        }

        foreach (Skill_Entity Item in References.ListSkill)
        {
            Instantiate(Skill_Item, Skill_Content).GetComponent<Skill_Item>().SetUp(Item);
        }
    }

    public bool CheckSkillOwned()
    {
        List<AccountSkill_Entity> list = AccountSkill_DAO.GetAllSkillForAccount(References.accountRefer.ID);

        foreach (AccountSkill_Entity skill_Entity in list)
        {
            if (skill_Entity.SkillID.Equals(SkillSelected.ID))
            {
                return true;
            }
        }
        return false;
    }

    public void BuySelectedSkill()
    {
        if (References.accountRefer.Coin >= SkillSelected.BuyCost)
        {
            Skill_DAO.BuySkill(References.accountRefer.ID, SkillSelected);
            References.accountRefer.Coin -= SkillSelected.BuyCost;

            References.UpdateAccountToDB();
            Account_DAO.GetAccountPowerByID(References.accountRefer.ID);
            Game_Manager.Instance.ReloadPlayerProperties();

            LoadSkillList();
            SetUpSelectedSkill(SkillSelected);
            ErrorTxt.text = "";
        }
        else
        {
            ErrorTxt.text = Message.NotEnoughMoney;
        }
    }

    public void UpgradeSelectedSkill()
    {
        if (References.accountRefer.Coin >= SkillSelected.UpgradeCost)
        {
            Skill_DAO.UpgradeSkill(References.accountRefer.ID, SkillSelected.ID, DamageBonus, CooldownBonus, ChakraBonus);
            References.accountRefer.Coin -= SkillSelected.UpgradeCost;
            
            References.UpdateAccountToDB();
            Account_DAO.GetAccountPowerByID(References.accountRefer.ID);
            Game_Manager.Instance.ReloadPlayerProperties();

            Game_Manager.Instance.PlayerManager.GetComponent<PlayerBase>().PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().SetUpCoinUI(References.accountRefer.Coin);
            UpgradeSkillErrorTxt.text = "";
            SetUpInformationUpgradeSkill();
            
        }
        else
        {
            UpgradeSkillErrorTxt.text = Message.NotEnoughMoney;
        }
    }

    public void OpenSkillPanel()
    {
        if (References.ListSkill != null)
        {
            LoadSkillList();
            SetUpSelectedSkill(References.ListSkill[0]);
            Skill_Content.GetChild(0).gameObject.GetComponent<Skill_Item>().
                Background.color = new Color32(190, 140, 10, 255);

            SkillPanel.SetActive(true);
            Game_Manager.Instance.IsBusy = true;
        }
    }

    public void CloseSkillPanel()
    {
        SkillPanel.SetActive(false);
        Game_Manager.Instance.IsBusy = false;
    }


    public void ResetErrorMessage()
    {
        UpgradeSkillErrorTxt.text = "";
        ErrorTxt.text = "";
    }

    public void SetUpBuyPanel(bool buyValue, bool UpgradeValue)
    {
        BuyButtonPanel.SetActive(buyValue);
        UpgradeButtonPanel.SetActive(UpgradeValue);
    }

    public void SetUpUpgradePanel(bool CanUpgradeValue)
    {
        CanUpgradePanel.SetActive(CanUpgradeValue);
    }

    public void ResetColor()
    {
        foreach (Transform trans in Skill_Content)
        {
            trans.gameObject.GetComponent<Skill_Item>().
                Background.color = new Color32(110, 80, 60, 255);
        }
    }

}
