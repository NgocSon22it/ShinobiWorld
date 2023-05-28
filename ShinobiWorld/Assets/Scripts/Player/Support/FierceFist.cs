using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FierceFist : MonoBehaviour
{
    [SerializeField] List<string> ListTag = new List<string>();

    string UserID;
    int Damage;

    public void SetUp(string UserID, int Damage)
    {
        this.UserID = UserID;
        this.Damage = Damage;
    }

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
            if (collision.gameObject.tag == "Enemy")
            {
                collision.GetComponent<Enemy>().TakeDamage(UserID, Damage);
            }
            TurnOff();
        }
    }
}
