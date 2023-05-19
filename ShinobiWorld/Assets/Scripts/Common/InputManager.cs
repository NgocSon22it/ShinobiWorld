using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using WebSocketSharp;

namespace Assets.Scripts.Common
{
    public class InputManager : MonoBehaviour
    {
        public TMP_InputField input;
        public static InputManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void CheckMin()
        {
            if(input.text.IsNullOrEmpty()) input.text = "1";
            else
            {
                var value = int.Parse(input.text);
                if (value <= 0) input.text = "1";
            }
            
        }

        public void CheckMax(int limit)
        {
            var value = int.Parse(input.text);
            if (value > limit) input.text = limit.ToString();
        }

        public void Plus()
        {
            var value = int.Parse(input.text);
            input.text = (++value).ToString();
        }
        public void Min()
        {
            var value = int.Parse(input.text);
            input.text = (--value).ToString();
        }
    }
}
