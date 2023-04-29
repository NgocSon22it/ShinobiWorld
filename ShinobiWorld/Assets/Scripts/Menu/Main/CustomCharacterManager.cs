using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomCharacterManager : MonoBehaviour
{
    public void ToggleValueChanged(Toggle Toggle)
    {
        if (Toggle.isOn)
        {
            switch (Toggle.gameObject.name)
            {
                case "Melee":
                    Debug.Log("Player selected Melee");
                    break;
                case "Range":
                    Debug.Log("Player selected Range");
                    break;
                case "Support":
                    Debug.Log("Player selected Support");
                    break;
                default:
                    Debug.Log("Somethings Wrong!");
                    break;
            }
        }
    }

}
