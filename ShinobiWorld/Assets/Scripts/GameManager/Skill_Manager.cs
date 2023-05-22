using Assets.Scripts.Database.Entity;
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

    [SerializeField] GameObject CanBuyPanel;
    [SerializeField] GameObject CanNotBuyPanel;


    [SerializeField] TMP_Text CanNotBuyText;

    [SerializeField] TMP_Text BuySkillErrorTxt;

    public Skill_Entity SkillSelected;
 

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
            SetUpBuyPanel(false, true);
            CanNotBuyText.text = "Kỹ năng này không trong vai trò của bạn!";
        }
        else
        {
            if (References.accountRefer.Level < SkillSelected.LevelUnlock)
            {
                CanNotBuyText.text = "Bạn cần đạt cấp độ " + SkillSelected.LevelUnlock + " để mở khóa!";
                SetUpBuyPanel(false, true);
            }
            else
            {

                if (CheckSkillOwned())
                {
                    SetUpBuyPanel(false, true);
                    CanNotBuyText.text = "Đã sở hữu!";
                }
                else
                {
                    SetUpBuyPanel(true, false);
                    CanNotBuyText.text = "";
                }
            }
        }

        LoadSkillList();
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
            References.LoadAccount();
            Game_Manager.Instance.PlayerManager.GetComponent<PlayerBase>().LoadAccountSkill();
            LoadSkillList();
            SetUpSelectedSkill(SkillSelected);
            BuySkillErrorTxt.text = "";
        }
        else
        {
            BuySkillErrorTxt.text = "Bạn không đủ xu để mua!";
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
        BuySkillErrorTxt.text = "";
    }

    public void SetUpBuyPanel(bool CanBuyValue, bool CanNotBuyValue)
    {
        CanBuyPanel.SetActive(CanBuyValue);
        CanNotBuyPanel.SetActive(CanNotBuyValue);
    }

}
