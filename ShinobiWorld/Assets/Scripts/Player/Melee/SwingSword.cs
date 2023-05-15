using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class SwingSword : MonoBehaviour
{
    [SerializeField] List<string> ListTag = new List<string>();

    public Transform Center;

    float posX, posY, angle = 1.5f;
    public float rotationRadius = 2f;
    public float angularSpeed = 2f;
    private void OnEnable()
    {
        angle = 1.5f;
        Invoke(nameof(TurnOff), 5f);
    }

    private void Update()
    {
        if(Center != null)
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
