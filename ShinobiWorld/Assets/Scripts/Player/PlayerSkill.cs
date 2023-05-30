using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] protected List<string> AttackAble_Tag = new List<string>();

    protected string UserID;
    protected int Damage;

    protected float LifeTime;

    public void SetUp(string UserID, int Damage)
    {
        this.UserID = UserID;
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
