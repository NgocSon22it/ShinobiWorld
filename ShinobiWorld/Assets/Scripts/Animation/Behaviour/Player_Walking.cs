using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Walking : StateMachineBehaviour
{
    PlayerBase Player;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player = animator.GetComponent<PlayerBase>();
        Player.Animation_SetUpWalking(true);
    }
}
