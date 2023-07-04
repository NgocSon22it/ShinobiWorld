using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    protected List<string> AttackAble_Tag = new List<string>()
    {
        "Enemy", "Ground"
    };

    protected string UserID;
    protected int Damage;

    protected float LifeTime;

    [SerializeField] protected Player_Pool player_Pool;
    protected GameObject HitEffect;

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
