using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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
    public Button WeaponBtn;

    [Header("PreviewSkill")]
    public Button Skill_1Btn;
    public Button Skill_2Btn;
    public Button Skill_3Btn;
    public Image Skill_1Image, Skill_2Image, Skill_3Image;

    [Header("PreviewEquipment")]
    public Button Equip_ShirtBtn;
    public Button Equip_HeadbandBtn;
    public Button Equip_WeaponBtn;
    public Image Equip_ShirtImage, Equip_HeadbandImage, Equip_WeaponImage;
    public TMP_Text Equip_ShirtTxt, Equip_HeadbandTxt, Equip_WeaponTxt;

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

    public Camera mainCamera; // Reference to the main camera
    public Canvas canvas;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = mainCamera;
    }

    public void Init()
    {
        DetailPlayer.SetActive(false);
        DetailWeapon.SetActive(false);
        DetailSkill.SetActive(false); 
        DetailEquipment.SetActive(false);
    }

    public void Open()
    {
        Game_Manager.Instance.IsBusy = true;
        Game_Manager.Instance.PlayerManager.GetComponent<PlayerBase>().PlayerAllUIInstance.SetActive(false) ;

        //References.accountRefer = Account_DAO.GetAccountByID("vRsLqEXrnhMpK48YRLlYMNBElTf1");

        References.listBagEquipment = BagEquipment_DAO.GetAllByUserID(References.accountRefer.ID);
        References.listHasSkill = HasSkill_DAO.GetAllSkillForAccount(References.accountRefer.ID);
        References.hasWeapon = HasWeapon_DAO.GetHasWeaponByID(References.accountRefer.ID);
        References.weapon = Weapon_DAO.GetWeaponByID(References.hasWeapon.WeaponID);

        InfoPanel.SetActive(true);
        
        Preview.SetActive(true);
        SetupPreview();

        Init();

        DetailPlayer.SetActive(true);
        ShowDetailPlayer();
    }

    public void Close()
    {
        InfoPanel.SetActive(false);
        Game_Manager.Instance.IsBusy = false;
        Game_Manager.Instance.PlayerManager.GetComponent<PlayerBase>().PlayerAllUIInstance.SetActive(true);

    }

    public void SetupPreview()
    {
        Name_Preview.text = PhotonNetwork.NickName;
        //Name_Preview.text = "Thien";
        Role.text = References.listRole.Find(obj => obj.ID == References.accountRefer.RoleInGameID).Name;
        Trophy.text = References.listTrophy.Find(obj => obj.ID == References.accountRefer.TrophyID).Name;

        LoadLayout();
        SetupPreviewBtn();
        SetupPreviewSkillBtn();
        SetupPreviewEquipmentBtn();

        //WeaponBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.weapon.Image +"Btn");
    }

    public void SetupPreviewBtn()
    {
        SetupPreviewBtnImage();
        Skill_1Btn.interactable = false;
        Skill_1Image.color = new Color32(255,255,255,100);

        Skill_2Btn.interactable = false;
        Skill_2Image.color = new Color32(255, 255, 255, 100);

        Skill_3Btn.interactable = false;
        Skill_3Image.color = new Color32(255, 255, 255, 100);

        Equip_ShirtBtn.interactable = false;
        Equip_ShirtTxt.gameObject.SetActive(true);
        Equip_ShirtImage.gameObject.SetActive(false);

        Equip_HeadbandBtn.interactable = false;
        Equip_HeadbandTxt.gameObject.SetActive(true);
        Equip_HeadbandImage.gameObject.SetActive(false);

        Equip_WeaponBtn.interactable = false;
        Equip_WeaponTxt.gameObject.SetActive(true);
        Equip_WeaponImage.gameObject.SetActive(false);
    }

    public void SetupPreviewBtnImage()
    {
        WeaponBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UIEquipmentShow);
        Skill_1Btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UISkillDefault);
        Skill_2Btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UISkillDefault);
        Skill_3Btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UISkillDefault);

        if (Equip_HeadbandImage.gameObject.activeSelf)
        {
            Equip_HeadbandBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UIEquipmentShow);
        }
        else Equip_HeadbandBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UIEquipmentDefault);

        if(Equip_ShirtImage.gameObject.activeSelf)
        {
            Equip_ShirtBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UIEquipmentShow);
        } else Equip_ShirtBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UIEquipmentDefault);

        if (Equip_WeaponImage.gameObject.activeSelf)
        {
            Equip_WeaponBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UIEquipmentShow);
        }
        else Equip_WeaponBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UIEquipmentDefault);
    }

    public void SetupPreviewBtnSelected(Button btn)
    {
        SetupPreviewBtnImage();
        btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UIInfoSelected);
    }

    public void SetupPreviewSkillBtn()
    {
        var role = References.accountRefer.RoleInGameID.Replace("Role_", "");
        var list = References.listSkill.FindAll(obj => obj.RoleInGameID == References.accountRefer.RoleInGameID);

        foreach (var skill in list)
        {
            var index = skill.ID.Replace("Skill_" + role, "");
            switch (index)
            {
                case "One":
                    Skill_1Image.sprite = Resources.Load<Sprite>(skill.Image);
                    break;
                case "Two":
                    Skill_2Image.sprite = Resources.Load<Sprite>(skill.Image);
                    break;
                case "Three":
                    Skill_3Image.sprite = Resources.Load<Sprite>(skill.Image);
                    break;
            }
        }

        foreach (var skill in References.listHasSkill)
        {
            var index = skill.SkillID.Replace("Skill_" + role, "");

            switch (index)
            {
                case "One":
                    Skill_1Btn.interactable = true;
                    Skill_1Image.color = new Color32(255, 255, 255, 255);
                    Skill_1Btn.onClick.AddListener
                    (() =>
                    {
                        ShowDetailSkill(skill);
                        SetupPreviewBtnSelected(Skill_1Btn);
                    });
                    break;
                case "Two":
                    Skill_2Btn.interactable = true;
                    Skill_2Image.color = new Color32(255, 255, 255, 255);
                    Skill_2Btn.onClick.AddListener(() =>
                    {
                        ShowDetailSkill(skill);
                        SetupPreviewBtnSelected(Skill_2Btn);
                    });
                    break;
                case "Three":
                    Skill_3Btn.interactable = true;
                    Skill_3Image.color = new Color32(255, 255, 255, 255);
                    Skill_3Btn.onClick.AddListener(() =>
                    {
                        ShowDetailSkill(skill);
                        SetupPreviewBtnSelected(Skill_3Btn);
                    });
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
        var listUsing = References.listBagEquipment.FindAll(obj => obj.IsUse);
        
        foreach (var equip in listUsing)
        {
            var equipment = References.listEquipment.Find(obj => obj.ID == equip.EquipmentID);

            switch(equipment.TypeEquipmentID)
            {
                case "Headband":
                    Equip_HeadbandBtn.interactable = true;
                    Equip_HeadbandBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UIEquipmentShow);
                    Equip_HeadbandImage.gameObject.SetActive(true); 
                    Equip_HeadbandImage.sprite = Resources.Load<Sprite>(equipment.Image);
                    Equip_HeadbandTxt.gameObject.SetActive(false);

                    Equip_HeadbandBtn.onClick.AddListener(() =>
                    {
                        ShowDetailEquipment(equip);
                        SetupPreviewBtnSelected(Equip_HeadbandBtn);
                    });
                    break;
                case "Shirt":
                    Equip_ShirtBtn.interactable = true;
                    Equip_ShirtBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UIEquipmentShow);
                    Equip_ShirtImage.gameObject.SetActive(true);
                    Equip_ShirtImage.sprite = Resources.Load<Sprite>(equipment.Image);
                    Equip_ShirtTxt.gameObject.SetActive(false);

                    Equip_ShirtBtn.onClick.AddListener(() =>
                    {
                        ShowDetailEquipment(equip);
                        SetupPreviewBtnSelected(Equip_ShirtBtn);
                    });
                    break;
                case "Weapon":
                    Equip_WeaponBtn.interactable = true;
                    Equip_WeaponBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UIEquipmentShow);
                    Equip_WeaponImage.gameObject.SetActive(true);
                    Equip_WeaponImage.sprite = Resources.Load<Sprite>(equipment.Image);
                    Equip_WeaponTxt.gameObject.SetActive(false);

                    Equip_WeaponBtn.onClick.AddListener(() => 
                        { 
                            ShowDetailEquipment(equip);
                            SetupPreviewBtnSelected(Equip_WeaponBtn);
                        } );
                    break;
            }
        } 
    }
   
    public void ShowDetailPlayer()
    {
        Init();

        DetailPlayer.SetActive(true);

        Name_Player.text = PhotonNetwork.NickName;
        //Name_Player.text = "Thien";
        Level_Player.text = References.accountRefer.Level.ToString();
        Health_Player.text = References.accountRefer.Health.ToString();
        Chakra_Player.text = References.accountRefer.Chakra.ToString();
        Strength_Player.text = References.accountRefer.Strength.ToString();
        Power_Player.text = References.accountRefer.Power.ToString();
        Exp_Player.text = References.accountRefer.Exp.ToString();
        Coin_Player.text = References.accountRefer.Coin.ToString();
    }

    public void ShowDetailSkill(HasSkill_Entity skill)
    {
        //Name_Skill, Level_Skill, Damage_Skill, Chakra_Skill, Cooldown_Skill
        Init();
        
        DetailSkill.SetActive(true);

        Name_Skill.text = References.listSkill.Find(obj => obj.ID == skill.SkillID).Name;
        Level_Skill.text = skill.Level.ToString();
        Damage_Skill.text = skill.Damage.ToString();
        Chakra_Skill.text = skill.Chakra.ToString();
        Cooldown_Skill.text = skill.Cooldown.ToString();
    }

    public void ShowDetailEquipment(BagEquipment_Entity equip)
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
        SetupPreviewBtnImage();
        WeaponBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(References.UIInfoSelected);

        DetailWeapon.SetActive(true);

        Name_Weapon.text = References.weapon.Name;
        Level_Weapon.text = References.hasWeapon.Level.ToString();
        Damage_Weapon.text = References.hasWeapon.Damage.ToString();
    }


}
