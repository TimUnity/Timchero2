using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{  
    //��� ������������ ������ � ������ ���������� ������ ���������� ���������� ������
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            print("Level Complitted");
            SceneManager.LoadScene("Level");
        }
    }
}
