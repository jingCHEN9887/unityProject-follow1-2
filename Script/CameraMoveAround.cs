using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveAround : MonoBehaviour
{
    public GameObject Target;
    public float speed;

    void Update()
    {
        transform.RotateAround(Target.transform.localPosition, Vector3.up,  speed*Time.deltaTime);
    }
}
