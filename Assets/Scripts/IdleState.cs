using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    public const string MOVE_STATE = "Move";
    const string FIRE_STATE = "Fire";
     
    PlayerMovement playerMovement;
    EnemyManager m_enemyManager;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        if (playerMovement == null)
        {
            playerMovement = PlayerMovement.GetInstance();
        }

        if (m_enemyManager == null)
        {
            m_enemyManager = EnemyManager.GetInstance();
        }

        animator.ResetTrigger(MOVE_STATE);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        if (playerMovement.isIdle == false) 
        { 
            animator.SetTrigger(MOVE_STATE); 
        }
        else if (m_enemyManager.TargetExists())animator.SetTrigger(FIRE_STATE);
    }
}









