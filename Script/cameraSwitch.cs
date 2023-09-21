using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraSwitch : MonoBehaviour
{
    public GameObject cam1, cam2;
    void Awake()
    {
        cam1.SetActive(true);
        cam2.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.F1))
        {
            if (cam1.activeInHierarchy)
            {
                cam1.SetActive(false);
                cam2.SetActive(true);
            }
            else
            {
                cam2.SetActive(false);
                cam1.SetActive(true);
            }
        }
    }
}
