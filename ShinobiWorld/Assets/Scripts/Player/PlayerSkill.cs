using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    protected List<string> AttackAble_Tag = new List<string>()
    {
        "Enemy", "Ground", "Clone"
    };

    [SerializeField] protected PhotonView PV;

    protected int Damage;

    protected float LifeTime;

    [SerializeField] protected Player_Pool player_Pool;
    protected GameObject HitEffect;

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
