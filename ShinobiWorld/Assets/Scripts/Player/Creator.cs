using Assets.Scripts.Database.DAO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Reporting;
using UnityEngine;
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
    public int roleNr;
    public List<Role> roles;
    public SpriteRenderer Weapon;
    public TMP_Text Name;

    string layout = "Role";

    public void OnRoleBtnClick()
    {
        layout = "Role";
    }

    public void OnSkinBtnClick()
    {
        layout = "Skin";
    }

    public void OnHairBtnClick()
    {
        layout = "Hair";
    }

    public void OnEyeBtnClick()
    {
        layout = "Eye";
    }

    public void OnMouthBtnClick()
    {
        layout = "Mouth";
    }

    public void OnSaveBtnClick()
    {
        Account_DAO.SaveLayout(References.UserID, roles[roleNr].ID, 
            eyes[eyeNr].ID, hairs[hairNr].ID, mouths[mouthNr].ID, skins[skinNr].ID);
    }

    void LateUpdate()
    {
        LayoutChoice();
    }

    public void Start()
    {
        foreach (var item in Hair_DAO.GetAll())
        {
            var obj = new Layout();
            obj.ID = item.ID;
            obj.Sprite = Resources.Load<Sprite>(item.Image);
            hairs.Add(obj);
        }
       
        foreach (var item in Eye_DAO.GetAll())
        {
            var obj = new Layout();
            obj.ID = item.ID;
            obj.Sprite = Resources.Load<Sprite>(item.Image);
            eyes.Add(obj);
        }
             
        foreach (var item in Mouth_DAO.GetAll())
        {
            var obj = new Layout();
            obj.ID = item.ID;
            obj.Sprite = Resources.Load<Sprite>(item.Image);
            mouths.Add(obj);
        }

        foreach (var item in Skin_DAO.GetAll())
        {
            var skin = new Skins();
            skin.ID = item.ID;
            skin.Shirt      = Resources.Load<Sprite>(item.Image+ "_Shirt");
            skin.LeftHand   = Resources.Load<Sprite>(item.Image+ "_LeftHand");
            skin.RightHand  = Resources.Load<Sprite>(item.Image+ "_RightHand");
            skin.LeftFoot   = Resources.Load<Sprite>(item.Image+ "_LeftFoot");
            skin.RightFoot  = Resources.Load<Sprite>(item.Image+ "_RightFoot");
            skins.Add(skin);
        }

        foreach (var item in Mouth_DAO.GetAll())
        {
            var obj = new Layout();
            obj.ID = item.ID;
            obj.Sprite = Resources.Load<Sprite>(item.Image);
            mouths.Add(obj);
        }

        foreach (var item in RoleInGame_DAO.GetAll())
        {
            var role = new Role();
            role.ID = item.ID;
            role.Weapon = Resources.Load<Sprite>(item.Image);
            role.Name = item.Name;
            roles.Add(role);
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
                Weapon.sprite = roles[roleNr].Weapon;
                Name.text = roles[roleNr].Name;
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
            case "Role":
                roleNr = (roleNr + 1) > roles.Count - 1 ? 0 : ++roleNr;
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
            case "Role":
                roleNr = (roleNr - 1) < 0 ? roles.Count - 1 : --roleNr;
                break;
        }
    }
}

[System.Serializable]
public struct Skins
{
    public int ID;
    public Sprite Shirt;
    public Sprite RightFoot;
    public Sprite LeftFoot;
    public Sprite RightHand;
    public Sprite LeftHand;
}

[System.Serializable]
public struct Layout
{
    public int ID;
    public Sprite Sprite;
}

[System.Serializable]
public struct Role
{
    public int ID;
    public Sprite Weapon;
    public string Name;
}