using Assets.Scripts.Database.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyManager : MonoBehaviour
{
    Trophy_Entity Trophy;
    public void RegisterUpgradeTrophy()
    {
        Trophy = References.listTrophy.Find(obj => obj.ID.Equals(References.accountRefer.TrophiesID));

        switch (Trophy.ID)
        {
            case "Trophie_None":
                Debug.Log("Trophie_None");
                
                if(References.accountRefer.Level >= Trophy.ContraitLevelAccount && References.accountRefer.Coin >= Trophy.Cost)
                {

                }
                break;
            case "Trophie_Genin":
                Debug.Log("Trophie_Genin");

                break;
            case "Trophie_Chunin":
                Debug.Log("Trophie_Chunin");

                break;
            case "Trophie_Jonin":
                Debug.Log("Trophie_Jonin");

                break;

        }



    }
}
