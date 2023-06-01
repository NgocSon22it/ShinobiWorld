using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Shop
{
    public class ShopClick : MonoBehaviour
    {
        void OnMouseDown()
        {
            ShopManager.Instance.OnShopBtnClick(); 
        }
    }
}
