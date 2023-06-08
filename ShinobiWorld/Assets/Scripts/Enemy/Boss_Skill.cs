using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Skill : MonoBehaviour
{
    [SerializeField] protected List<string> AttackAble_Tag = new List<string>();

    protected int Damage;

    protected float LifeTime;
 

    public void SetUp(int Damage)
    {
        this.Damage = Damage;
    }

    public void OnEnable()
    {
        Invoke(nameof(TurnOff), LifeTime);
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        CancelInvoke();
    }
}
