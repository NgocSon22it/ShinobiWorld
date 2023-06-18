using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Pool : MonoBehaviourPunCallbacks
{
    [Header("Projectile Amount")]
    int Amount = 5;

    [SerializeField] List<GameObject> List_SkillOne = new List<GameObject>();

    [SerializeField] List<GameObject> List_SkillTwo = new List<GameObject>();

    [SerializeField] List<GameObject> List_SkillThree = new List<GameObject>();

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
}
