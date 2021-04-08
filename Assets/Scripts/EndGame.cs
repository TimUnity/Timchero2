using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{  
    //При столкновении игрока с точкой завершения уровня происходит завершение уровня
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            print("Level Complitted");
            SceneManager.LoadScene("Level");
        }
    }
}
