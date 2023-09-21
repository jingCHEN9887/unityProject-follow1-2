using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joyStickTest : MonoBehaviour
{
    protected float _joy_x = 0f;
    protected float _joy_y = 0f;

    void Update()
    {
        _joy_x = Input.GetAxis("Horizontal_Left");
        _joy_y = Input.GetAxis("Vertical_Left");
        
        Debug.Log("Get JoyStick Horizontal : " + _joy_x.ToString());
        Debug.Log("Get JoyStick Vertical : " + _joy_y.ToString());
    }
}
