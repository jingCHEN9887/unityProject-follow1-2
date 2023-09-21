using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.MLAgents;
#pragma warning disable CS0234 // 命名空間 'Unity.MLAgents' 中沒有類型或命名空間名稱 'Actuators' (是否遺漏了組件參考?)
using Unity.MLAgents.Actuators;
#pragma warning restore CS0234 // 命名空間 'Unity.MLAgents' 中沒有類型或命名空間名稱 'Actuators' (是否遺漏了組件參考?)
#pragma warning disable CS0234 // 命名空間 'Unity.MLAgents' 中沒有類型或命名空間名稱 'Sensors' (是否遺漏了組件參考?)
using Unity.MLAgents.Sensors;
#pragma warning restore CS0234 // 命名空間 'Unity.MLAgents' 中沒有類型或命名空間名稱 'Sensors' (是否遺漏了組件參考?)
using Random = UnityEngine.Random;

#pragma warning disable CS0246 // 找不到類型或命名空間名稱 'Agent' (是否遺漏了 using 指示詞或組件參考?)
public class MallKddbot : Agent
#pragma warning restore CS0246 // 找不到類型或命名空間名稱 'Agent' (是否遺漏了 using 指示詞或組件參考?)
{
    //Define gameobject goal
    public GameObject target;
    [Range(0f, 100f)]
    public float MaxLinearVelocity;
    [Range(0.1f, 360f)]
    public float MaxAngularVelocity;

    public enum GameMode
    {
        PedestrianPassThrough,
        Escape,
        GoStraight,
        Combine
    }
    public GameMode modeSwitch;
    public LayerMask unwalkableMask;
    public bool joyStick;

    Rigidbody rBody;
    private int n_threshold;
    // Last Agent & Target distance
    private float last_linearAction;
    private float last_angularAction;
    private float initialDistanceToTarget;
    private int _stage;

#pragma warning disable CS0115 // 'MallKddbot.Initialize()': 未找到任何合適的方法可覆寫
    public override void Initialize()
#pragma warning restore CS0115 // 'MallKddbot.Initialize()': 未找到任何合適的方法可覆寫
    {
        rBody = GetComponent<Rigidbody>();
        _stage = 0;
    }

#pragma warning disable CS0115 // 'MallKddbot.OnEpisodeBegin()': 未找到任何合適的方法可覆寫
    public override void OnEpisodeBegin()
#pragma warning restore CS0115 // 'MallKddbot.OnEpisodeBegin()': 未找到任何合適的方法可覆寫
    {
        //set velocity to zero
        rBody.velocity = transform.forward * 0f;
        transform.Rotate(transform.up, Time.fixedDeltaTime * 0f);
        
        switch (modeSwitch)
        {
            case GameMode.PedestrianPassThrough:
                // Initial Agent & goal pos
                switch (_stage)
                {
                    case 0:
                        // Agent Pos
                        transform.localPosition = new Vector3(26f,0.5f,2f);
                        transform.localEulerAngles = new Vector3 (0.0f,Random.value * 120f - 150f,0.0f);    // [-150~-30]
                        // Goal Pos
                        target.transform.localPosition = new Vector3(21.6f, 0.5f, 3.2f);
                        break;
                    case 1:
                        // Agent Pos
                        transform.localPosition = new Vector3(21.6f,0.5f,3.2f);
                        transform.localEulerAngles = new Vector3 (0.0f,Random.value * 360f,0.0f);    // [0~360]
                        // Goal Pos
                        target.transform.localPosition = new Vector3(Random.value * 6.2f + 18.6f, 0.5f, -3.7f); // [18.6~24.8]
                        break;
                    case 2:
                        // Agent Pos
                        transform.localPosition = new Vector3(21.6f,0.5f,3.2f);
                        transform.localEulerAngles = new Vector3 (0.0f,Random.value * 360f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(16.3f, 0.5f, Random.value * 6.7f + 0.8f);  // [0.8~7.5]
                        break;
                    case 3:
                        // Agent Pos
                        transform.localPosition = new Vector3(23.7f,0.5f,Random.value * 8.4f - 21.6f);   // [-13.2~-21.6]
                        transform.localEulerAngles = new Vector3 (0.0f,Random.value * 60f + 240f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(14.7f, 0.5f, Random.value * 8f - 21.6f);
                        break;
                    case 4:
                        // Agent Pos
                        transform.localPosition = new Vector3(Random.value * 4.9f - 22f,0.5f,9.5f);
                        transform.localEulerAngles = new Vector3 (0.0f,Random.value * 60f - 30f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(Random.value * 5.5f - 19.9f, 0.5f, 14.3f);
                        break;
                    case 5:
                        // Agent Pos
                        transform.localPosition = new Vector3(-6f,0.5f,Random.value * 6.8f + 14.4f);
                        transform.localEulerAngles = new Vector3 (0.0f,Random.value * 60f + 60f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(-0.2f, 0.5f, Random.value * 11.7f + 12.5f);
                        break;
                    case 6:
                        // Agent Pos
                        transform.localPosition = new Vector3(13.3f,0.5f,Random.value * 13f + 11.5f);
                        transform.localEulerAngles = new Vector3 (0.0f,Random.value * 60f + 60f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(21.8f, 0.5f, Random.value * 13.4f + 11.2f);
                        break;
                    case 7:
                        // Agent Pos
                        transform.localPosition = new Vector3(Random.value * 6.3f + 18.1f,0.5f,8.7f);
                        transform.localEulerAngles = new Vector3 (0.0f,Random.value * 60f + 150f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(21.6f, 0.5f, 3.2f);
                        break;
                }
                break;
            case GameMode.Escape:
                // Test2 (Escape)
                // Initial Agent pos
                transform.localPosition = new Vector3(Random.value * 2.4f - 24f, 0.5f, Random.value * 1.8f - 22.3f);
                transform.localEulerAngles = new Vector3(0.0f, Random.value * 360f - 180f, 0.0f);   //random rotation(-180,180)
                // Initial Goal pos
                target.transform.localPosition = new Vector3(-18.9f, 0.5f, -17.7f);
                break;
            case GameMode.GoStraight:
                // Initial Agent & Goal pos
                switch (_stage)
                {
                    case 0:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(-21.9f,-9.6f),new Vector2(24.1f,-3.5f));
                        transform.localEulerAngles = new Vector3(0.0f,Random.value * 360f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(-21.9f,-9.6f),new Vector2(24.1f,-3.5f));
                        break;
                    case 1:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(22.2f,-21.6f),new Vector2(24.3f,-3.2f));
                        transform.localEulerAngles = new Vector3(0.0f,Random.value * 360f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(22.2f,-21.6f),new Vector2(24.3f,-3.2f));
                        break;
                    case 2:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(-17.6f,-13.9f),new Vector2(16.3f,-21.5f));
                        transform.localEulerAngles = new Vector3(0.0f,Random.value * 360f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(-17.6f,-13.9f),new Vector2(16.3f,-21.5f));
                        break;
                    case 3:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(-21.9f,7.5f),new Vector2(15.6f,0.9f));
                        transform.localEulerAngles = new Vector3(0.0f,Random.value * 360f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(-21.9f,7.5f),new Vector2(15.6f,0.9f));
                        break;
                    case 4:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(-21.9f,-17.8f),new Vector2(-17.5f,9.2f));
                        transform.localEulerAngles = new Vector3(0.0f,Random.value * 360f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(-21.9f,-17.8f),new Vector2(-17.5f,9.2f));
                        break;
                    case 5:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(0f,12f),new Vector2(13.2f,24.6f));
                        transform.localEulerAngles = new Vector3(0.0f,Random.value * 360f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(0f,12f),new Vector2(13.2f,24.6f));
                        break;
                    case 6:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(21f,9.1f),new Vector2(24.6f,25f));
                        transform.localEulerAngles = new Vector3(0.0f,Random.value * 360f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(21f,9.1f),new Vector2(24.6f,25f));
                        break;
                    case 7:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(-18.7f,21.5f),new Vector2(-5.7f,13.9f));
                        transform.localEulerAngles = new Vector3(0.0f,Random.value * 360f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(-18.7f,21.5f),new Vector2(-5.7f,13.9f));
                        break;
                }
                break;
            case GameMode.Combine:
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
                        transform.localPosition = new Vector3(Random.value * 4.9f + 18.5f, 0.5f, Random.value * 2.1f - 5.9f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(Random.value * 2.9f + 22f, 0.5f, Random.value * 9.2f - 22f);
                        break;
                    case 2: 
                        // Agent keep origin Pos
                        transform.localPosition = new Vector3(Random.value * 2.9f + 22f, 0.5f, Random.value * 9.2f - 22f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(Random.value * 2.4f + 13.1f, 0.5f, Random.value * 5.9f - 21.1f);
                        break;
                    case 3:
                        // Agent keep origin Pos
                        transform.localPosition = new Vector3(Random.value * 2.4f + 13.1f, 0.5f, Random.value * 5.9f - 21.1f);
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
                        transform.localPosition = new Vector3(Random.value * 2.9f - 21f, 0.5f, Random.value * 4.9f - 16.1f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(Random.value * 3.2f - 21.3f, 0.5f, Random.value * 5.1f + 3.9f);
                        break;
                    case 6:
                        // Agent keep origin Pos
                        transform.localPosition = new Vector3(Random.value * 3.2f - 21.3f, 0.5f, Random.value * 5.1f + 3.9f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(Random.value * 5.2f - 17.5f, 0.5f, Random.value * 4.7f + 15.1f);
                        break;
                    case 7:
                        // Agent keep origin Pos
                        transform.localPosition = new Vector3(Random.value * 5.2f - 17.5f, 0.5f, Random.value * 4.7f + 15.1f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(Random.value * 3.1f - 6.8f, 0.5f, Random.value * 3.7f + 17.5f);
                        break;
                    case 8:
                        // Agent keep origin Pos
                        transform.localPosition = new Vector3(Random.value * 3.1f - 6.8f, 0.5f, Random.value * 3.7f + 17.5f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(Random.value * 4.4f + 2.1f, 0.5f, Random.value * 8f + 14.1f);
                        break;
                    case 9:
                        // Agent keep origin Pos
                        transform.localPosition = new Vector3(Random.value * 4.4f + 2.1f, 0.5f, Random.value * 8f + 14.1f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(Random.value * 2.1f + 12.2f, 0.5f, Random.value * 7.9f + 14.2f);
                        break;
                    case 10:
                        // Agent keep origin Pos
                        transform.localPosition = new Vector3(Random.value * 2.1f + 12.2f, 0.5f, Random.value * 7.9f + 14.2f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(Random.value * 3.6f + 21.2f, 0.5f, Random.value * 6.1f + 9.7f);
                        break;
                    case 11:
                        // Agent keep origin Pos
                        transform.localPosition = new Vector3(Random.value * 3.6f + 21.2f, 0.5f, Random.value * 6.1f + 9.7f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(21.6f,0.5f,2.9f);  // go back to start point
                        break;
                    case 12:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(-16.1f,-9.6f), new Vector2(17.5f, -3.1f));
                        transform.localEulerAngles = new Vector3(0.0f,Random.value * 360f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(-16.1f,-9.6f), new Vector2(17.5f, -3.1f));
                        break;
                    case 13:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(-16.1f,0.8f), new Vector2(17.1f, 7.5f));
                        transform.localEulerAngles = new Vector3(0.0f,Random.value * 360f,0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(-16.1f,0.8f), new Vector2(17.1f, 7.5f));
                        break;
                }
                break;
        }

        //Random.value will create a value between [0,1]
        //normalization position region to [0,1] can use [(X-Xmin)/(Xmax - Xmin)]
        //X = Xnom*(Xmax - Xmin) + Xmin

        // Whole Area3 Example.
        // Vector3 finalTarget = new Vector3(Random.value * 44.4f - 45.6f, 0.5f, Random.value * 48.6f - 46.9f);
        // while (Physics.CheckSphere(finalTarget, 1.5f, unwalkableMask))
        // {
        //     finalTarget = new Vector3(Random.value * 44.4f - 45.6f, 0.5f, Random.value * 48.6f - 46.9f);
        // }
        initialDistanceToTarget = Vector3.Distance(target.transform.position, gameObject.transform.position);
        this.n_threshold = 0;

        this.last_linearAction = 0.0f;
        this.last_angularAction = 0.0f;
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
    }

    void OnCollisionStay(Collision col)
    {
        if(col.gameObject.tag == "Wall")
        {
            // Debug.Log("你撞到牆了");
            AddReward(-0.1f);
        }

        if(col.gameObject.tag == "Obstacle")
        {
            // Debug.Log("你撞到障礙物了");
            AddReward(-0.1f);
        }
        if(col.gameObject.tag == "Goal")
        {
            // Debug.Log("撞到目標");
            n_threshold += 1;
        }

        if(col.gameObject.tag == "People")
        {
            // Debug.Log("你撞到人了 乾！");
            AddReward(-5f);
            // EndEpisode();
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
        
        rBody.velocity = transform.forward * linear;    // m/sec
        float rot = angular * Time.fixedDeltaTime;      // degree/sec
        Vector3 rotation = new Vector3(0f, rot, 0f);
        transform.Rotate(rotation);
        ////////////////////////////////////////
        
        // Reward
        ////////////////////////////////////////
        float distanceToTarget = Vector3.Distance(gameObject.transform.localPosition, target.transform.localPosition);
        float angleToTarget = GetAngle(target.transform, gameObject.transform);

        switch (modeSwitch)
        {
            case GameMode.PedestrianPassThrough:
                // action reward
                if (linear < 0) //hope not to backward too fast and often.
                {
                    AddReward(0.01f * linear);
                }
                // Distance Reward
                if (n_threshold <= 4)
                {
                    // restrict agent not to detour too far.
                    if (distanceToTarget > 2*initialDistanceToTarget)
                    {
                        float delta = distanceToTarget - initialDistanceToTarget;
                        AddReward(-0.01f * delta);
                    }
                    // encourage agent get closer to target
                    AddReward(Mathf.Pow(10, -3) * (1/distanceToTarget-1));
                }
                //Arrived goal
                if (n_threshold > 4)
                {
                    AddReward(10f);
                    if (_stage == 7)
                        _stage = 0;
                    else
                    {
                        _stage += 1;
                    }
                    EndEpisode();
                }
                break;
            case GameMode.Escape:
                // action reward (11/1)
                if (n_threshold <= 4)
                {
                    // restrict not to turn left and right or forward and backward
                    if (last_linearAction < 0f && linear > 0 || last_linearAction > 0 && linear < 0
                                                             || last_angularAction < 0f && angular > 0
                                                             || last_angularAction > 0 && angular < 0)
                    {
                        AddReward(-0.1f);
                    }
                    // time reward
                    AddReward(-0.005f);
                }
                if (n_threshold > 4)
                {
                    AddReward(5f);
                    EndEpisode();
                }
                break;
            case GameMode.GoStraight:
                // Distance reward
                if (n_threshold <= 4)
                {
                    AddReward(Mathf.Pow(10, -2) * (1/distanceToTarget-1));
                    
                    // Angle Reward (2021/11/4 added)
                    if (angleToTarget < 3f)
                    {
                        AddReward(Mathf.Pow(10, -2) * (3-angleToTarget));
                    }
                }
                //Arrived goal
                if (n_threshold > 4)
                {

                    AddReward(5f);
                    if (_stage == 7)
                        _stage = 0;
                    else
                    {
                        _stage += 1;
                    }
                    EndEpisode();
                }
                break;
            case GameMode.Combine:
                // Before arrived goal
                if (n_threshold <= 4)
                {
                    // restrict agent not to detour too far.
                    if (distanceToTarget > 2*initialDistanceToTarget)
                    {
                        float delta = distanceToTarget - initialDistanceToTarget;
                        AddReward(-0.01f * delta);
                    }

                    // encourage agent get closer to target
                    AddReward(Mathf.Pow(10, -3) * (1 / distanceToTarget - 1));
                }
                //Arrived goal
                if (n_threshold > 4)
                {
                    AddReward(10f);
                    if (_stage == 13)
                        _stage = 0;
                    else
                    {
                        _stage += 1;
                    }
                    EndEpisode();
                }
                break;
        }
        ////////////////////////////////////////
        this.last_linearAction = linear;
        this.last_angularAction = angular;
        ////////////////////////////////////////
        // Falling Detect.
        if (gameObject.transform.localPosition.x < -28f || gameObject.transform.localPosition.x > 27.8f
                                                        || gameObject.transform.localPosition.z < -26.8f
                                                        ||gameObject.transform.localPosition.z > 28.8f)
        {
            AddReward(-3f);
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
                continuousActionsOut[0] = 1.0f;
            }
            else if(Input.GetKey(KeyCode.S))
            {
                continuousActionsOut[0] = -1.0f;
            }
            else if(Input.GetKey(KeyCode.A))
            {
                continuousActionsOut[1] = -1.0f;
            }
            else if(Input.GetKey(KeyCode.D))
            {
                continuousActionsOut[1] = 1.0f;
            }
            else
            {
                continuousActionsOut[0] = 0f;
                continuousActionsOut[1] = 0f;
            }
        }
    }

}