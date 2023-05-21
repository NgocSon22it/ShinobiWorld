using Assets.Scripts.Database.Entity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class SwingSword : MonoBehaviour
{
    [SerializeField] List<string> ListTag = new List<string>();

    public Transform Center;
    Collider2D collider2;

    float posX, posY, angle = 1.5f;
    public float rotationRadius = 2f;
    public float angularSpeed = 2f;

    string UserID;
    AccountWeapon_Entity accountWeapon_Entity;

    float DamageSeconds = 0.2f;

    private void Awake()
    {
        collider2 = GetComponent<Collider2D>();
    }

    public void SetUpSwingSword(string UserID, AccountWeapon_Entity accountWeapon_Entity)
    {
        this.UserID = UserID;
        this.accountWeapon_Entity = accountWeapon_Entity;
    }

    private void OnEnable()
    {
        angle = 1.5f;
        Invoke(nameof(TurnOff), 5f);
        StartCoroutine(LogTriggeredObjects());
    }

    private void Update()
    {
        if (Center != null)
        {
            posX = Center.position.x + Mathf.Cos(angle) * rotationRadius;
            posY = Center.position.y + Mathf.Sin(angle) * rotationRadius;
            transform.position = new Vector2(posX, posY);

            angle = angle + Time.deltaTime * angularSpeed;

            if (angle >= 360f)
            {
                angle = 1.5f;
            }
        }
    }


    public void SetUpCenter(Transform transform)
    {
        Center = transform;
    }
    private void OnDisable()
    {
        CancelInvoke();
        StopCoroutine(LogTriggeredObjects());

    }

    void TurnOff()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator LogTriggeredObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(DamageSeconds);
            List<Collider2D> colliders = new List<Collider2D>();
            Physics2D.OverlapCollider(collider2, new ContactFilter2D(), colliders);

            foreach (Collider2D collider in colliders)
            {
                if (ListTag.Contains(collider.gameObject.tag))
                {
                    if (collider.gameObject.tag == "Enemy")
                    {
                        collider.GetComponent<Enemy>().TakeDamage(UserID, accountWeapon_Entity.Damage);
                    }
                }
            }
        }
    }
}
