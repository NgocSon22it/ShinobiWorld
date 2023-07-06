using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Pool : MonoBehaviour
{

    [SerializeField] List<GameObject> List_NormalAttack = new List<GameObject>();
    [SerializeField] List<GameObject> List_NormalAttack_Hit = new List<GameObject>();

    [SerializeField] List<GameObject> List_SkillOne = new List<GameObject>();
    [SerializeField] List<GameObject> List_SkillOne_Hit = new List<GameObject>();

    [SerializeField] List<GameObject> List_SkillTwo = new List<GameObject>();
    [SerializeField] List<GameObject> List_SkillTwo_Hit = new List<GameObject>();

    [SerializeField] List<GameObject> List_SkillThree = new List<GameObject>();
    [SerializeField] List<GameObject> List_SkillThree_Hit = new List<GameObject>();


    public GameObject GetNormalAttackFromPool()
    {
        for (int i = 0; i < List_NormalAttack.Count; i++)
        {
            if (!List_NormalAttack[i].activeInHierarchy)
            {
                return List_NormalAttack[i];
            }
        }
        return null;
    }
    public GameObject GetNormalAttack_Hit_FromPool()
    {
        for (int i = 0; i < List_NormalAttack_Hit.Count; i++)
        {
            if (!List_NormalAttack_Hit[i].activeInHierarchy)
            {
                return List_NormalAttack_Hit[i];
            }
        }
        return null;
    }
    public GameObject GetSkillOneFromPool()
    {
        for (int i = 0; i < List_SkillOne.Count; i++)
        {
            if (!List_SkillOne[i].activeInHierarchy)
            {
                return List_SkillOne[i];
            }
        }
        return null;
    }
    public GameObject GetSkillOne_Hit_FromPool()
    {
        for (int i = 0; i < List_SkillOne_Hit.Count; i++)
        {
            if (!List_SkillOne_Hit[i].activeInHierarchy)
            {
                return List_SkillOne_Hit[i];
            }
        }
        return null;
    }
    public GameObject GetSkillTwoFromPool()
    {
        for (int i = 0; i < List_SkillTwo.Count; i++)
        {
            if (!List_SkillTwo[i].activeInHierarchy)
            {
                return List_SkillTwo[i];
            }
        }
        return null;
    }
    public GameObject GetSkillTwo_Hit_FromPool()
    {
        for (int i = 0; i < List_SkillTwo_Hit.Count; i++)
        {
            if (!List_SkillTwo_Hit[i].activeInHierarchy)
            {
                return List_SkillTwo_Hit[i];
            }
        }
        return null;
    }
    public GameObject GetSkillThreeFromPool()
    {
        for (int i = 0; i < List_SkillThree.Count; i++)
        {
            if (!List_SkillThree[i].activeInHierarchy)
            {
                return List_SkillThree[i];
            }
        }
        return null;
    }
    public GameObject GetSkillThree_Hit_FromPool()
    {
        for (int i = 0; i < List_SkillThree_Hit.Count; i++)
        {
            if (!List_SkillThree_Hit[i].activeInHierarchy)
            {
                return List_SkillThree_Hit[i];
            }
        }
        return null;
    }

}
