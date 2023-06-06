using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crap : Enemy
{


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (photonView.IsMine)
        {
            
            boss_Entity.ID = "Boss_Bat";
            boss_Entity = Boss_DAO.GetBossByID(boss_Entity.ID);
            CurrentHealth = boss_Entity.Health;
            MovePosition = GetRandomPosition();
        }

        LoadHealthUI();
    }

    new void Update()
    {
        if (photonView.IsMine)
        {
            Move();
        }
    }
    public void Move()
    {

            if (isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, MovePosition, 3f * Time.deltaTime);

                if (transform.position == MovePosition)
                {
                    Break_CurrentTime = Break_TotalTime;
                    isMoving = false;
                }

            }
            else
            {
                Break_CurrentTime -= Time.deltaTime;

                if (Break_CurrentTime <= 0f)
                {
                    MovePosition = GetRandomPosition();
                    isMoving = true;
                }
            }

        animator.SetBool("Walk", isMoving);
    }
}
