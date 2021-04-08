using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //массив врагов
    public GameObject[] enemiesToSpawn;
    private EnemyManager enemyManager;
    private float x_pos;
    private float y_pos;
    private float z_pos;
    private float scale;

    Coroutine StartEnemyCreation;
    
    private void Start()
    { 
        if (enemyManager == null)
        {
            enemyManager = EnemyManager.GetInstance();
        } 

        //ќпредел€ем координаты "высадки"
        foreach (var item in enemiesToSpawn)
        {
            if (item.name == "AirEnemy")
            {
                x_pos = Random.Range(-9.4f, 9.5f);
                z_pos = Random.Range(-21.0f, 4.0f);
                y_pos = 6f;
                scale = 1.5f; 
            }
            else
            {
                x_pos = Random.Range(-8.3f, 8.4f);
                z_pos = Random.Range(-23.0f, -27.0f);
                y_pos = 1f;
                scale = 6f; 
            }

            Vector3 newPosition = new Vector3(x_pos, y_pos, z_pos);
            
            //¬ысаживаем врагов по рандомным координатам
            StartEnemyCreation = StartCoroutine(DelayedEnemyCreation(item, newPosition, scale, 0.5f));
        } 
    }  

    //¬ысаживаем врагов по рандомным координатам
    private IEnumerator DelayedEnemyCreation(GameObject Enemy, Vector3 enemyPosition, float enemyScale, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject spawnedEnemy = Instantiate(Enemy);
        spawnedEnemy.transform.position = enemyPosition/*new Vector3(x_pos, y_pos, z_pos)*/;
        spawnedEnemy.transform.localScale = new Vector3(enemyScale, enemyScale, enemyScale); 
    }  
}
