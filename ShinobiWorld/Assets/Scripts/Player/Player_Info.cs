using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Info : MonoBehaviourPunCallbacks
{
    public static Player_Info Instance;
    public GameObject InfoPanel;
    public string ID ;

    [Header("Preview")]
    public GameObject Preview;
    public TMP_Text Name_Preview;
    public TMP_Text Role;
    public TMP_Text Trophy;
    public Button Skill_1Btn, Skill_2Btn, Skill_3Btn, Equip_ShirtBtn, Equip_PantBtn, Equip_WeaponBtn, WeaponBtn;

    [Header("DetailPlayer")]
    public GameObject DetailPlayer;
    public TMP_Text Name_Player, Level_Player, Health_Player, Chakra_Player, 
                    Strength_Player, Power_Player, Exp_Player, Coin_Player;

    [Header("DetailWeapon")]
    public GameObject DetailWeapon;
    public TMP_Text Name_Weapon, Level_Weapon, Damage_Weapon;

    [Header("DetailSkill")]
    public GameObject DetailSkill;
    public TMP_Text Name_Skill, Level_Skill, Damage_Skill, Chakra_Skill, Cooldown_Skill;

    [Header("DetailEquipment")]
    public GameObject DetailEquipment;
    public TMP_Text Name_Equipment, Level_Equipment, Damage_Equipment, Health_Equipment, Chakra_Equipment;

    [Header("Layout")]
    //Sprite layout
    //Skin
    public SpriteRenderer Shirt;
    public SpriteRenderer RightFoot;
    public SpriteRenderer LeftFoot;
    public SpriteRenderer RightHand;
    public SpriteRenderer LeftHand;
    //Eye
    public SpriteRenderer Eye;
    //Hair
    public SpriteRenderer Hair;
    //Mouth
    public SpriteRenderer Mouth;
    //Weapon
    public SpriteRenderer Weapon;

    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        DetailPlayer.SetActive(false);
        DetailWeapon.SetActive(false);
        DetailSkill.SetActive(false); 
        DetailEquipment.SetActive(false);
    }

    public void OnAvatarBtnClick()
    {
        References.accountRefer = Account_DAO.GetAccountByID("vRsLqEXrnhMpK48YRLlYMNBElTf1");

        References.listAccountEquipment = AccountEquipment_DAO.GetAllByUserID(References.accountRefer.ID);
        References.listAccountSkill = AccountSkill_DAO.GetAllSkillForAccount(References.accountRefer.ID);
        References.accountWeapon = AccountWeapon_DAO.GetAccountWeaponByID(References.accountRefer.ID);
        References.weapon = Weapon_DAO.GetWeaponByID(References.accountWeapon.WeaponID);

        InfoPanel.SetActive(true);
        
        Preview.SetActive(true);
        SetupPreview();

        Init();

        DetailPlayer.SetActive(true);
        ShowDetailPlayer();
    }

    public void OnCloseBtnClick()
    {
        InfoPanel.SetActive(false);
    }

    public void SetupPreview()
    {
        //Name_Preview.text = photonView.Owner.NickName;
        Name_Preview.text = "Thien";
        Role.text = References.listRole.Find(obj => obj.ID == References.accountRefer.RoleInGameID).Name;
        Trophy.text = References.listTrophy.Find(obj => obj.ID == References.accountRefer.TrophiesID).Name;

        LoadLayout();
        SetupPreviewBtn();
        SetupPreviewSkillBtn();
        SetupPreviewEquipmentBtn();

        //WeaponBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.weapon.Image +"Btn");
    }

    public void SetupPreviewBtn()
    {
        Skill_1Btn.interactable = false;
        Skill_2Btn.interactable = false;
        Skill_3Btn.interactable = false;
        Equip_ShirtBtn.interactable = false;
        Equip_PantBtn.interactable = false;
        Equip_WeaponBtn.interactable = false;
    }

    public void SetupPreviewSkillBtn()
    {
        var role = References.accountRefer.RoleInGameID.Replace("Role_", "");
        var list = References.ListSkill.FindAll(obj => obj.RoleInGameID == References.accountRefer.RoleInGameID);

        foreach (var skill in list)
        {
            var index = skill.ID.Replace("Skill_" + role, "");
            switch (index)
            {
                case "One":
                    Skill_1Btn.GetComponentInChildren<TMP_Text>().text = skill.Name;
                    break;
                case "Two":
                    Skill_2Btn.GetComponentInChildren<TMP_Text>().text = skill.Name;
                    break;
                case "Three":
                    Skill_3Btn.GetComponentInChildren<TMP_Text>().text = skill.Name;
                    break;
            }
        }

        foreach (var skill in References.listAccountSkill)
        {
            var index = skill.SkillID.Replace("Skill_" + role, "");

            switch (index)
            {
                case "One":
                    Skill_1Btn.interactable = true;
                    Skill_1Btn.onClick.AddListener(() => ShowDetailSkill(skill));
                    break;
                case "Two":
                    Skill_2Btn.interactable = true;
                    Skill_2Btn.onClick.AddListener(() => ShowDetailSkill(skill));
                    break;
                case "Three":
                    Skill_3Btn.interactable = true;
                    Skill_3Btn.onClick.AddListener(() => ShowDetailSkill(skill));
                    break;
            }
        }
    }

    public void LoadLayout()
    {
        string image = References.listEye.Find(obj => obj.ID == References.accountRefer.EyeID).Image;
        Eye.sprite = Resources.Load<Sprite>(image);

        //Hair
        image = References.listHair.Find(obj => obj.ID == References.accountRefer.HairID).Image;
        Hair.sprite = Resources.Load<Sprite>(image);

        //Mouth
        image = References.listMouth.Find(obj => obj.ID == References.accountRefer.MouthID).Image;
        Mouth.sprite = Resources.Load<Sprite>(image);

        //Weapon
        Weapon.sprite = Resources.Load<Sprite>(References.weapon.Image);

        //Skin
        image = References.listSkin.Find(obj => obj.ID == References.accountRefer.SkinID).Image;

        Shirt.sprite = Resources.Load<Sprite>(image + "_Shirt");
        LeftHand.sprite = Resources.Load<Sprite>(image + "_LeftHand");
        RightHand.sprite = Resources.Load<Sprite>(image + "_RightHand");
        LeftFoot.sprite = Resources.Load<Sprite>(image + "_LeftFoot");
        RightFoot.sprite = Resources.Load<Sprite>(image + "_RightFoot");
    }

    public void SetupPreviewEquipmentBtn()
    {
        var listUsing = References.listAccountEquipment.FindAll(obj => obj.IsUse);
        
        foreach (var equip in listUsing)
        {
            var type = References.listEquipment.Find(obj => obj.ID == equip.EquipmentID).TypeEquipmentID;

            switch(type)
            {
                case "Shirt":
                    Equip_ShirtBtn.interactable = true;
                    Equip_ShirtBtn.onClick.AddListener(() => ShowDetailEquipment(equip));
                    break;
                case "Pant":
                    Equip_PantBtn.interactable = true;
                    Equip_PantBtn.onClick.AddListener(() => ShowDetailEquipment(equip));
                    break;
                case "Weapon":
                    Equip_WeaponBtn.interactable = true;
                    Equip_WeaponBtn.onClick.AddListener(() => ShowDetailEquipment(equip));
                    break;
            }
        } 
    }
   
    public void ShowDetailPlayer()
    {
        //Name_Player.text = photonView.Owner.NickName;
        Name_Player.text = "Thien";
        Level_Player.text = References.accountRefer.Level.ToString();
        Health_Player.text = References.accountRefer.Health.ToString();
        Chakra_Player.text = References.accountRefer.Chakra.ToString();
        Strength_Player.text = References.accountRefer.Strength.ToString();
        Power_Player.text = References.accountRefer.Power.ToString();
        Exp_Player.text = References.accountRefer.Exp.ToString();
        Coin_Player.text = References.accountRefer.Coin.ToString();
    }

    public void ShowDetailSkill(AccountSkill_Entity skill)
    {
        //Name_Skill, Level_Skill, Damage_Skill, Chakra_Skill, Cooldown_Skill
        Init();
        DetailSkill.SetActive(true);

        Name_Skill.text = References.ListSkill.Find(obj => obj.ID == skill.SkillID).Name;
        Level_Skill.text = skill.Level.ToString();
        Damage_Skill.text = skill.Damage.ToString();
        Chakra_Skill.text = skill.Chakra.ToString();
        Cooldown_Skill.text = skill.Cooldown.ToString();
    }

    public void ShowDetailEquipment(AccountEquipment_Entity equip)
    {
        //Name_Equipment, Level_Equipment, Damage_Equipment, Health_Equipment, Chakra_Equipment;
        Init();
        DetailEquipment.SetActive(true);

        Name_Equipment.text = References.listEquipment.Find(obj => obj.ID == equip.EquipmentID).Name;
        Level_Equipment.text = equip.Level.ToString();
        Damage_Equipment.text = equip.Damage.ToString();
        Health_Equipment.text = equip.Health.ToString();
        Chakra_Equipment.text = equip.Chakra.ToString();
    }

    public void ShowDetailWeapon()
    {
        //Name_Weapon, Level_Weapon, Damage_Weapon;
        Init();
        DetailWeapon.SetActive(true);

        Name_Weapon.text = References.weapon.Name;
        Level_Weapon.text = References.accountWeapon.Level.ToString();
        Damage_Weapon.text = References.accountWeapon.Damage.ToString();
    }


}
