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
public class combineAgent : Agent
#pragma warning restore CS0246 // 找不到類型或命名空間名稱 'Agent' (是否遺漏了 using 指示詞或組件參考?)
{
    //Define gameobject goal
    public GameObject target;//V  
    [Range(0f, 100f)]
    public float MaxLinearVelocity;
    [Range(0.1f, 1.0f)]
    public float MaxAngularVelocity;

    public enum GameMode //V 
    {
        PedestrianPassThrough,//V 
        Escape,//V 
        GoStraight//V 
    }
    public GameMode modeSwitch;//V 
    public LayerMask unwalkableMask;
    
    Rigidbody rBody;//V
    private int n_threshold;//V
    private float linearDistanceToTarget;//V
    private int _stage;//V

#pragma warning disable CS0115 // 'combineAgent.Initialize()': 未找到任何合適的方法可覆寫
    public override void Initialize()//V
#pragma warning restore CS0115 // 'combineAgent.Initialize()': 未找到任何合適的方法可覆寫
    {
        rBody = GetComponent<Rigidbody>();//V
        _stage = 0;//V
    }

#pragma warning disable CS0115 // 'combineAgent.OnEpisodeBegin()': 未找到任何合適的方法可覆寫
    public override void OnEpisodeBegin()
#pragma warning restore CS0115 // 'combineAgent.OnEpisodeBegin()': 未找到任何合適的方法可覆寫
    {
        //set velocity to zero
        rBody.velocity = transform.forward * 0f;
        transform.Rotate(transform.up, Time.fixedDeltaTime * 0f);

        switch (modeSwitch)//V{42~179}
        {
            case GameMode.PedestrianPassThrough:
                // Initial Agent & goal pos
                switch (_stage)
                {
                    case 0:
                        // Agent Pos
                        transform.localPosition = new Vector3(26f, 0.5f, 2f);
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 120f - 150f, 0.0f); // [-150~-30]
                        // Goal Pos
                        target.transform.localPosition = new Vector3(21.6f, 0.5f, 3.2f);
                        break;
                    case 1:
                        // Agent Pos
                        transform.localPosition = new Vector3(21.6f, 0.5f, 3.2f);
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 360f, 0.0f); // [0~360]
                        // Goal Pos
                        target.transform.localPosition =
                            new Vector3(Random.value * 6.2f + 18.6f, 0.5f, -3.7f); // [18.6~24.8]
                        break;
                    case 2:
                        // Agent Pos
                        transform.localPosition = new Vector3(21.6f, 0.5f, 3.2f);
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 360f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition =
                            new Vector3(16.3f, 0.5f, Random.value * 6.7f + 0.8f); // [0.8~7.5]
                        break;
                    case 3:
                        // Agent Pos
                        transform.localPosition =
                            new Vector3(23.7f, 0.5f, Random.value * 8.4f - 21.6f); // [-13.2~-21.6]
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 60f + 240f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(14.7f, 0.5f, Random.value * 8f - 21.6f);
                        break;
                    case 4:
                        // Agent Pos
                        transform.localPosition = new Vector3(Random.value * 4.9f - 22f, 0.5f, 9.5f);
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 60f - 30f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(Random.value * 5.5f - 19.9f, 0.5f, 14.3f);
                        break;
                    case 5:
                        // Agent Pos
                        transform.localPosition = new Vector3(-6f, 0.5f, Random.value * 6.8f + 14.4f);
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 60f + 60f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(-0.2f, 0.5f, Random.value * 11.7f + 12.5f);
                        break;
                    case 6:
                        // Agent Pos
                        transform.localPosition = new Vector3(13.3f, 0.5f, Random.value * 13f + 11.5f);
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 60f + 60f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(21.8f, 0.5f, Random.value * 13.4f + 11.2f);
                        break;
                    case 7:
                        // Agent Pos
                        transform.localPosition = new Vector3(Random.value * 6.3f + 18.1f, 0.5f, 8.7f);
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 60f + 150f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(21.6f, 0.5f, 3.2f);
                        break;
                }
                break;
            case GameMode.Escape:
                // Test2 (Escape)
                // Initial Agent pos
                transform.localPosition = new Vector3(Random.value * 2.4f - 24f, 0.5f, Random.value * 1.8f - 22.3f);
                transform.localEulerAngles = new Vector3(0.0f, Random.value * 360f - 180f, 0.0f); //random rotation(-180,180)
                // Initial Goal pos
                target.transform.localPosition = new Vector3(-18.9f, 0.5f, -17.7f);
                break;
            case GameMode.GoStraight:
                // Initial Agent & Goal pos
                switch (_stage)
                {
                    case 0:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(-21.9f, -9.6f), new Vector2(24.1f, -3.5f));
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 360f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(-21.9f, -9.6f), new Vector2(24.1f, -3.5f));
                        break;
                    case 1:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(22.2f, -21.6f), new Vector2(24.3f, -3.2f));
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 360f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(22.2f, -21.6f), new Vector2(24.3f, -3.2f));
                        break;
                    case 2:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(-17.6f, -13.9f), new Vector2(16.3f, -21.5f));
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 360f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(-17.6f, -13.9f), new Vector2(16.3f, -21.5f));
                        break;
                    case 3:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(-21.9f, 7.5f), new Vector2(15.6f, 0.9f));
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 360f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(-21.9f, 7.5f), new Vector2(15.6f, 0.9f));
                        break;
                    case 4:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(-21.9f, -17.8f), new Vector2(-17.5f, 9.2f));
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 360f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(-21.9f, -17.8f), new Vector2(-17.5f, 9.2f));
                        break;
                    case 5:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(0f, 12f), new Vector2(13.2f, 24.6f));
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 360f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(0f, 12f), new Vector2(13.2f, 24.6f));
                        break;
                    case 6:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(21f, 9.1f), new Vector2(24.6f, 25f));
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 360f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(21f, 9.1f), new Vector2(24.6f, 25f));
                        break;
                    case 7:
                        // Agent Pos
                        transform.localPosition = spownPos(new Vector2(-18.7f, 21.5f), new Vector2(-5.7f, 13.9f));
                        transform.localEulerAngles = new Vector3(0.0f, Random.value * 360f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = spownPos(new Vector2(-18.7f, 21.5f), new Vector2(-5.7f, 13.9f));
                        break;//V
                }
                break;
        }//V
        this.n_threshold = 0;//V
        // Caculate with final goal
        linearDistanceToTarget = Vector3.Distance(target.transform.position, gameObject.transform.position);
    }
