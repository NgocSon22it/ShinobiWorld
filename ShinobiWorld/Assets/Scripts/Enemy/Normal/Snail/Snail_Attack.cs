using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail_Attack : Boss_Skill
{
    [SerializeField] GameObject Main;
    [SerializeField] Snail snail;
    GameObject AttackHit;

    Vector3 TargetPosition;
    public void SetUpPoint(Vector3 BatPosition, Vector3 TargetPosision)
    {
        Main.transform.position = BatPosition;
        this.TargetPosition = TargetPosision;
    }

    private void Update()
    {
        if (TargetPosition != null)
        {
            Main.transform.position = Vector3.MoveTowards(Main.transform.position, TargetPosition, 6 * Time.deltaTime);
        }

        if (Main.transform.position == transform.position)
        {
            TurnOff();
            AttackHit = snail.GetAttack_Hit();
            if (AttackHit != null)
            {
                AttackHit.transform.position = transform.position;
                AttackHit.GetComponent<Snail_Attack_Hit>().SetUp(Damage);
                AttackHit.SetActive(true);
            }
        }
    }

    new void OnEnable()
    {
        LifeTime = 3f;
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
    }
}
