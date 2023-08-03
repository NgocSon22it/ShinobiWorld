using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PK_ReadyBase : MonoBehaviour
{
    bool IsReady;

    public bool GetReady()
    {
        return IsReady;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IsReady = true;
            PK_Manager.Instance.CheckAllPlayerReady();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IsReady = false;
            PK_Manager.Instance.CheckAllPlayerReady();
        }
    }

}
