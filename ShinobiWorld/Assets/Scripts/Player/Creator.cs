using System.Collections;
using System.Collections.Generic;
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
    public Skins[] skins;
    public SpriteRenderer Shirt;
    public SpriteRenderer RightFoot;
    public SpriteRenderer LeftFoot;
    public SpriteRenderer RightHand;
    public SpriteRenderer LeftHand;

    [Header("Hair")]
    public int hairNr;
    public Layout[] hairs;
    public SpriteRenderer Hair;

    [Header("Eye")]
    public int eyeNr;
    public Layout[] eyes;
    public SpriteRenderer Eye;

    [Header("Mouth")]
    public int mouthNr;
    public Layout[] mouths;
    public SpriteRenderer Mouth;

    [Header("Role")]
    public int roleNr;
    public Weapon weapon;
    public string[] roles;

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

    void LateUpdate()
    {
        LayoutChoice();
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
                weapon.Clear();
                weapon.SetActive(roleNr, roles[roleNr]);
                break;
        }
    }

    public void Plus()
    {
        switch (layout)
        {
            case "Hair":
                hairNr = (hairNr + 1) > hairs.Length - 1 ? 0 : ++hairNr;
                break;
            case "Eye":
                eyeNr = (eyeNr + 1) > eyes.Length - 1 ? 0 : ++eyeNr;
                break;
            case "Mouth":
                mouthNr = (mouthNr + 1) > mouths.Length - 1 ? 0 : ++mouthNr;
                break;
            case "Skin":
                skinNr = (skinNr + 1) > skins.Length -1 ? 0 : ++skinNr;
                break;
            case "Role":
                roleNr = (roleNr + 1) > roles.Length - 1 ? 0 : ++roleNr;
                break;
        }
    }

    public void Min()
    {
        switch (layout)
        {
            case "Hair":
                hairNr = (hairNr - 1) < 0 ? hairs.Length - 1 : --hairNr;
                break;
            case "Eye":
                eyeNr = (eyeNr - 1) < 0 ? eyes.Length - 1 : --eyeNr;
                break;
            case "Mouth":
                mouthNr = (mouthNr - 1) < 0 ? mouths.Length - 1 : --mouthNr;
                break;
            case "Skin":
                skinNr = (skinNr - 1) < 0 ? skins.Length - 1 : --skinNr;
                break;
            case "Role":
                roleNr = (roleNr - 1) < 0 ? roles.Length - 1 : --roleNr;
                break;
        }
    }
}

[System.Serializable]
public struct Skins
{
    public Sprite Shirt;
    public Sprite RightFoot;
    public Sprite LeftFoot;
    public Sprite RightHand;
    public Sprite LeftHand;
}

[System.Serializable]
public struct Layout
{
    public Sprite Sprite;
}

[System.Serializable]
public struct Weapon
{
    //0 Support
    public GameObject GloveRight;
    public GameObject GloveLeft;
    //1 Range
    public GameObject Dart;
    //2 Melee
    public GameObject Sword;

    public TMP_Text Name;

    public void Clear()
    {
        GloveRight.SetActive(false);
        GloveLeft.SetActive(false);
        Dart.SetActive(false); 
        Sword.SetActive(false);
    }

    public void SetActive(int index, string name)
    {
        Name.text = name;
        switch (index)
        {
            case 0:
                GloveRight.SetActive(true);
                GloveLeft.SetActive(true);
                break;
            case 1:
                Dart.SetActive(true);
                break;
            case 2:
                Sword.SetActive(true);
                break;
        }
    }
}