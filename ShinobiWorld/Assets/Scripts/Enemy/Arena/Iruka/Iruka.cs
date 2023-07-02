using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iruka : Enemy
{

    Coroutine AttackCoroutine;
    bool IsStartCoroutine;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        AttackAndMove();

    }

    public void AttackAndMove()
    {

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, MovePosition, 3f * Time.deltaTime);

            if (transform.position == MovePosition)
            {
                isMoving = false;
            }

        }
        else
        {
            if (!IsStartCoroutine)
            {
                AttackCoroutine = StartCoroutine(RandomAttack());
            }
        }

        animator.SetBool("Walk", isMoving);
    }


    public IEnumerator RandomAttack()
    {
        IsStartCoroutine = true;

        yield return new WaitForSeconds(3f);

        MovePosition = GetRandomPosition();
        isMoving = true;
        IsStartCoroutine = false;
    }
}
