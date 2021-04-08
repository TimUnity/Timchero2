using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{ 
    // ласс отвечающий за получаемый урон и бонусы

    [SerializeField] float healthLimit = 100f;
    private GameObject CoinsTxt;
    private GameObject HealthTxt; 
    private GameObject player; 

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        CoinsTxt = GameObject.FindGameObjectWithTag("COINS");
        HealthTxt = GameObject.FindGameObjectWithTag("HEALTH");
    }

    private void ReduceHeath(float reduceValue)
    {
        if (healthLimit >= reduceValue)
        {
            healthLimit -= reduceValue;
        }
        else
        {
            healthLimit = 0;
        }

        if (healthLimit <= 0)
        {
            Destroy(gameObject);
        }

        HealthTxt.gameObject.GetComponent<TextMeshProUGUI>().text =
            player.gameObject.GetComponent<Health>().healthLimit.ToString();
    }

    private void IncreaseHeath(float increaseValue)
    { 
        healthLimit += increaseValue; 

        HealthTxt.gameObject.GetComponent<TextMeshProUGUI>().text =
            player.gameObject.GetComponent<Health>().healthLimit.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        //ѕри столкновении со снар€дами
        if (other.transform.CompareTag("PROJECTILE") && 
            gameObject.transform.tag.Contains(other.GetComponent<ProjectileControl>().GetTargetTag()))
        {
            var hitPower = other.GetComponent<ProjectileControl>().GetHitPower();
            ReduceHeath(hitPower); 
        }   

        //при столкновении с монетами
        if (other.transform.CompareTag("COIN"))
        {
            IncreaseHeath(25);

            int coinsCount = Convert.ToInt16(CoinsTxt.gameObject.GetComponent<TextMeshProUGUI>().text);
            coinsCount++;

            CoinsTxt.gameObject.GetComponent<TextMeshProUGUI>().text = coinsCount.ToString();

            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        //ѕри столкновении с врагом
        if (collider.transform.tag.Contains("ENEMY") && gameObject.transform.CompareTag("Player"))
        {
            try
            {
                var hitPower = collider.gameObject.GetComponent<ProjectileControl>().GetHitPower();
                ReduceHeath(hitPower * 2); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            } 
        } 
        //print("collided with: " + collider.gameObject.name);
    }

    public void SetHealth(int value)
    {
        if (value >= 0)
        {
            healthLimit = value;
        }
    }
}
