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

    [Header("Coin")]
    [SerializeField] TMP_Text PlayerCoinTxt;

    [Header("Selected Skill")]
    [SerializeField] Image SkillImage;
    [SerializeField] TMP_Text SkillNameTxt;
    [SerializeField] TMP_Text SkillCoinTxt;
    [SerializeField] TMP_Text SkillDescriptionTxt;
    [SerializeField] TMP_Text ButtonCoinTxt;

    [Header("Status")]
    [SerializeField] GameObject BuyButtonPanel;
    [SerializeField] GameObject UpgradeButtonPanel;

    [Header("Upgrade Skill")]
    [SerializeField] GameObject UpgradePanel;
    [SerializeField] Image Upgrade_SkillImage;
    [SerializeField] TMP_Text Upgrade_SkillNameTxt;
    [SerializeField] TMP_Text Upgrade_SkillDescriptionTxt;
    [SerializeField] TMP_Text Upgrade_ButtonCoinTxt;
    [SerializeField] TMP_Text Upgrade_CurrentLevelTxt;
    [SerializeField] TMP_Text Upgrade_CurrentDamageTxt;
    [SerializeField] TMP_Text Upgrade_CurrentCooldownTxt;
    [SerializeField] TMP_Text Upgrade_CurrentChakraTxt;

    [SerializeField] TMP_Text Upgrade_NextLevelTxt;
    [SerializeField] TMP_Text Upgrade_NextDamageTxt;
    [SerializeField] TMP_Text Upgrade_NextCooldownTxt;
    [SerializeField] TMP_Text Upgrade_NextChakraTxt;
    int DamageBonus, ChakraBonus;
    double CooldownBonus;

    [SerializeField] GameObject CanUpgradePanel;
    [SerializeField] GameObject CanNotUpgradePanel;

    [SerializeField] TMP_Text ErrorTxt;
    [SerializeField] TMP_Text UpgradeSkillErrorTxt;

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
        ButtonCoinTxt.text = skill_Entity.BuyCost.ToString();


        if (!SkillSelected.RoleInGameID.Equals(References.accountRefer.RoleInGameID))
        {
            SetUpBuyPanel(false, false);
            ErrorTxt.text = "Kỹ năng này không trong vai trò của bạn!";
        }
        else
        {
            if (References.accountRefer.Level < SkillSelected.LevelUnlock)
            {
                ErrorTxt.text = "Bạn cần đạt cấp độ " + SkillSelected.LevelUnlock + " để mở khóa!";
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

        LoadSkillList();
    }


    public void SetUpInformationUpgradeSkill()
    {
        AccountSkill = AccountSkill_DAO.GetAccountSkillByID(References.accountRefer.ID, SkillSelected.ID);
        if (AccountSkill != null)
        {
            ResetErrorMessage();
            UpgradePanel.SetActive(true);
            Upgrade_SkillImage.sprite = Resources.Load<Sprite>(SkillSelected.Image);
            Upgrade_SkillNameTxt.text = SkillSelected.Name;
            Upgrade_SkillDescriptionTxt.text = SkillSelected.Description;
            Upgrade_ButtonCoinTxt.text = SkillSelected.BuyCost.ToString();
            Upgrade_CurrentLevelTxt.text = AccountSkill.Level.ToString();
            Upgrade_CurrentDamageTxt.text = AccountSkill.Damage.ToString("F2");
            Upgrade_CurrentCooldownTxt.text = AccountSkill.Cooldown.ToString("F2");
            Upgrade_CurrentChakraTxt.text = AccountSkill.Chakra.ToString("F2");
            SetUpStatusForUpgrade();
        }
    }
    public void SetUpStatusForUpgrade()
    {
        if (AccountSkill.Level < References.MaxUpgradeLevel)
        {
            SetUpUpgradePanel(true, false);
            Upgrade_NextLevelTxt.text = (AccountSkill.Level + 1).ToString();

            DamageBonus = Convert.ToInt32(AccountSkill.Damage * (1 + References.Uppercent_Skill_Damage / 100f));
            CooldownBonus = (AccountSkill.Cooldown * (1 - References.Uppercent_Skill_CoolDown / 100f));
            ChakraBonus = Convert.ToInt32((AccountSkill.Chakra * (1 - References.Uppercent_Skill_Chakra / 100f)));
            
            
            
            Upgrade_NextDamageTxt.text = DamageBonus.ToString("F2");
            Upgrade_NextCooldownTxt.text = CooldownBonus.ToString("F2");
            Upgrade_NextChakraTxt.text = ChakraBonus.ToString("F2");
            
        }
        else
        {
            SetUpUpgradePanel(false, true);
        }
    }

    public void CloseUpgradePanel()
    {
        UpgradePanel.SetActive(false);

    }

    public void LoadSkillList()
    {
        PlayerCoinTxt.text = References.accountRefer.Coin.ToString();
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
            Game_Manager.Instance.ReloadPlayerProperties();

            LoadSkillList();
            SetUpSelectedSkill(SkillSelected);
            ErrorTxt.text = "";
        }
        else
        {
            ErrorTxt.text = "Bạn không đủ xu để mua!";
        }
    }

    public void UpgradeSelectedSkill()
    {
        if (References.accountRefer.Coin >= SkillSelected.UpgradeCost)
        {
            Skill_DAO.UpgradeSkill(References.accountRefer.ID, SkillSelected.ID, DamageBonus, CooldownBonus, ChakraBonus);
            References.accountRefer.Coin -= SkillSelected.UpgradeCost;
            References.UpdateAccountToDB();

            PlayerCoinTxt.text = References.accountRefer.Coin.ToString();
            Game_Manager.Instance.ReloadPlayerProperties();

            Game_Manager.Instance.PlayerManager.GetComponent<PlayerBase>().PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().SetUpCoinUI(References.accountRefer.Coin);
            SetUpInformationUpgradeSkill();
            UpgradeSkillErrorTxt.text = "";
        }
        else
        {
            UpgradeSkillErrorTxt.text = "Bạn không đủ xu để mua!";
        }
    }

    public void OpenSkillPanel()
    {
        if (References.ListSkill != null)
        {
            SetUpSelectedSkill(References.ListSkill[0]);
            LoadSkillList();
            SkillPanel.SetActive(true);

        }
    }

    public void CloseSkillPanel()
    {
        SkillPanel.SetActive(false);
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

    public void SetUpUpgradePanel(bool CanUpgradeValue, bool CanNotUpgradeValue)
    {
        CanUpgradePanel.SetActive(CanUpgradeValue);
        CanNotUpgradePanel.SetActive(CanNotUpgradeValue);
    }

}
