using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class CoinAnimator : MonoBehaviour
{
    NavMeshAgent navmeshAgent;
    
    [SerializeField] private GameObject player;
    private void Start()
    { 
        player = GameObject.FindGameObjectWithTag("Player");
        
        navmeshAgent = GetComponent<NavMeshAgent>();
        navmeshAgent.speed = 10f;
        navmeshAgent.enabled = true;

        DOTween.SetTweensCapacity(500,50);
        gameObject.transform.DOLocalRotate(new Vector3(0f, 180f, 0f),  5f, RotateMode.Fast);

        StartCoroutine(MoveToPlayer(2f));
    }

    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator MoveToPlayer(float delay)
    {
        //Остановка передвижения
        yield return new WaitForSeconds(delay); 
        navmeshAgent.SetDestination(player.transform.position); 
    }
}
