using Assets.Scripts.Database.DAO;
using Assets.Scripts.GameManager;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Creator : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Skin")]
    public int skinNr;
    public List<Skins> skins;
    public SpriteRenderer Shirt;
    public SpriteRenderer RightFoot;
    public SpriteRenderer LeftFoot;
    public SpriteRenderer RightHand;
    public SpriteRenderer LeftHand;

    [Header("Hair")]
    public int hairNr;
    public List<Layout> hairs;
    public SpriteRenderer Hair;

    [Header("Eye")]
    public int eyeNr;
    public List<Layout> eyes;
    public SpriteRenderer Eye;

    [Header("Mouth")]
    public int mouthNr;
    public List<Layout> mouths;
    public SpriteRenderer Mouth;

    [Header("Role")]
    public SpriteRenderer Weapon;
    public Button MeleeBtn, RangeBtn, SupportBtn;

    [Header("Button")]
    public Button HairBtn;
    public Button EyeBtn;
    public Button MouthBtn;
    public Button SkinBtn;
    public Button BackBtn;
    public Button LeftBtn, RightBtn;
    public Button CloseErrorBtn;
    public Button SaveBtn;
    public GameObject ChangeBtn;

    [Header("Name")]
    public TMP_InputField DisplayName;
    public TMP_Text MessageErr;
    public GameObject ErrorPanel;


    string layout = "Role";
    string IDRole = "Role_Melee";

    bool isDisconnect = false;

    private void Awake()
    {
        DisplayName.text = string.Empty;
        HairBtn.onClick.AddListener(OnHairBtnClick);
        EyeBtn.onClick.AddListener(OnEyeBtnClick);
        MouthBtn.onClick.AddListener(OnMouthBtnClick);
        SkinBtn.onClick.AddListener(OnSkinBtnClick);
        BackBtn.onClick.AddListener(OnBackBtnClick);
        LeftBtn.onClick.AddListener(Min);
        RightBtn.onClick.AddListener(Plus);

        SupportBtn.onClick.AddListener(OnSupportBtnClick);
        RangeBtn.onClick.AddListener(OnRangeBtnClick);
        MeleeBtn.onClick.AddListener(OnMeleeBtnClick);

        SaveBtn.onClick.AddListener(OnSaveBtnClick);
        CloseErrorBtn.onClick.AddListener(ClosePopupPanel);
    }

    public void ResetColorRole()
    {
        MeleeBtn.GetComponent<Image>().color = References.ButtonColorDefaul;
        RangeBtn.GetComponent<Image>().color = References.ButtonColorDefaul;
        SupportBtn.GetComponent<Image>().color = References.ButtonColorDefaul;
        ResetColorLayout();
        ChangeBtn.SetActive(false);
    }

    public void ResetColorLayout()
    {
        ChangeBtn.SetActive(true);
        HairBtn.GetComponent<Image>().color = References.ButtonColorSelected;
        EyeBtn.GetComponent<Image>().color = References.ButtonColorSelected;
        MouthBtn.GetComponent<Image>().color = References.ButtonColorSelected;
        SkinBtn.GetComponent<Image>().color = References.ButtonColorSelected;
    }
    public void OnMeleeBtnClick()
    {
        IDRole = "Role_Melee";
        layout = "Role";
        ResetColorRole();
        MeleeBtn.GetComponent<Image>().color = References.ButtonColorSelected;
    }

    public void OnRangeBtnClick()
    {
        IDRole = "Role_Range";
        layout = "Role";
        ResetColorRole();
        RangeBtn.GetComponent<Image>().color = References.ButtonColorSelected;
    }

    public void OnSupportBtnClick()
    {
        IDRole = "Role_Support";
        layout = "Role";
        ResetColorRole();
        SupportBtn.GetComponent<Image>().color = References.ButtonColorSelected;
    }

    public void OnSkinBtnClick()
    {
        layout = "Skin";
        ResetColorLayout();
        SkinBtn.GetComponent<Image>().color = Color.white;
    }

    public void OnHairBtnClick()
    {
        layout = "Hair";
        ResetColorLayout();
        HairBtn.GetComponent<Image>().color = Color.white;
    }

    public void OnEyeBtnClick()
    {
        layout = "Eye";
        ResetColorLayout();
        EyeBtn.GetComponent<Image>().color = Color.white;
    }

    public void OnMouthBtnClick()
    {
        layout = "Mouth";
        ResetColorLayout();
        MouthBtn.GetComponent<Image>().color = Color.white;
    }

    public void OnBackBtnClick()
    {
        if (References.accountRefer != null)
        {
            Account_DAO.ChangeStateOnline(References.accountRefer.ID, false);
        }
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(Scenes.Login);
    }

    public void OnSaveBtnClick()
    {
        if (UIManager.Instance.IsWiFiConnected())
        {
            var displayName = DisplayName.text;
            if (string.IsNullOrWhiteSpace(displayName))
            {
                OpenPopupPanel(Message.NameEmpty);
            }
            else if (displayName.Length < 4 || displayName.Length > 16
                || displayName.Split(new[] { '\r', '\n', '\t', ' ' }).Length > 1)
            {
                OpenPopupPanel(Message.NameInvalid);
            }
            else if (Account_DAO.IsDisplayNameExist(displayName))
            {
                OpenPopupPanel(Message.NameExist);
            }
            else
            {
                Account_DAO.SaveLayout(References.accountRefer.ID, displayName, IDRole,
                eyes[eyeNr].ID, hairs[hairNr].ID, mouths[mouthNr].ID, skins[skinNr].ID);
                References.accountRefer = Account_DAO.GetAccountByID(References.accountRefer.ID);
                PhotonNetwork.LoadLevel(Scenes.Konoha);
            }
        }
        else
        {
            OpenPopupPanel("Vui lòng kết nối mạng");
            isDisconnect = true;
        }
    }

    public void OpenPopupPanel(string message)
    {
        ErrorPanel.SetActive(true);
        MessageErr.text = message;
    }

    public void ClosePopupPanel()
    {
        if (isDisconnect) { isDisconnect = false; SceneManager.LoadScene(Scenes.Login); }
        else ErrorPanel.SetActive(false);

    }

    void LateUpdate()
    {
        LayoutChoice();
    }

    public void Start()
    {
        if (UIManager.Instance.IsWiFiConnected())
        {
            foreach (var item in References.listHair)
            {
                var obj = new Layout();
                obj.ID = item.ID;
                obj.Sprite = Resources.Load<Sprite>(item.Image);
                hairs.Add(obj);
            }

            foreach (var item in References.listEye)
            {
                var obj = new Layout();
                obj.ID = item.ID;
                obj.Sprite = Resources.Load<Sprite>(item.Image);
                eyes.Add(obj);
            }

            foreach (var item in References.listMouth)
            {
                var obj = new Layout();
                obj.ID = item.ID;
                obj.Sprite = Resources.Load<Sprite>(item.Image);
                mouths.Add(obj);
            }

            foreach (var item in References.listSkin)
            {
                var skin = new Skins();
                skin.ID = item.ID;
                skin.Shirt = Resources.Load<Sprite>(item.Image + "_Shirt");
                skin.LeftHand = Resources.Load<Sprite>(item.Image + "_LeftHand");
                skin.RightHand = Resources.Load<Sprite>(item.Image + "_RightHand");
                skin.LeftFoot = Resources.Load<Sprite>(item.Image + "_LeftFoot");
                skin.RightFoot = Resources.Load<Sprite>(item.Image + "_RightFoot");
                skins.Add(skin);
            }

            //init layout
            Hair.sprite = hairs[0].Sprite;
            Eye.sprite = eyes[0].Sprite;
            Mouth.sprite = mouths[0].Sprite;
            Shirt.sprite = skins[0].Shirt;
            LeftHand.sprite = skins[0].LeftHand;
            RightHand.sprite = skins[0].RightHand;
            LeftFoot.sprite = skins[0].LeftFoot;
            RightFoot.sprite = skins[0].RightFoot;
            var image = References.listRole.Find(obj => obj.ID == IDRole).Image;
            Weapon.sprite = Resources.Load<Sprite>(image);

            ChangeBtn.SetActive(false);

            ResetColorRole();
            MeleeBtn.GetComponent<Image>().color = References.ButtonColorSelected;
        }
        else
        {
            Debug.Log("Disconnect");
        }

    }

    void LayoutChoice()
    {
        switch (layout)
        {
            case "Hair":
                Hair.sprite = hairs[hairNr].Sprite;
                break;
            case "Eye":
                Eye.sprite = eyes[eyeNr].Sprite;
                break;
            case "Mouth":
                Mouth.sprite = mouths[mouthNr].Sprite;
                break;
            case "Skin":
                Shirt.sprite = skins[skinNr].Shirt;
                LeftHand.sprite = skins[skinNr].LeftHand;
                RightHand.sprite = skins[skinNr].RightHand;
                LeftFoot.sprite = skins[skinNr].LeftFoot;
                RightFoot.sprite = skins[skinNr].RightFoot;
                break;
            case "Role":
                var image = References.listRole.Find(obj => obj.ID == IDRole).Image;
                Weapon.sprite = Resources.Load<Sprite>(image);
                break;
        }
    }

    public void Plus()
    {
        switch (layout)
        {
            case "Hair":
                hairNr = (hairNr + 1) > hairs.Count - 1 ? 0 : ++hairNr;
                break;
            case "Eye":
                eyeNr = (eyeNr + 1) > eyes.Count - 1 ? 0 : ++eyeNr;
                break;
            case "Mouth":
                mouthNr = (mouthNr + 1) > mouths.Count - 1 ? 0 : ++mouthNr;
                break;
            case "Skin":
                skinNr = (skinNr + 1) > skins.Count - 1 ? 0 : ++skinNr;
                break;
        }
    }

    public void Min()
    {
        switch (layout)
        {
            case "Hair":
                hairNr = (hairNr - 1) < 0 ? hairs.Count - 1 : --hairNr;
                break;
            case "Eye":
                eyeNr = (eyeNr - 1) < 0 ? eyes.Count - 1 : --eyeNr;
                break;
            case "Mouth":
                mouthNr = (mouthNr - 1) < 0 ? mouths.Count - 1 : --mouthNr;
                break;
            case "Skin":
                skinNr = (skinNr - 1) < 0 ? skins.Count - 1 : --skinNr;
                break;
        }
    }

    private void OnApplicationQuit()
    {
        if (UIManager.Instance.IsWiFiConnected())
        {
            Account_DAO.ChangeStateOnline(References.accountRefer.ID, false);
        }
    }
}

[System.Serializable]
public struct Skins
{
    public string ID;
    public Sprite Shirt;
    public Sprite RightFoot;
    public Sprite LeftFoot;
    public Sprite RightHand;
    public Sprite LeftHand;
}

[System.Serializable]
public struct Layout
{
    public string ID;
    public Sprite Sprite;
}