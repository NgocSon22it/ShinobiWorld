using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public class InputValidation : MonoBehaviour
    {

        public void CheckMin()
        {
            Debug.Log(GetComponent<TMP_InputField>().text.ToString());
        }
    }
}
