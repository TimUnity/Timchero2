using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public List<EnemyComponent> enemyList = new List<EnemyComponent>();

    //Добавляем врага в список врагов
    public void IncludeEnemy(EnemyComponent enemy) 
    { 
        enemyList.Add(enemy); 
    }

    //удаляем врага из списка врагов
    public void ExcludeEnemy(EnemyComponent enemy) 
    { 
        enemyList.Remove(enemy); 
    }

    public bool TargetExists() 
    { 
        return enemyList.Count > 0; 
    }

    //получаем ближайший вражеский объект
    public GameObject GetNearestEnemy(Vector3 origin)
    {
        EnemyComponent nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (EnemyComponent enemy in enemyList)
        {
            float distance = (enemy.transform.position - origin).sqrMagnitude;

            if (distance < minDistance)
            {
                nearestEnemy = enemy;
                minDistance = distance;
            }
        } 

        if (nearestEnemy == null) { return null; }
        else { return nearestEnemy.gameObject; }
    }

    //Получаем вектор в сторону ближайшего врага
    public Vector3 GetNearestTarget(Vector3 origin)
    {
        EnemyComponent nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (EnemyComponent enemy in enemyList)
        {
            float distance = (enemy.transform.position - origin).sqrMagnitude;

            if (distance < minDistance)
            {
                nearestEnemy = enemy;
                minDistance = distance;
            }
        } 

        if (nearestEnemy == null) { return Vector3.zero; }
        else { return nearestEnemy.transform.position; }
    } 
}
