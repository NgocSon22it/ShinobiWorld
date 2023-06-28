using Assets.Scripts.Database.Entity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill_Item : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Image SkillImage;
    [SerializeField] GameObject SelectedObject;
    [SerializeField] TMP_Text CostTxt;
    [SerializeField] TMP_Text NameTxt;

    Skill_Entity skill_Entity;

    public void OnPointerDown(PointerEventData eventData)
    {
        Skill_Manager.Instance.SetUpSelectedSkill(skill_Entity);
    }

    public void SetUp(Skill_Entity skill_Entity)
    {
        this.skill_Entity = skill_Entity;
        SkillImage.sprite = Resources.Load<Sprite>(skill_Entity.Image);
        CostTxt.text = skill_Entity.BuyCost.ToString();
        NameTxt.text = skill_Entity.Name.ToString();
        SetUpSelected();
    }

    public void SetUpSelected()
    {
        if (Skill_Manager.Instance.SkillSelected.ID.Equals(skill_Entity.ID))
        {
            SelectedObject.SetActive(true);
        }
        else
        {
            SelectedObject.SetActive(false);
        }
    }



}