#pragma warning disable CS0246 // 找不到類型或命名空間名稱 'VectorSensor' (是否遺漏了 using 指示詞或組件參考?)
    public override void CollectObservations(VectorSensor sensor)//V{184~196}
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
    void OnCollisionStay(Collision col)//V{197~222}
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

        rBody.velocity = transform.forward * linear;
        Debug.Log("Agent Velocity :" + rBody.velocity.ToString("0.0"));
        float rot = 360 * angular * Time.fixedDeltaTime;
        Vector3 rotation = new Vector3(0f, rot, 0f);
        transform.Rotate(rotation);
        ////////////////////////////////////////
        // Reward
        ////////////////////////////////////////
        float distanceToTarget = new float();//V
        distanceToTarget = Vector3.Distance(gameObject.transform.localPosition, target.transform.localPosition);//V

        // Before arrived goal
        if (n_threshold <= 4)//V
        {
            // restrict agent not to detour too far.
            if (distanceToTarget > 2*linearDistanceToTarget)//V
            {
                float delta = distanceToTarget - linearDistanceToTarget;//V
                AddReward(-0.01f * delta);//V
            }

            // encourage agent get closer to target
            AddReward(Mathf.Pow(10, -3) * (1 / distanceToTarget - 1));//V
        }
        //Arrived goal
        if (n_threshold > 4)//V
        {
            AddReward(10f);//V
            switch (modeSwitch)//V
            {
                case GameMode.GoStraight://V
                    if (_stage == 7)//V
                        _stage = 0;//V
                    else
                    {
                        _stage += 1;//V
                    }
                    break;//V
                case GameMode.PedestrianPassThrough://V
                    if (_stage == 7)//V
                        _stage = 0;//V
                    else//V
                    {
                        _stage += 1;//V
                    }
                    break;//V
                case GameMode.Escape://V
                    break;//V
            }
            EndEpisode();//V
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
    public float GetAngle(Transform a, Transform b)//V{}
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
    public override void Heuristic(in ActionBuffers actionsOut)//V{}
#pragma warning restore CS0246 // 找不到類型或命名空間名稱 'ActionBuffers' (是否遺漏了 using 指示詞或組件參考?)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        
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
