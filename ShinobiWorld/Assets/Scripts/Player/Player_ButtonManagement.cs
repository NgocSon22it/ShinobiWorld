using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_ButtonManagement : MonoBehaviour
{

    [SerializeField] GameObject SkillOneCooldown;
    [SerializeField] GameObject SkillTwoCooldown;
    [SerializeField] GameObject SkillThreeCooldown;

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
        if (Player.GetComponent<PlayerBase>().SkillOneCooldown_Current > 0)
        {
            SkillOneCooldown.SetActive(true);
            SkillOneCooldown.GetComponent<Image>().fillAmount = Player.SkillOneCooldown_Current / Player.SkillOneCooldown_Total;
        }
        else
        {
            SkillOneCooldown.SetActive(false);
        }
    }

    public void SkillTwo()
    {
        if (Player.GetComponent<PlayerBase>().SkillTwoCooldown_Current > 0)
        {
            SkillTwoCooldown.SetActive(true);
            SkillTwoCooldown.GetComponent<Image>().fillAmount = Player.SkillTwoCooldown_Current / Player.SkillTwoCooldown_Total;
        }
        else
        {
            SkillTwoCooldown.SetActive(false);
        }
    }

    public void SkillThree()
    {
        if (Player.GetComponent<PlayerBase>().SkillThreeCooldown_Current > 0)
        {
            SkillThreeCooldown.SetActive(true);
            SkillThreeCooldown.GetComponent<Image>().fillAmount = Player.SkillThreeCooldown_Current / Player.SkillThreeCooldown_Total;
        }
        else
        {
            SkillThreeCooldown.SetActive(false);
        }
    }

}
