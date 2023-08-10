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
                    break;
                case "Range":
                    break;
                case "Support":
                    break;
                default:
                    break;
            }
        }
    }

}
