using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireState : StateMachineBehaviour
{
    bool canFire = false; 
    PlayerMovement playerMovement;  
    EnemyManager enemyManager;
    WeaponComponent weaponComponent;  

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerMovement == null)
        {
            playerMovement = PlayerMovement.GetInstance();
        }

        if (enemyManager == null)
        {
            enemyManager = EnemyManager.GetInstance();
        }

        if (weaponComponent == null) 
        { 
            weaponComponent = animator.GetComponent<WeaponComponent>(); 
        }

        canFire = true;
    } 

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Получаем вектор в сторону ближайшего врага
        Vector3 target = enemyManager.GetNearestTarget(animator.transform.position);
        if (target != Vector3.zero)
        { 
            target.y = animator.transform.position.y + 1;

            if (canFire)
            {
                canFire = false;
                weaponComponent.FireWeapon(animator.transform.position + Vector3.up, target);
            }
            else animator.SetTrigger(MoveState.IDLE_STATE);
        }

        if (playerMovement.isIdle == false)
        {
            animator.SetTrigger(IdleState.MOVE_STATE);
        }
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 target = enemyManager.GetNearestTarget(animator.transform.position);
        
        if (target != Vector3.zero)
        { 
            target.y = animator.transform.position.y + 1;
            animator.gameObject.transform.LookAt(target);
        }
    }
}













