using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaller : MonoBehaviour
{
    //Настройки камеры
    private int currentWidth = 0;
    private int currentHeight = 0;
 
    public float widthAmount = 33.5f;  
    public float heightAmount = 20f; 
 
    private Camera mainCamera;
 
    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        SetResolution();
    }
 
    private void SetResolution()
    {
        currentWidth = Screen.width;
        currentHeight = Screen.height;
        float width_size = (float)(widthAmount * Screen.height / Screen.width * 0.5);
        float height_size = (float)(heightAmount * Screen.width / Screen.height * 0.5) * ((float)Screen.height/ Screen.width);
        mainCamera.orthographicSize = Mathf.Max(height_size, width_size);
    }
 
    void Update()
    {
        if(Screen.width != currentWidth || Screen.height != currentHeight)
        {
            SetResolution();
        }
    }
}
