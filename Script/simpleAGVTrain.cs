using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Integrations.Match3;
using UnityEngine.AI;
using System.Collections.Generic;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Google.Protobuf.WellKnownTypes;
//using System.IO;
using System;



//ctrl+f 230625, 230626, to do, 230703, 230704, 230826 old car contorl, 230903 reward cos similarity, 230904 reward time limit, 230915, 230918, 230918 show training info
//ctrl+f 230919
public class simpleAGVTrain : Agent
{
    public GameObject target;
    public bool joyStick;  // 是否使用操縱桿輸入來控制 AGV
    Rigidbody rBody;
    //private AGVWheelController AGVController;  //?對附加到 AGV 的 AGVWheelController 組件的[引用reference] 
    private int n_threshold;  //?跟踪某個閾值
    private float linearDistanceToTarget;    // 230626
    private int _stage;  // 230625
    [Range(0f, 100f)]// 230826
    public float MaxLinearVelocity;// 230826
    [Range(0.1f, 1.0f)]// 230826
    public float MaxAngularVelocity;// 230826
    private int _round;//230918 show training info
    private int _failCount;//230918 show training info
    private int _ArrivalCount;//230919
    int stepCountGreaterThan1000 = 0;//230918
    int stepCountBetween1000And1500 = 0;//230918
    int stepCountGreaterThan1500 = 0;//230918
    private int _colPeopleCount;//230919
    private int _colObstacleCount;//230919



    public enum GameMode
    {
        //PedestrianPassThrough,  // 230625
        //Escape,  // 230625
        //GoStraight  // 230625
        FollowScene1,  // 230625
        FollowScene2,  // 230625 
        FollowScene3,  // 230625
        FollowScene4,  // 230625
        test1, //230910
        test2 //230918
    }

    public GameMode modeSwitch;

    [SerializeField] GameObject[] movers; // 230703
    public override void Initialize()  // 在訓練開始時或代理重置時調用一次
    {
        rBody = GetComponent<Rigidbody>();
        //AGVController = GetComponent<AGVWheelController>();//230826
        _stage = 0;// 230625
        _round = 0;// 230918 show training info
        _failCount = 0;//230918 show training info
        _ArrivalCount = 0;//230919
        stepCountGreaterThan1000 = 0;//230918計數器重置為0
        stepCountBetween1000And1500 = 0;//230918
        stepCountGreaterThan1500 = 0;//230918
        //_colPeopleCount = 0;//230919
        //_colObstacleCount = 0;//230919


        movers = new GameObject[movers.Length]; // 230703
        movers = GameObject.FindGameObjectsWithTag("People"); // 碰撞設定要改 230703
    }

