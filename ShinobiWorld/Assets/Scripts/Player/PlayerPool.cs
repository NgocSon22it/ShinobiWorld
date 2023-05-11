using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPool : MonoBehaviour
{
    [Header("Projectile Amount")]
    int Amount = 20;

    [SerializeField] GameObject NormalAttack;
    List<GameObject> List_NormalAttack = new List<GameObject>();

    private void Start()
    {
        GameObject obj;

        for (int i = 0; i < Amount; i++)
        {
            obj = Instantiate(NormalAttack);
            obj.SetActive(false);
            List_NormalAttack.Add(obj);
        }
    }

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

}
