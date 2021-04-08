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
    
    //����� ���������� �� ����� ���� � �������� �� ���
    private void Start()
    {
        //��������� ������ �������
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
        //���������� ���������� ���������� �����, ��������� �� ������
        Vector3 target = enemyManager.GetNearestTarget(transform.position); 
        bool canShoot = playerMovement.isIdle;

        if (target != Vector3.zero)
        {  
            //��������� ������ ����������� �� ������ � �����
            Vector3 difference = gameObject.transform.position - target;
            float distanceTo = difference.magnitude;

            if (canShoot)
            { 
                //��������� ��������� �� �����
                if (distanceTo < attackRange)
                { 
                    Vector3 correctedTarget = new Vector3(target.x, 0.1f, target.z); 
                    transform.LookAt(correctedTarget); 

                    //�������� �� �������
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
        //������� ������ ������� ������������ � ��������� ��� � ������� �����
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = shootingStartPoint.position;
        projectile.transform.localScale = new Vector3(0.2f, 0.2f, 0.5f); 
        projectile.transform.rotation = Quaternion.LookRotation(direction);
        //��������� ��� ���� � ��������� ���� ��� ������� ������� � ��� ��������
        projectile.GetComponent<ProjectileControl>().SetTargetTag(enemyTag);
        projectile.GetComponent<ProjectileControl>().SetHitPower(hitPower);
        projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
        //���������� ������ "�����������"
        timer = 0.0f; 
    }

    public float GetSpeed()
    {
        float speed = movementSpeed;
        return speed;
    }
}
