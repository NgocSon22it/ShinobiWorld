using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class Melee_SkillThree : PlayerSkill
{
    public Vector3  EndPoint;

    public GameObject Explosion;

    bool Reach;
    public void SetUpPoint(Vector3 EndPoint, GameObject Explosion)
    {
        this.EndPoint = EndPoint;
        this.Explosion = Explosion;
    }

    private void Update()
    {
        if (EndPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, EndPoint, 8 * Time.deltaTime);
        }

        if(transform.position == EndPoint && !Reach)
        {
            Reach = true;
            Explosion.transform.position = EndPoint;
            TurnOff();
            Explosion.SetActive(true);
        }
    }

    new void OnEnable()
    {
        LifeTime = 5f;
        Reach = false;
        base.OnEnable();
    }

    new void OnDisable()
    {
        base.OnDisable();
    }    
}
