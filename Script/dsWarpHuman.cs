using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class dsWarpHuman : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private Vector3 initialPosition;
    //public static float speed = 1.0f;
    [SerializeField] private float xThreshold = -63.7f; // speed up
    [SerializeField] private float speedUp = 1.7f; // speed up

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        if (points.Length > 0)
        {
            GotoNextPoint();
        }
        else
        {
            Debug.LogWarning("No points have been set up for the agent.");
        }

        initialPosition = transform.position;

    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }


    void Update()
    {

        // Check if agent's x-coordinate is greater than -60f
        if (transform.position.x >= xThreshold)
        {
            //agent.ResetPath();
            // Change the agent's speed to 2.4m/s
            agent.speed = speedUp;
            //Vector3 newVelocity = agent.velocity.normalized * 2f;
            //agent.velocity = newVelocity;
            //agent.acceleration = 10f; // Set a higher acceleration value
            //Debug.Log("Change the agent's speed");

            // Choose the next destination point when the agent gets
            // close to the current one.
            // 當兩個條件都成立時，表示智能體已完成當前路徑並接近目的地，此時將執行 if 塊中的代碼
            if (!agent.pathPending && agent.remainingDistance < 1f)
            {
                agent.speed = 1f;
                //GotoNextPoint();
                //Vector3 targetPosition = new Vector3(-72f, 0f, -17f);
                Vector3 targetPosition = initialPosition;
                //Debug.Log("!Warp agent!");
                agent.Warp(targetPosition);
                //Debug.Log("!Warp agent!");
                GotoNextPoint();
            }
        }
    }
}