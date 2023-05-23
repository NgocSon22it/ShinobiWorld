using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_ButtonManagement : MonoBehaviour
{

    [SerializeField] GameObject SkillOne_Cooldown;
    [SerializeField] GameObject SkillTwo_Cooldown;
    [SerializeField] GameObject SkillThree_Cooldown;

    [SerializeField] GameObject SkillOne_LowChakra;
    [SerializeField] GameObject SkillTwo_LowChakra;
    [SerializeField] GameObject SkillThree_LowChakra;

    [SerializeField] GameObject SkillOne_Lock;
    [SerializeField] GameObject SkillTwo_Lock;
    [SerializeField] GameObject SkillThree_Lock;

    PlayerBase Player;

    private void Update()
    {
        if (Player != null)
        {
            SkillOne();
            SkillTwo();
            SkillThree();
        }
    }

    public void SetUpPlayer(GameObject player)
    {
        Player = player.GetComponent<PlayerBase>();
    }

    public void SkillOne()
    {
        if(Player.GetComponent<PlayerBase>().SkillOne_Entity != null)
        {
            SkillOne_Lock.SetActive(false);
            if (Player.GetComponent<PlayerBase>().SkillOneCooldown_Current > 0)
            {
                SkillOne_Cooldown.SetActive(true);
                SkillOne_Cooldown.GetComponent<Image>().fillAmount = Player.SkillOneCooldown_Current / Player.SkillOneCooldown_Total;
            }
            else
            {
                SkillOne_Cooldown.SetActive(false);
                if (Player.GetComponent<PlayerBase>().AccountEntity.CurrentCharka >= Player.GetComponent<PlayerBase>().SkillOne_Entity.Chakra)
                {
                    SkillOne_LowChakra.SetActive(false);
                }
                else
                {
                    SkillOne_LowChakra.SetActive(true);
                }
            }
        }
        else
        {
            SkillOne_Lock.SetActive(true);           
        }         
    }

    public void SkillTwo()
    {
        if(Player.GetComponent<PlayerBase>().SkillTwo_Entity != null)
        {
            SkillTwo_Lock.SetActive(false);

            if (Player.GetComponent<PlayerBase>().SkillTwoCooldown_Current > 0)
            {
                SkillTwo_Cooldown.SetActive(true);
                SkillTwo_Cooldown.GetComponent<Image>().fillAmount = Player.SkillTwoCooldown_Current / Player.SkillTwoCooldown_Total;
            }
            else
            {
                SkillTwo_Cooldown.SetActive(false);
                if (Player.GetComponent<PlayerBase>().AccountEntity.CurrentCharka >= Player.GetComponent<PlayerBase>().SkillTwo_Entity.Chakra)
                {
                    SkillTwo_LowChakra.SetActive(false);
                }
                else
                {
                    SkillTwo_LowChakra.SetActive(true);
                }
            }
        }
        else
        {
            SkillTwo_Lock.SetActive(true);

        }

    }

    public void SkillThree()
    {
        if (Player.GetComponent<PlayerBase>().SkillThree_Entity != null)
        {
            SkillThree_Lock.SetActive(false);
            if (Player.GetComponent<PlayerBase>().SkillThreeCooldown_Current > 0)
            {
                SkillThree_Cooldown.SetActive(true);
                SkillThree_Cooldown.GetComponent<Image>().fillAmount = Player.SkillThreeCooldown_Current / Player.SkillThreeCooldown_Total;
            }
            else
            {
                SkillThree_Cooldown.SetActive(false);
                if (Player.GetComponent<PlayerBase>().AccountEntity.CurrentCharka >= Player.GetComponent<PlayerBase>().SkillThree_Entity.Chakra)
                {
                    SkillThree_LowChakra.SetActive(false);
                }
                else
                {
                    SkillThree_LowChakra.SetActive(true);
                }
            }
        }
        else
        {
            SkillThree_Lock.SetActive(true);
        }
    }

}
