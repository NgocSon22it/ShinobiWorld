using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testdamage : MonoBehaviour
{
    

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Health.instance.UseHealth(15);

        if(Input.GetKeyDown(KeyCode.A))
            Chakka.instance.UseChakka(15);
    }
}
