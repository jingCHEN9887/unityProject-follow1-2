using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Integrations.Match3;
using UnityEngine;

public enum Axel
{
    Left,
    Right
}

[Serializable]
public struct Wheel
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel;  //指定車輪屬於左軸還是右軸
}

public class AGVWheelController : MonoBehaviour  //控制輪子
{
    [SerializeField] private float maxAcceleration = 7.3f;  //Robotics MX-64 Max Torque.最大加速度
    [SerializeField] private float maxLinearVelocity = 5.0f;  //最大線速度
    [SerializeField] private float maxAngularVelocity = 0.8f;  //最大角速度=旋轉速度
    [SerializeField] private float brakeTorque = 3f;  // ?決定制動時施加的扭矩大小
    [SerializeField] private Vector3 _centerMass;  //?AGV的質心，設為AGV局部坐標系中的特定位置
    [SerializeField] private List<Wheel> wheels;  //輪子列表，包括視覺模型和碰撞器
    [SerializeField] private bool DebugMode;  //手控小車
    private float inputTurn, inputLinear;  //轉動和線性運動的輸入值
    private Rigidbody rb;
    private float lastAngle;  //先前的旋轉角度
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = _centerMass;
        inputLinear = 0.0f;
        inputTurn = 0.0f;
        lastAngle = transform.eulerAngles.y;
    }
    
    void Update()
    {
        AnimateWheels();
        // If you want to control by keyboard, please uncomment below.
        if (DebugMode)  //手控小車
        {
            KeyboardInput();
        }
    }
    private void FixedUpdate() //固定時間間隔使用，用於物理相關的計算和更新
    {
        Move(); //根據輸入值移動AGV
        lastAngle = transform.eulerAngles.y;  //更新 先前的旋轉角度
        Debug.Log("AGV Velocity : " + rb.velocity.sqrMagnitude.ToString("0.0"));
    }
    
    // LinearVel 單位為 m/s, AngularVel 單位為 rad/s.
    public void GetAction(float LinearVel, float AngularVel)
    {//在Move()用這些值來控制AGV
        inputLinear = LinearVel;
        inputTurn = AngularVel;
    }

    private void KeyboardInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            inputLinear = 1.0f;
            //inputLinear += 0.1f;  // 每次按下"W"键，线性速度增加0.1f, todo在合适时机重置inputLinear，以免速度无限增加
        }
        else if (Input.GetKey(KeyCode.S))
        {
            inputLinear = -1.0f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputTurn = -1.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            inputTurn = 1.0f;
        }
        else
        {
            inputTurn = 0.0f;
            inputLinear = 0.0f;
        }
        // Debug.Log("inputX = " + inputX.ToString() + ", inputY = " + inputY.ToString());
    }

    private void Move()  // 根据输入的线性速度和角速度，控制车辆移动
    {
        float radPerSec = Mathf.Abs(transform.eulerAngles.y - lastAngle) * Mathf.PI / 180 / Time.deltaTime;  // 角速度
        // Debug.Log("rad/s : " + radPerSec.ToString("0.00"));
        foreach (var wheel in wheels)
        {
            // Auto brake
            if (inputLinear == 0.0f && inputTurn == 0.0f)  //输入的线速度和角速度都为零=停止
            {
                wheel.collider.motorTorque = 0.0f;  //车轮马达扭矩=零
                wheel.collider.brakeTorque = brakeTorque;  //刹车扭矩=指定的刹车扭矩值
                // Debug.Log("Brake!");
            }
            else
            {
                // MaxSpeed Control
                if (rb.velocity.sqrMagnitude < maxLinearVelocity)  //当前速度 < 最大线速度，可加速
                {
                    wheel.collider.brakeTorque = 0.0f; //取消刹车力，允许车辆加速
                    if (radPerSec < maxAngularVelocity)  //角速度<最大角速度，可旋转
                    {
                        if (wheel.axel == Axel.Left)
                        { //根据输入的线性速度和角速度& maxAcceleration，设置轮子的马达扭矩
                          //将输入的线性速度和角速度相加，并限制在-1到1之间，然后乘以最大加速度
                            wheel.collider.motorTorque = Mathf.Clamp(inputTurn + inputLinear, -1f, 1f) * maxAcceleration;
                        }
                        else if (wheel.axel == Axel.Right)
                        {
                            //如果车辆无法旋转，根据输入的线性速度以及最大加速度，设置轮子的马达扭矩
                            wheel.collider.motorTorque = Mathf.Clamp(-inputTurn + inputLinear, -1f, 1f) * maxAcceleration;
                        }
                    }
                    else
                    {
                        wheel.collider.motorTorque = Mathf.Clamp(inputLinear, -1f, 1f) * maxAcceleration;
                    }
                    
                }
                else
                {
                    wheel.collider.motorTorque = 0.0f; //当前速度达到最大线性速度，則停止
                    wheel.collider.brakeTorque = brakeTorque; //刹车: 减速
                }
            }
        }
    }
    
    private void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion _rot;
            Vector3 _pos;
            wheel.collider.GetWorldPose(out _pos, out _rot);
            wheel.model.transform.position = _pos;
            wheel.model.transform.rotation = _rot;
        }
    }
}
