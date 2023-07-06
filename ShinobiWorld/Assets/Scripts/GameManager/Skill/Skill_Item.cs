using Assets.Scripts.Database.Entity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill_Item : MonoBehaviour
    //, IPointerDownHandler
{
    [SerializeField] Image SkillImage;
    public Image Background;
    [SerializeField] TMP_Text CostTxt;
    [SerializeField] TMP_Text NameTxt;

    Skill_Entity skill_Entity;

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    Skill_Manager.Instance.SetUpSelectedSkill(skill_Entity);
    //    Background.color = new Color32(190, 140, 10, 255);
    //}
    public void OnClick()
    {
        Skill_Manager.Instance.SetUpSelectedSkill(skill_Entity);

        Skill_Manager.Instance.ResetColor();
        Background.color = new Color32(190, 140, 10, 255);
    }

    public void SetUp(Skill_Entity skill_Entity)
    {
        this.skill_Entity = skill_Entity;
        SkillImage.sprite = Resources.Load<Sprite>(skill_Entity.Image);
        CostTxt.text = skill_Entity.BuyCost.ToString();
        NameTxt.text = skill_Entity.Name.ToString();
        Background.color = new Color32(110, 80, 60, 255);
    }



}
