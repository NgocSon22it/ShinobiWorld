using Assets.Scripts.Database.Entity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill_Item : MonoBehaviour
{
    [SerializeField] Image SkillImage;
    [SerializeField] TMP_Text CostTxt;
    [SerializeField] TMP_Text NameTxt;

    Skill_Entity skill_Entity;


    public void OnClick()
    {
        Skill_Manager.Instance.SetUpSelectedSkill(skill_Entity);

        Skill_Manager.Instance.ResetColor();
        GetComponent<Image>().color = References.ItemColorSelected;
    }

    public void SetUp(Skill_Entity skill_Entity, bool isFirst)
    {
        if (isFirst) GetComponent<Image>().color = References.ItemColorSelected;
        this.skill_Entity = skill_Entity;
        SkillImage.sprite = Resources.Load<Sprite>(skill_Entity.Image);
        CostTxt.text = skill_Entity.BuyCost.ToString();
        NameTxt.text = skill_Entity.Name.ToString();
        GetComponent<Image>().color = References.ItemColorDefaul;
    }



}
