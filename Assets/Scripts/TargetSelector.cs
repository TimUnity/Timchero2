using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TargetSelector : MonoBehaviour
{
    [HideInInspector]
    public GameObject priorityTarget;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject endDoor;
    [SerializeField] private GameObject startDoor;
    [SerializeField] private Transform shootingStartPoint;
     
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private int healthLimit = 100; 
    [SerializeField] private int hitPower = 25;
    [SerializeField] private float projectileFireRate = 1.1f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float attackRange = 15f;
    [SerializeField] private string enemyTag = "ENEMY";
     
    private float timer;
    EnemyManager enemyManager;
    PlayerMovement playerMovement; 
    
    //Класс отвечающий за выбор цели и стрельбу по ней
    private void Start()
    {
        //Подключам другие объекты
        if (enemyManager == null)
        {
            enemyManager = EnemyManager.GetInstance();
        }

        if (playerMovement == null)
        {
            playerMovement = PlayerMovement.GetInstance();
        }

        

        gameObject.GetComponent<Health>().SetHealth(healthLimit); 
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        //Определяем координаты ближайшего врага, пробегаем по списку
        Vector3 target = enemyManager.GetNearestTarget(transform.position); 
        bool canShoot = playerMovement.isIdle;

        if (target != Vector3.zero)
        {  
            //Вычисляем вектор направления от игрока к врагу
            Vector3 difference = gameObject.transform.position - target;
            float distanceTo = difference.magnitude;

            if (canShoot)
            { 
                //Проверяем дистанцию до врага
                if (distanceTo < attackRange)
                { 
                    Vector3 correctedTarget = new Vector3(target.x, 0.1f, target.z); 
                    transform.LookAt(correctedTarget); 

                    //Стреляем по таймеру
                    if (timer > projectileFireRate)
                    {
                        Vector3 direction = -difference / distanceTo;
                        direction.Normalize();
                        Shoot(direction);
                    }
                }
            }
        }
    }

    private void Shoot(Vector3 direction)
    {
        //Создаем объект снаряда поворачиваем и запускаем его в сторону врага
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = shootingStartPoint.position;
        projectile.transform.localScale = new Vector3(0.2f, 0.2f, 0.5f); 
        projectile.transform.rotation = Quaternion.LookRotation(direction);
        //Указываем тэг цели и наносимый урон для данного снаряда и его скорость
        projectile.GetComponent<ProjectileControl>().SetTargetTag(enemyTag);
        projectile.GetComponent<ProjectileControl>().SetHitPower(hitPower);
        projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        //сбрасываем таймер "перезарядки"
        timer = 0.0f; 
    }

    public float GetSpeed()
    {
        float speed = movementSpeed;
        return speed;
    }
}
