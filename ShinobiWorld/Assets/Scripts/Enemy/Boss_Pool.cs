using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Pool : MonoBehaviourPunCallbacks
{
    [Header("Projectile Amount")]
    int Amount = 5;

    [SerializeField] GameObject SkillOne;
    List<GameObject> List_SkillOne = new List<GameObject>();

    [SerializeField] GameObject SkillTwo;
    List<GameObject> List_SkillTwo = new List<GameObject>();

    [SerializeField] GameObject SkillThree;
    List<GameObject> List_SkillThree = new List<GameObject>();


    public void InitializeProjectilePool(string Extension)
    {
        GameObject obj;

        if (SkillOne != null)
        {
            for (int i = 0; i < Amount; i++)
            {
                obj = PhotonNetwork.Instantiate(Extension + SkillOne.name, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                List_SkillOne.Add(obj);
            }
        }
        if (SkillTwo != null)
        {
            for (int i = 0; i < Amount; i++)
            {
                obj = PhotonNetwork.Instantiate(Extension + SkillTwo.name, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                List_SkillTwo.Add(obj);
            }
        }
        if (SkillThree != null)
        {
            for (int i = 0; i < Amount; i++)
            {
                obj = PhotonNetwork.Instantiate(Extension + SkillThree.name, Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                List_SkillThree.Add(obj);
            }
        }
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
}
