using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : StateMachineBehaviour
{
    public const string IDLE_STATE = "Idle";
    public float  m_playerSpeed = 2;

    PlayerMovement playerMovement;  

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        if (playerMovement == null)
        {
            playerMovement = PlayerMovement.GetInstance();
        } 
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(IDLE_STATE); 
        
        if (playerMovement.isIdle == false)
        {
            animator.SetTrigger(IDLE_STATE);
        } 
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 direction = playerMovement.GetMoveDirection();

        Vector3 lookatPos = animator.transform.position + direction;
        lookatPos.y = animator.transform.position.y;

        animator.transform.LookAt(lookatPos);
        animator.transform.position = animator.transform.position + (direction * m_playerSpeed * Time.deltaTime);
    }
}


//Look rotation viewing vector is zero
//    UnityEngine.Quaternion:LookRotation (UnityEngine.Vector3)
//PlayerMovement:Update () (at Assets/Scripts/PlayerMovement.cs:43)



