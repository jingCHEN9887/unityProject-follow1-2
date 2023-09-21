using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
#pragma warning disable CS0234 // 命名空間 'Unity.MLAgents' 中沒有類型或命名空間名稱 'Actuators' (是否遺漏了組件參考?)
using Unity.MLAgents.Actuators;
#pragma warning restore CS0234 // 命名空間 'Unity.MLAgents' 中沒有類型或命名空間名稱 'Actuators' (是否遺漏了組件參考?)
#pragma warning disable CS0234 // 命名空間 'Unity.MLAgents' 中沒有類型或命名空間名稱 'Sensors' (是否遺漏了組件參考?)
using Unity.MLAgents.Sensors;
#pragma warning restore CS0234 // 命名空間 'Unity.MLAgents' 中沒有類型或命名空間名稱 'Sensors' (是否遺漏了組件參考?)
using Random = UnityEngine.Random;

#pragma warning disable CS0246 // 找不到類型或命名空間名稱 'Agent' (是否遺漏了 using 指示詞或組件參考?)
public class FinalBotControl : Agent
#pragma warning restore CS0246 // 找不到類型或命名空間名稱 'Agent' (是否遺漏了 using 指示詞或組件參考?)
{
    public GameObject target;
    [Range(0f, 10f)]
    public float MaxLinearVelocity;
    [Range(0.1f, 1.0f)]
    public float MaxAngularVelocity;

    public LayerMask unwalkableMask;
    public Text h_text;

    public bool joyStick;
    
    Rigidbody rBody;
    private int n_threshold;
    private int _stage;
    private Vector3 _lastGoal;
    private float startDistance;

#pragma warning disable CS0115 // 'FinalBotControl.Initialize()': 未找到任何合適的方法可覆寫
    public override void Initialize()
#pragma warning restore CS0115 // 'FinalBotControl.Initialize()': 未找到任何合適的方法可覆寫
    {
        rBody = GetComponent<Rigidbody>();
        _stage = 0;
        _lastGoal = new Vector3(21.6f,0.5f,2.9f);
    }

#pragma warning disable CS0115 // 'FinalBotControl.OnEpisodeBegin()': 未找到任何合適的方法可覆寫
    public override void OnEpisodeBegin()
#pragma warning restore CS0115 // 'FinalBotControl.OnEpisodeBegin()': 未找到任何合適的方法可覆寫
    {
        //set velocity to zero
        rBody.velocity = transform.forward * 0f;
        transform.Rotate(transform.up, Time.fixedDeltaTime * 0f);

        switch (_stage)
        {
            case 0:
                // Agent Pos
                transform.localPosition = new Vector3(21.6f,0.5f,2.9f);
                transform.localEulerAngles = new Vector3 (0.0f,Random.value * 120f - 150f,0.0f);
                // Goal Pos
                target.transform.localPosition = new Vector3(Random.value * 4.9f + 18.5f, 0.5f, Random.value * 2.1f - 5.9f);
                break;
            case 1:
                // Agent keep origin Pos
                transform.localPosition = _lastGoal;
                // Goal Pos
                target.transform.localPosition = new Vector3(Random.value * 2.9f + 22f, 0.5f, Random.value * 9.2f - 22f);
                break;
            case 2: 
                // Agent keep origin Pos
                transform.localPosition = _lastGoal;
                // Goal Pos
                target.transform.localPosition = new Vector3(Random.value * 2.4f + 13.1f, 0.5f, Random.value * 5.9f - 21.1f);
                break;
            case 3:
                // Agent keep origin Pos
                transform.localPosition = _lastGoal;
                // Goal Pos
                target.transform.localPosition = new Vector3(Random.value * 6.7f - 16f, 0.5f, Random.value * 6.4f - 21f);
                break;
            case 4: // Escape position
                // Agent Pos
                transform.localPosition = new Vector3(Random.value * 2.4f - 24f, 0.5f, Random.value * 1.8f - 22.3f);
                transform.localEulerAngles = new Vector3(0.0f, Random.value * 360f - 180f, 0.0f);
                // Goal Pos
                target.transform.localPosition = new Vector3(Random.value * 2.9f - 21f, 0.5f, Random.value * 4.9f - 16.1f);
                break;
            case 5:
                // Agent keep origin Pos
                transform.localPosition = _lastGoal;
                // Goal Pos
                target.transform.localPosition = new Vector3(Random.value * 3.2f - 21.3f, 0.5f, Random.value * 5.1f + 3.9f);
                break;
            case 6:
                // Agent keep origin Pos
                transform.localPosition = _lastGoal;
                // Goal Pos
                target.transform.localPosition = new Vector3(Random.value * 5.2f - 17.5f, 0.5f, Random.value * 4.7f + 15.1f);
                break;
            case 7:
                // Agent keep origin Pos
                transform.localPosition = _lastGoal;
                // Goal Pos
                target.transform.localPosition = new Vector3(Random.value * 3.1f - 6.8f, 0.5f, Random.value * 3.7f + 17.5f);
                break;
            case 8:
                // Agent keep origin Pos
                transform.localPosition = _lastGoal;
                // Goal Pos
                target.transform.localPosition = new Vector3(Random.value * 4.4f + 2.1f, 0.5f, Random.value * 8f + 14.1f);
                break;
            case 9:
                // Agent keep origin Pos
                transform.localPosition = _lastGoal;
                // Goal Pos
                target.transform.localPosition = new Vector3(Random.value * 2.1f + 12.2f, 0.5f, Random.value * 7.9f + 14.2f);
                break;
            case 10:
                // Agent keep origin Pos
                transform.localPosition = _lastGoal;
                // Goal Pos
                target.transform.localPosition = new Vector3(Random.value * 3.6f + 21.2f, 0.5f, Random.value * 6.1f + 9.7f);
                break;
            case 11:
                // Agent keep origin Pos
                transform.localPosition = _lastGoal;
                // Goal Pos
                target.transform.localPosition = new Vector3(21.6f,0.5f,2.9f);  // go back to start point
                break;
            case 12:
                // Agent Pos
                transform.localPosition = new Vector3(-17f, 0.5f, Random.value * 6.8f - 9.8f);
                transform.localEulerAngles = new Vector3(0.0f,Random.value * 60f + 60f,0.0f);
                // Goal Pos
                target.transform.localPosition = new Vector3(-1.1f, 0.5f, Random.value * 3.2f - 8f);
                break;
            case 13:
                // Agent Pos
                transform.localPosition = _lastGoal;
                // Goal Pos
                target.transform.localPosition = new Vector3(17.4f, 0.5f, Random.value * 5.1f - 9f);
                break;
            case 14:
                // Agent Pos
                transform.localPosition = new Vector3(-15.8f, 0.5f, Random.value * 3.6f + 2f);
                transform.localEulerAngles = new Vector3(0.0f,Random.value * 60f + 60f,0.0f);
                // Goal Pos
                target.transform.localPosition = new Vector3(-1.1f, 0.5f, Random.value * 5.3f + 1.5f);
                break;
            case 15:
                // Agent Pos
                transform.localPosition = _lastGoal;
                // Goal Pos
                target.transform.localPosition = new Vector3(15.7f, 0.5f, Random.value * 3.5f + 3.2f);
                break;
        }
        
        this.n_threshold = 0;
        startDistance = Vector3.Distance(gameObject.transform.localPosition, target.transform.localPosition);
    }
    
#pragma warning disable CS0246 // 找不到類型或命名空間名稱 'VectorSensor' (是否遺漏了 using 指示詞或組件參考?)
    public override void CollectObservations(VectorSensor sensor)
#pragma warning restore CS0246 // 找不到類型或命名空間名稱 'VectorSensor' (是否遺漏了 using 指示詞或組件參考?)
    {
        // Relationship between Agent and Target
        float distanceToTarget = Vector3.Distance(gameObject.transform.localPosition, target.transform.localPosition);
        float angleToTarget = GetAngle(target.transform, gameObject.transform);
        
        sensor.AddObservation(distanceToTarget);    // float var
        sensor.AddObservation(angleToTarget);       // float var

        sensor.AddObservation(rBody.velocity.x);      // float
        sensor.AddObservation(rBody.velocity.z);      // float
        sensor.AddObservation(transform.localEulerAngles.y);      // float
        int h_action = hierarchicalModeSwitch();
        sensor.AddObservation(h_action);                //int
    }
    
    void OnCollisionStay(Collision col)
    {
        // if(col.gameObject.tag == "Wall")
        // {
        //     // Debug.Log("你撞到牆了");
        //     AddReward(-0.1f);
        // }
        //
        // if(col.gameObject.tag == "Obstacle")
        // {
        //     // Debug.Log("你撞到障礙物了");
        //     AddReward(-0.1f);
        // }
        //
        // if(col.gameObject.tag == "People")
        // {
        //     // Debug.Log("你撞到人了 乾！");
        //     AddReward(-10f);
        //     // EndEpisode();
        // }
        if(col.gameObject.tag == "Goal")
        {
            // Debug.Log("到達目的！");
            n_threshold += 1;
        }
    }

#pragma warning disable CS0246 // 找不到類型或命名空間名稱 'ActionBuffers' (是否遺漏了 using 指示詞或組件參考?)
    public override void OnActionReceived(ActionBuffers actionBuffers)
#pragma warning restore CS0246 // 找不到類型或命名空間名稱 'ActionBuffers' (是否遺漏了 using 指示詞或組件參考?)
    {
        // Control Agent
        ////////////////////////////////////////
        float linear = MaxLinearVelocity * Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);
        float angular = MaxAngularVelocity * Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);
        
        rBody.velocity = transform.forward * linear;
        float rot = 360 * angular * Time.fixedDeltaTime;
        Vector3 rotation = new Vector3(0f, rot, 0f);
        transform.Rotate(rotation);
        ////////////////////////////////////////
        float distanceToTarget = Vector3.Distance(gameObject.transform.localPosition, target.transform.localPosition);
        // Reward
        ////////////////////////////////////////
        //Arrived goal
        if (n_threshold <= 4)
        {
            if (distanceToTarget > 1.3f * startDistance)
            {
                // Just design for short distance obstacle avoidance.
                AddReward(-2f);
                EndEpisode();
            }

            if (this.StepCount == this.MaxStep)
            {
                AddReward(-5f);
            }
            else
            {
                // Debug.Log("step reward : "+ -10f*this.StepCount/this.MaxStep);
                // AddReward(-10f*this.StepCount/this.MaxStep);
            }
        }

        if (n_threshold > 4)
        {
            // AddReward(10f);
            AddReward(10f-(20f*this.StepCount/this.MaxStep));
            if (_stage == 15)
                _stage = 0;
            else
            {
                _stage += 1;
            }

            _lastGoal = target.transform.localPosition;
            EndEpisode();
        }
        // Falling Detect.
        if (gameObject.transform.localPosition.x < -28f || gameObject.transform.localPosition.x > 27.8f
                                                        || gameObject.transform.localPosition.z < -26.8f
                                                        ||gameObject.transform.localPosition.z > 28.8f)
        {
            AddReward(-10f);
            EndEpisode();
        }
    }
    
    //a is target, b is agent.
    public float GetAngle(Transform a, Transform b)
    {
        Vector3 targetDir = a.position - b.position;
        float angle = Vector3.Angle(targetDir, b.forward);

        return angle;
    }
    
    public Vector3 spownPos(Vector2 a, Vector2 b)
    {
        float _x_delta = Mathf.Abs(a.x - b.x);
        float _z_delta = Mathf.Abs(a.y - b.y);
        float _x_min = a.x;
        if (b.x < a.x)
            _x_min = b.x;
        float _z_min = a.y;
        if (b.y < a.y)
            _z_min = b.y;
        Vector3 _pos = new Vector3(Random.value * _x_delta + _x_min, 0.5f, Random.value * _z_delta + _z_min);
        while (Physics.CheckSphere(_pos, 1.5f, unwalkableMask))
        {
            _pos = new Vector3(Random.value * _x_delta + _x_min, 0.5f, Random.value * _z_delta + _z_min);
        }

        return _pos;
    }

    public int hierarchicalModeSwitch()
    {
        Vector2 goalPos = new Vector2(target.transform.localPosition.x, target.transform.localPosition.z);
        Vector2 agebtPos = new Vector2(transform.localPosition.x, transform.localPosition.z);
        int actionNum = 0;  // action : [goStraight, passThrought, escape]
        this.h_text.text = "GoStraight";
        // Area1
        if (agebtPos.x > 16.5f && agebtPos.x < 28.5f && agebtPos.y > -1.7f && agebtPos.y < 9.9f)
        {
            actionNum = 1;
            this.h_text.text = "PassThrough";
            // Debug.Log("Area1");
        }
        // Area3
        if (agebtPos.x > 15.8f && agebtPos.x < 22.5f && agebtPos.y > -21.8f && agebtPos.y < -13.1f)
        {
            actionNum = 1;
            this.h_text.text = "PassThrough";
            // Debug.Log("Area3");
        }
        // Area5
        if (agebtPos.x > -24.9f && agebtPos.x < -19.2f && agebtPos.y > -23.3f && agebtPos.y < -19.1f)
        {
            actionNum = 2;
            this.h_text.text = "Escape";
            // Debug.Log("Area5");
        }
        // Area7
        if (agebtPos.x > -22.2f && agebtPos.x < 2.3f && agebtPos.y > 7.9f && agebtPos.y < 26.2f)
        {
            actionNum = 1;
            this.h_text.text = "PassThrough";
            // Debug.Log("Area7");
        }
        // Area8
        if (agebtPos.x > -15.7f && agebtPos.x < -5.3f && agebtPos.y > 14f && agebtPos.y < 21.7f)
        {
            actionNum = 0;
            this.h_text.text = "GoStraight";
            // Debug.Log("Area8");
        }
        // Area10
        if (agebtPos.x > 13f && agebtPos.x < 21.3f && agebtPos.y > 10.9f && agebtPos.y < 25.8f)
        {
            actionNum = 1;
            this.h_text.text = "PassThrough";
            // Debug.Log("Area10");
        }
        // Area11
        if (agebtPos.x > -17.1f && agebtPos.x < -0.9f && agebtPos.y > -9.8f && agebtPos.y < -3f)
        {
            actionNum = 1;
            this.h_text.text = "PassThrough";
            // Debug.Log("Area11");
        }
        // Area12
        if (agebtPos.x > -9.4f && agebtPos.x < -1.5f && agebtPos.y > 0.5f && agebtPos.y < 7.9f)
        {
            actionNum = 1;
            this.h_text.text = "PassThrough";
            // Debug.Log("Area12");
        }
        // Area13
        if (agebtPos.x > 0.9f && agebtPos.x < 16.1f && agebtPos.y > 0.3f && agebtPos.y < 7.8f)
        {
            actionNum = 1;
            this.h_text.text = "PassThrough";
            // Debug.Log("Area13");
        }
        // Area14
        if (agebtPos.x > -14.8f && agebtPos.x < -9.8f && agebtPos.y > 0.5f && agebtPos.y < 7.8f)
        {
            actionNum = 2;
            this.h_text.text = "Escape";
            // Debug.Log("Area14");
        }
        return actionNum;
    }

#pragma warning disable CS0246 // 找不到類型或命名空間名稱 'ActionBuffers' (是否遺漏了 using 指示詞或組件參考?)
    public override void Heuristic(in ActionBuffers actionsOut)
#pragma warning restore CS0246 // 找不到類型或命名空間名稱 'ActionBuffers' (是否遺漏了 using 指示詞或組件參考?)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;

        if (joyStick)
        {
            float _joy_x = Input.GetAxis("Horizontal_Left");
            float _joy_y = Input.GetAxis("Vertical_Left");
            
            continuousActionsOut[0] = _joy_y;
            continuousActionsOut[1] = _joy_x;
        }
        else
        {
            if(Input.GetKey(KeyCode.W))
            {
                continuousActionsOut[0] = 0.5f;
            }
            else if(Input.GetKey(KeyCode.X))
            {
                continuousActionsOut[0] = -0.5f;
            }
            else if(Input.GetKey(KeyCode.A))
            {
                continuousActionsOut[1] = -0.5f;
            }
            else if(Input.GetKey(KeyCode.D))
            {
                continuousActionsOut[1] = 0.5f;
            }
            else
            {
                continuousActionsOut[0] = 0f;
                continuousActionsOut[1] = 0f;
            }
        }
    }
}