    public override void OnEpisodeBegin()
    {
        //AGVController.GetAction(0.0f, 0.0f);// 必須加入此行才會正式將速度與角速度值賦予AGV. 230826
        //set velocity to zero
        rBody.velocity = transform.forward * 0f; //230826
        transform.Rotate(transform.up, Time.fixedDeltaTime * 0f);//230826

        // 下方switch 230325
        switch (modeSwitch)
        {
            case GameMode.FollowScene1:
                switch (_stage)
                {
                    case 0:
                        // Agent Pos
                        transform.localPosition = new Vector3(UnityEngine.Random.value * 2f - 72.5f, 0.5f, UnityEngine.Random.value * 7f - 25f);
                        transform.localEulerAngles = new Vector3(0.0f, UnityEngine.Random.value * 120f - 150f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(UnityEngine.Random.value * 2f - 50.5f, 0.5f, UnityEngine.Random.value * 6f - 28f);
                        break;
                    case 1:
                        // Agent Pos
                        transform.localPosition = new Vector3(UnityEngine.Random.value * 2f - 72.5f, 0.5f, UnityEngine.Random.value * 7f - 25f);  // Random.value = [0, 1)
                        transform.localEulerAngles = new Vector3(0.0f, UnityEngine.Random.value * 60f + 150f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(UnityEngine.Random.value * 2f - 53f, 0.5f, UnityEngine.Random.value * 3f - 18f);
                        break;
                }
                break;

            case GameMode.FollowScene2:
                switch (_stage)
                {
                    case 0:
                        // Agent Pos
                        transform.localPosition = new Vector3(UnityEngine.Random.value * 4.5f - 31.5f, 0.5f, UnityEngine.Random.value * 2.5f - 16.5f);
                        transform.localEulerAngles = new Vector3(0.0f, UnityEngine.Random.value * 120f - 150f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(-3f, 0.5f, UnityEngine.Random.value * 3.5f - 15f);
                        break;
                    case 1:
                        // Agent Pos
                        transform.localPosition = new Vector3(-29f, 0.5f, UnityEngine.Random.value * 5f - 28f);  // Random.value = [0, 1)
                        transform.localEulerAngles = new Vector3(0.0f, UnityEngine.Random.value * 60f + 150f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(-3f, 0.5f, -25.5f);
                        break;
                }
                break;

            case GameMode.FollowScene3:
                switch (_stage)
                {
                    case 0:
                        // Agent Pos
                        transform.localPosition = new Vector3(UnityEngine.Random.value * 2f + 4.5f, 0.5f, UnityEngine.Random.value * 8f - 25f);
                        transform.localEulerAngles = new Vector3(0.0f, UnityEngine.Random.value * 120f - 150f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(UnityEngine.Random.value * 3f + 27f, 0.5f, UnityEngine.Random.value * 3f - 15f);
                        break;
                    case 1:
                        // Agent Pos
                        transform.localPosition = new Vector3(UnityEngine.Random.value * 2f + 4.5f, 0.5f, UnityEngine.Random.value * 8f - 25f);
                        transform.localEulerAngles = new Vector3(0.0f, UnityEngine.Random.value * 60f + 150f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(UnityEngine.Random.value * 2f + 29f, 0.5f, UnityEngine.Random.value * 3f - 30f);
                        break;
                }
                break;

            case GameMode.FollowScene4:
                // Agent Pos
                transform.localPosition = new Vector3(41f, 0.5f, UnityEngine.Random.value * 2.5f - 14f);
                transform.localEulerAngles = new Vector3(0.0f, UnityEngine.Random.value * 120f - 150f, 0.0f);
                // Goal Pos
                target.transform.localPosition = new Vector3(UnityEngine.Random.value * 2.5f + 68.5f, 0.5f, UnityEngine.Random.value * 3.5f - 29f);
                break;

            case GameMode.test1:
                switch (_stage)
                {
                    case 0:
                        // Agent Pos
                        transform.localPosition = new Vector3(UnityEngine.Random.value * 4f - 70f, 0.5f, UnityEngine.Random.value * 1f - 6f);
                        transform.localEulerAngles = new Vector3(0.0f, UnityEngine.Random.value * 120f - 150f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(UnityEngine.Random.value * 3f - 60f, 0.5f, UnityEngine.Random.value * 3f - 2f);
                        break;
                    case 1:
                        // Initial Agent pos
                        transform.localPosition = new Vector3(UnityEngine.Random.value * 4f - 70f, 0.5f, UnityEngine.Random.value * 1f - 6f);
                        transform.localEulerAngles = new Vector3(0.0f, UnityEngine.Random.value * 360f - 180f, 0.0f); //random rotation(-180,180)
                        // Initial Goal pos
                        target.transform.localPosition = new Vector3(UnityEngine.Random.value * 3f - 55f, 0.5f, UnityEngine.Random.value * 5f + 2f);
                        break;
                }
                break;

            case GameMode.test2:
                switch (_stage)
                {
                    case 0:
                        // Agent Pos
                        transform.localPosition = new Vector3(UnityEngine.Random.value * 2f - 71f, 0.5f, UnityEngine.Random.value * 5f + 17f);
                        transform.localEulerAngles = new Vector3(0.0f, UnityEngine.Random.value * 120f - 150f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(UnityEngine.Random.value * 2f - 54f, 0.5f, UnityEngine.Random.value * 6f + 16f);
                        break;
                    case 1:
                        // Agent Pos
                        transform.localPosition = new Vector3(UnityEngine.Random.value * 2f - 71f, 0.5f, UnityEngine.Random.value * 5f + 17f);
                        transform.localEulerAngles = new Vector3(0.0f, UnityEngine.Random.value * 120f - 150f, 0.0f);
                        // Goal Pos
                        target.transform.localPosition = new Vector3(UnityEngine.Random.value * 1f - 54f, 0.5f, UnityEngine.Random.value * 1f + 18f);
                        break;
                }
                break;
        }
        this.n_threshold = 0;  // 230626
        this._colPeopleCount = 0;//230919
        this._colObstacleCount = 0;//230919
        // Caculate with final goal
        linearDistanceToTarget = Vector3.Distance(target.transform.position, gameObject.transform.position);  // 230626註解 230826不註解 
    }

    //private List<TransformAndDirection> moverTransforms = new List<TransformAndDirection>(); //230903
    //private class TransformAndDirection //230903
    //{// Define a class or struct to store the Transform and globalForwardDirection.
    //    public Transform transform; //230903
    //    public Vector3 globalForwardDirection; //230903
    //}


    public override void CollectObservations(VectorSensor sensor)
    {
        // Relationship between Agent and Target
        float distanceToTarget = Vector3.Distance(gameObject.transform.localPosition, target.transform.localPosition);
        float angleToTarget = GetAngle(target.transform, gameObject.transform);
        
        sensor.AddObservation(distanceToTarget);    // float var
        sensor.AddObservation(angleToTarget);       // float var
        sensor.AddObservation(rBody.velocity.x);      // float
        sensor.AddObservation(rBody.velocity.z);      // float
        sensor.AddObservation(transform.localEulerAngles.y);      // float

        //// SARL's input 
        //for (int i = 0; i < movers.Length; i++)
        //{
        //    if (movers[i] != null && movers[i].gameObject != null)
        //    {
        //        //Debug.Log("i:" + i + ", movers.Length:" + movers.Length);
        //        int xPos = (int)movers[i].transform.position.x;
        //        sensor.AddObservation(xPos);
        //        sensor.AddObservation((int)movers[i].transform.position.z);
        //        //Debug.Log("xPos:" + i + ":" + xPos);

        //        float distanceToMover = Vector3.Distance(gameObject.transform.position, movers[i].transform.position);
        //        sensor.AddObservation(distanceToMover);
        //        //Debug.Log("Distance between pedestrians and cars." + i + ": " + distanceToMover);

        //        //Vector3 forwardDirection = movers[i].transform.forward;//230903 获取Local朝向方向
        //        //Debug.Log("pedestrians forwardDirection." + i + ": " + forwardDirection);//230903
        //        Vector3 globalForwardDirection = movers[i].transform.TransformDirection(Vector3.forward);//230903 Global朝向方向              
        //        //Debug.Log("pedestrians globalForwardDirection." + i + ": " + globalForwardDirection);//230903
        //        //230903 Store the Transform and globalForwardDirection together.
        //        moverTransforms.Add(new TransformAndDirection
        //        { 
        //            transform = movers[i].transform,
        //            globalForwardDirection = globalForwardDirection
        //        });

        //        //sensor.AddObservation(forwardDirection); //添加朝向方向观测值到VectorSensor 20230717 TO DO 是不是取道無用的INFO?
        //        // 取行人ID-->不用傳給RL
        //        int instanceID = movers[i].GetInstanceID();
        //        //Debug.Log("Pedestrian ID." + i + ": " + instanceID);
        //        //sensor.AddObservation(instanceID); // 找info的時候用

        //        NavMeshAgent agent = movers[i].GetComponent<NavMeshAgent>();
        //        if (agent != null)
        //        {
        //            Vector3 velocity = agent.velocity; // Get the NavMeshAgent's velocity                      
        //            float xVelocity = Mathf.Round(velocity.x * 1000f) / 1000f; // Extract only the x and z components of the velocity
        //            float zVelocity = Mathf.Round(velocity.z * 1000f) / 1000f;
        //            sensor.AddObservation(xVelocity);
        //            sensor.AddObservation(zVelocity);
        //            //Debug.Log("X velocity for mover " + i + ": " + xVelocity);
        //            //Debug.Log("Z velocity for mover " + i + ": " + zVelocity);

        //            float headingAngle = Quaternion.LookRotation(agent.velocity).eulerAngles.y;
        //            sensor.AddObservation(headingAngle);
        //            //Debug.Log("Heading angle for mover " + i + ": " + headingAngle);

        //            //Vector3 nextPosition = agent.nextPosition; // 取行人下一步要到的位置
        //            //Debug.Log("Next position for mover " + i + ": " + nextPosition);
        //            //Vector3 destination = agent.destination; // 取行人的目标位置
        //            //Debug.Log("Destination for mover " + i + ": " + destination);
        //        }
        //    }
        //}
        //Transform objectTransform = transform; // 获取物体的Transform组件--> 是小車的id?

        //int objectInstanceID = GetInstanceID(); // 获取物体的InstanceID
        //sensor.AddObservation(objectInstanceID);
    }

    void OnCollisionEnter(Collision col)
    {// 230915
        //if (col.gameObject.tag == "Wall")
        //{
        //    //Debug.Log("Collided with a wall.");
        //    AddReward(-0.01f);
        //}
        if (col.gameObject.tag == "Obstacle")
        {           
            AddReward(-0.5f);
            _colObstacleCount += 1;//230919
            //Debug.Log("Collided with an obstacle." + _colObstacleCount);
        }
        if (col.gameObject.tag == "People")
        {
            AddReward(-2f);
            _colPeopleCount += 1;//230919
            Debug.Log("Collided with a person." + _colPeopleCount);
            //EndEpisode(); //230630 先註解掉 等之後訓練出一點動作再加入
        }
        if (col.gameObject.tag == "Goal")
        {
            //Debug.Log("Reached the destination.");
            AddReward(10f);
            //EndEpisode();  // 230915
            //Debug.Log("Arrival Goal.");
            n_threshold += 5; // 230915
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)  // 使用该方法的時機:当代理收到动作时

    {
        //float linear = Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);//230826
        //float angular = Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);//230826
        //AGVController.GetAction(linear, angular);//230826
        float linear = MaxLinearVelocity * Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);//230826
        float angular = MaxAngularVelocity * Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);//230826
        rBody.velocity = transform.forward * linear;//230826
        //Debug.Log("Agent Velocity :" + rBody.velocity.ToString("0.0"));//230826
        float rot = 360 * angular * Time.fixedDeltaTime;//230826
        Vector3 rotation = new Vector3(0f, rot, 0f);//230826
        transform.Rotate(rotation);//230826

        ////////////////Reward//////////////////
        float distanceToTarget = new float();  // Agent与目标的距离
        distanceToTarget = Vector3.Distance(gameObject.transform.localPosition, target.transform.localPosition);

        //// 230903 cos similarity
        //Vector3 vectorToTarget = target.transform.localPosition - gameObject.transform.localPosition; //230903the vector from car to target
        //Debug.Log("car vectorToTarget: " + vectorToTarget);//230903

        //float maxCosineSimilarity = float.MinValue;//230903
        ////Debug.Log("Initial Max Cosine Similarity: " + maxCosineSimilarity);//230903
        //int indexOfMaxCosineSimilarity = -1; //230903 Initialize to an invalid index
        //for (int i = 0; i < moverTransforms.Count; i++)//230903 Access the stored mover information here
        //{
        //    Transform moverTransform = moverTransforms[i].transform;//230903
        //    Vector3 globalForwardDirection = moverTransforms[i].globalForwardDirection;//230903
        //    Debug.Log("pedestrians forwardDirection" + i + ": " + globalForwardDirection);//230903

        //    //float cosineSimilarity = Vector3.Dot(globalForwardDirection.normalized, vectorToTarget.normalized);//230903
        //    float cosineSimilarity = Mathf.Floor(Vector3.Dot(globalForwardDirection.normalized, vectorToTarget.normalized) * 1000f) / 1000f;//230903
        //    Debug.Log("Cosine Similarity: " + cosineSimilarity);//230903

        //    // Check if the current cosineSimilarity is greater than the maximum
        //    if (cosineSimilarity > maxCosineSimilarity) //230903
        //    {
        //        maxCosineSimilarity = cosineSimilarity; //230903 Update the maximum
        //        indexOfMaxCosineSimilarity = i; //230903 Update the index
        //    }
        //}
        //// Now, maxCosineSimilarity contains the maximum value, and indexOfMaxCosineSimilarity contains the corresponding index
        //Debug.Log("Max Cosine Similarity: " + maxCosineSimilarity);//230903
        ////Debug.Log("Index of Max Cosine Similarity: " + indexOfMaxCosineSimilarity);//230903
        //AddReward(0.5f * maxCosineSimilarity);//230903
        //Debug.Log("AddReward: Max Cosine Similarity.");//230903
        //moverTransforms.Clear();//230903 Clear the list for the next step


        // Before arrived goal 有以下两个奖励机制：
        if (n_threshold <= 4)
        {
            // restrict agent not to detour too far. 限制Agent不要绕路太远
            if (distanceToTarget > 2 * linearDistanceToTarget)
            {
                float delta = distanceToTarget - linearDistanceToTarget;
                AddReward(-0.01f * delta);
            }

            // encourage agent get closer to target. 鼓励Agent靠近目标
            AddReward(Mathf.Pow(10, -3) * (1 / distanceToTarget - 1));

            // 230904 time limit
            int timeStep = StepCount;// 230904
            
            //Debug.Log("Time Step: " + timeStep);// 230904
            AddReward(-0.01f);// 230904 timeStepPenalty ??
            //Debug.Log("AddReward = -0.01f");
            
            if (this.StepCount == this.MaxStep) //是否超過最大步數 //t4, 230918 show training
            {
                _failCount += 1;//230918 show training

                _round += 1;//t4, 230918 show training 到第X回合
                Debug.Log("round:" + _round + " Timeout:" + _failCount + " Obstacle:" + _colObstacleCount + " People:" + _colPeopleCount);//t4, 230918 show training
            }

        }
        //Arrived goal
        if (n_threshold > 4)
        {
            AddReward(10f);
            switch (modeSwitch)  //根据 modeSwitch 更新 _stage 
            {
                case GameMode.FollowScene1:
                    if (_stage == 1)//to do ?
                        _stage = 0;
                    Debug.Log("FollowScene1, stage = 0");
                    break;
                case GameMode.FollowScene2:
                    if (_stage == 1)
                        _stage = 0;
                    Debug.Log("FollowScene2, stage = 0");
                    break;
                case GameMode.FollowScene3:
                    if (_stage == 1)
                        _stage = 0;
                    Debug.Log("FollowScene3, stage = 0");
                    break;
                case GameMode.FollowScene4:
                    break;
                case GameMode.test1:
                    if (_stage == 1)
                        _stage = 0;
                    else
                    {
                        _stage += 1;
                    }
                    Debug.Log("test1, stage = 0");
                    break;
                case GameMode.test2:
                    break;
            }
            _round += 1;//t4, 230918 show training 到第X回合
            _ArrivalCount += 1;//230919
            //230918 檢查 StepCount 的值並計數
            if (StepCount < 1000)//230918
            {//230918
                stepCountGreaterThan1000++;// 230918
            }

            if (StepCount > 1000 && StepCount <= 1500)//230918
            {//230918
                stepCountBetween1000And1500++;//230918
            }

            if (StepCount > 1500)//230918
            {//230918
                stepCountGreaterThan1500++;//230918
            }
            Debug.Log("round: " + _round + " Arrival Goal: " + _ArrivalCount + " ,StepCount:" + StepCount + " ,stepCount>1500:" + stepCountGreaterThan1500 + " stepCount<1000: " + stepCountGreaterThan1000 + " stepCount1000-1500: " + stepCountBetween1000And1500 + " ,Obstacle: " + _colObstacleCount + " ,People: " + _colPeopleCount);//230918
            EndEpisode();
            //Debug.Log("test1, Episode End");
        }

        switch (modeSwitch)//230918 Falling Detect=检测Agent是否超出范围
        {//230918
            case GameMode.test1://230918
                if (gameObject.transform.localPosition.x < -74.5f || gameObject.transform.localPosition.x > -48f
                                                        || gameObject.transform.localPosition.z < -8.5f
                                                        || gameObject.transform.localPosition.z > 5f)
                {
                    AddReward(-10f);//230918
                    EndEpisode();//230918
                }
                break;//230918

            //case GameMode.test2://230918
            //    if (gameObject.transform.localPosition.x < -74.5f || gameObject.transform.localPosition.x > -48f
            //                                            || gameObject.transform.localPosition.z < 12f
            //                                            || gameObject.transform.localPosition.z > 26f)
            //    {
            //        AddReward(-10f);//230918
            //        EndEpisode();//230918
            //    }
            //    break;//230918
            case GameMode.test2://230918
                if (gameObject.transform.localPosition.x < -75f || gameObject.transform.localPosition.x > -47f
                                                        || gameObject.transform.localPosition.z < 11f
                                                        || gameObject.transform.localPosition.z > 29f)
                {
                    AddReward(-10f);//230918
                    EndEpisode();//230918
                }
                break;//230918
        }//230918
    }

    //a is target, b is agent.
    public float GetAngle(Transform a, Transform b)
    {
        Vector3 targetDir = a.position - b.position;
        float angle = Vector3.Angle(targetDir, b.forward);

        return angle;
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;

        //if (joyStick)
        //{
        //    float _joy_x = Input.GetAxis("Horizontal_Left");
        //    float _joy_y = Input.GetAxis("Vertical_Left");

        //    continuousActionsOut[0] = _joy_y;
        //    continuousActionsOut[1] = _joy_x;
        //}
        //else
        //{
        //    if(Input.GetKey(KeyCode.W))
        //    {
        //        continuousActionsOut[0] = 1f;
        //    }
        //    else if(Input.GetKey(KeyCode.S))
        //    {
        //        continuousActionsOut[0] = -1f;
        //    }
        //    else if(Input.GetKey(KeyCode.A))
        //    {
        //        continuousActionsOut[1] = -1f;
        //    }
        //    else if(Input.GetKey(KeyCode.D))
        //    {
        //        continuousActionsOut[1] = 1f;
        //    }
        //    else
        //    {
        //        continuousActionsOut[0] = 0f;
        //        continuousActionsOut[1] = 0f;
        //    }
        //}

        float _joy_x = 0f;
        float _joy_y = 0f;

        if (joyStick)
        {
            _joy_x = Input.GetAxis("Horizontal_Left");
            _joy_y = Input.GetAxis("Vertical_Left");
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                _joy_y = 1f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _joy_y = -1f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                _joy_x = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _joy_x = 1f;
            }
        }

        // Here, combine forward and right movement based on your desired behavior.
        // For example, you can move forward and right simultaneously at half speed:
        float speed = 0.5f;
        float forward = _joy_y * speed;
        float right = _joy_x * speed;

        continuousActionsOut[0] = forward;
        continuousActionsOut[1] = right;
    }

    
}
