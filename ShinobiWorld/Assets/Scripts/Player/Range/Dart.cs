using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField] List<string> ListTag = new List<string>();

    private void OnEnable()
    {
        Invoke(nameof(TurnOff), 5f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void TurnOff()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ListTag.Contains(collision.gameObject.tag))
        {
            TurnOff();
        }
    }
}