using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
 
[RequireComponent(typeof(Animator))]

public class EnemyComponent : MonoBehaviour
{
    NavMeshAgent navmeshAgent;
    Animator animator;
    private EnemyManager enemyManager;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject blow;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject endLevelDoor;
    [SerializeField] private Transform shootingStartPoint;
    [SerializeField] private float agrDistance = 20.0f;
    [SerializeField] private float IdleTime = 3f;
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float moveTime = 5f;
    [SerializeField] private int healthLimit = 100;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int hitPower = 25;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private string enemyTag = "Player";

    private float timer;
    private bool isIdle;
    private Coroutine EnemyStartMovement;
    private Coroutine EnemyStopMovement;
    public bool isEnemyEnabled = false;
    
    private void Start()
    { 
        if (enemyManager == null)
        {
            enemyManager = EnemyManager.GetInstance();
        } 

        //Добавляем вражеский объект в список врагов
        EnemyManager.GetInstance().IncludeEnemy(this); 
        player = GameObject.FindGameObjectWithTag("Player");
        endLevelDoor = GameObject.FindGameObjectWithTag("END_DOOR");

        EnemyStartMovement = null;
        EnemyStopMovement = null;
        animator = GetComponent<Animator>();  
        gameObject.GetComponent<Health>().SetHealth(healthLimit);   
    }

    private void FixedUpdate()
    {
        if (isEnemyEnabled)
        {
            float step =  movementSpeed * Time.deltaTime; 

            //Действия при перемещении
            if (!isIdle)
            {
                if (player != null)
                {
                    //Передвижение в сторону игрока
                    if (gameObject.transform.CompareTag("ENEMY"))
                    { 
                        navmeshAgent.SetDestination(player.transform.position); 
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, 
                            new Vector3(player.transform.position.x, 6.0f, player.transform.position.z), step);
                    }
                 
                    //Поворот в сторону игрока
                    transform.LookAt(new Vector3(player.transform.position.x, 6.0f, player.transform.position.z));
                }
            }

            timer += Time.deltaTime;
            Vector3 target = Vector3.zero;

            if (player != null)
            {
                //Определение игрока как цели
                target = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z);
            }  

            if (target != Vector3.zero)
            {
                Vector3 difference = gameObject.transform.position - target;  
                float distanceTo = difference.magnitude;

                if (!isIdle)
                {
                    //Действия при пересечении условной линии дистанции до врага
                    if (distanceTo < agrDistance)
                    {
                        StopCoroutine(EnemyStartMovement);
                        StopCoroutine(EnemyStopMovement);
                    
                        if (gameObject.transform.CompareTag("ENEMY"))
                        { 
                            navmeshAgent.ResetPath(); 
                        } 

                        isIdle = true;
                        EnemyStartMovement = StartCoroutine(MoveStart(IdleTime));
                    } 
                }

                //Действия при остановке
                if (isIdle)
                {  
                    //Поворот в сторону игрока
                    transform.LookAt(new Vector3(player.transform.position.x, 6.0f, player.transform.position.z));
                 
                    //проверка дистанции
                    if (distanceTo < agrDistance)
                    {
                        Vector3 correctedTarget = new Vector3(target.x, target.y + 5f, target.z);
                        transform.LookAt(correctedTarget);

                        //Стрельба по таймеру
                        if (timer > fireRate)
                        {
                            Vector3 direction = -difference / distanceTo;
                            direction.Normalize();
                            //direction = new Vector3(direction.x, 5f, direction.z);
                            Shoot(direction); 
                        }
                    }
                }
            }
        }
    }

    private IEnumerator MoveStart(float delay)
    {
        //Запуск предвижения
        yield return new WaitForSeconds(delay);
        isIdle = false; 
        EnemyStopMovement = StartCoroutine(MoveStop(moveTime));
    }

    private IEnumerator MoveStop(float delay)
    {
        //Остановка передвижения
        yield return new WaitForSeconds(delay); 
        isIdle = true;
        
        if (gameObject.transform.CompareTag("ENEMY"))
        { 
            navmeshAgent.ResetPath();
        } 
 
        EnemyStartMovement = StartCoroutine(MoveStart(IdleTime));
    }

    private void OnDestroy()
    {
        //при уничтожении врага, происходит вычеркивание его из списка живых врагов
        if (EnemyManager.GetInstance())
        {
            EnemyManager.GetInstance().ExcludeEnemy(this);

            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                CoinSpawner();
            }

            //Формальный взрыв
            GameObject newBlow = Instantiate(blow);
            newBlow.transform.position = gameObject.transform.position;
            newBlow.transform.localScale = new Vector3(1f, 1f, 1f);   
        }

        //Проверка на всех убитых, если врагов нет, то открывается дверь на сл уровень
        if (enemyManager.enemyList.Count == 0)
        { 
            endLevelDoor.gameObject.transform.DOLocalMoveY(-15.6f, 2f);
        }
    }

    private void CoinSpawner()
    {
        //Выпадение монетки из убитого врага
        GameObject newCoin = Instantiate(coin);
        newCoin.transform.position = gameObject.transform.position;
        newCoin.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
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

    public void SetEnemyEnabled(bool value)
    { 
        if (value)
        {
            if (gameObject.transform.CompareTag("ENEMY"))
            {
                navmeshAgent = GetComponent<NavMeshAgent>();
                navmeshAgent.speed = movementSpeed;
                navmeshAgent.enabled = true;
            }

            isIdle = true; 
            EnemyStartMovement = StartCoroutine(MoveStart(IdleTime)); 
            
            isEnemyEnabled = value;
        }
    }
}
