using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Pool : MonoBehaviour
{
    [Header("Projectile Amount")]
    int Amount = 10;

    [SerializeField] List<GameObject> List_NormalAttack = new List<GameObject>();

    [SerializeField] List<GameObject> List_SkillOne = new List<GameObject>();

    [SerializeField] List<GameObject> List_SkillTwo = new List<GameObject>();

    [SerializeField] List<GameObject> List_SkillThree = new List<GameObject>();

    [SerializeField] List<GameObject> List_SkillThreeExplosion = new List<GameObject>();

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

    public GameObject GetSkillThreeExplosionFromPool()
    {
        for (int i = 0; i < List_SkillThreeExplosion.Count; i++)
        {
            if (!List_SkillThreeExplosion[i].activeInHierarchy)
            {
                return List_SkillThreeExplosion[i];
            }
        }
        return null;
    }

}
