using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private GameObject startGate;
    private GameObject[] enemyesWalking;
    private GameObject[] enemyesFlying;
    EnemyManager enemyManager;

    private void Start()
    { 
        if (enemyManager == null)
        {
            enemyManager = EnemyManager.GetInstance();
        }

        startGate = GameObject.FindGameObjectWithTag("START_DOOR");
        //enemyesWalking = GameObject.FindGameObjectsWithTag("ENEMY");
        //enemyesFlying = GameObject.FindGameObjectsWithTag("ENEMY_FLY");
    }

    //При столкновении игрока с точкой началла уровня происходит старт уровня
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            startGate.gameObject.transform.DOLocalMoveY(-15.6f, 2f);
            print("STARTO!!!");

            foreach (var enemy in enemyManager.enemyList)
            {
                enemy.gameObject.GetComponent<EnemyComponent>().SetEnemyEnabled(true);
                print("enemy: " + enemy.gameObject.name);
            } 
        }
    }
}
