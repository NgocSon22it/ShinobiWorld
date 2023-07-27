using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician_Attack : Boss_Skill
{
    [SerializeField] GameObject MainFire;
    [SerializeField] Magician magician;
    GameObject AttackHit;

    public Vector3 EndPoint;

    public void SetUpPoint(Vector3 EndPoint)
    {
        this.EndPoint = EndPoint;
        MainFire.transform.position = EndPoint + new Vector3(0, 8, 0);
    }

    private void Update()
    {
        if (EndPoint != null)
        {
            MainFire.transform.position = Vector3.MoveTowards(MainFire.transform.position, EndPoint, 5 * Time.deltaTime);
        }

        if (MainFire.transform.position == EndPoint)
        {
            TurnOff();
            AttackHit = magician.GetAttack_Hit();
            if (AttackHit != null)
            {
                AttackHit.transform.position = EndPoint;
                AttackHit.GetComponent<Magician_Attack_Hit>().SetUp(Damage);
                AttackHit.SetActive(true);
            }
        }
    }
    new void OnEnable()
    {
        LifeTime = 5f;
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
    }
}
