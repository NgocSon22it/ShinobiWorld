using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena_ReadyBase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BossArena_Manager.Instance.CurrentNumber++;
            BossArena_Manager.Instance.CheckAllPlayerReady();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BossArena_Manager.Instance.CurrentNumber--;
            BossArena_Manager.Instance.CheckAllPlayerReady();

        }
    }
}
