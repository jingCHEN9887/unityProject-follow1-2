using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class destroySpawn : MonoBehaviour
{
    //public void OnCollisionEnter(Collision collision)
    //{
    //    //if (collision.gameObject.tag == "wall")
    //    if (transform.position.x > -54f || transform.position.z > 6f)
    //    {
    //        //Destroy(collision.gameObject);
    //        //Debug.Log("1collision.gameObject"); //Destroy the object that was collided with.=Destroy the wall.
    //        //Destroy(gameObject);
    //        //Debug.Log("2gameObject");
    //        //transform.parent.GetComponent<agentPrefab>().SpawnAgent();
    //        //Debug.Log("SpawnAgent");

    //        NavMeshAgent agent = GetComponent<NavMeshAgent>();
    //        Vector3 targetPosition = new Vector3(-72f, 0f, -17f);
    //        Debug.Log("Warp agent1");
    //        agent.Warp(targetPosition);
    //        Debug.Log("Warp agent2");

    //    }
    //}

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private Vector3 initialPosition;

    void Start()
    {
        
        //Debug.Log("666");
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


    void Update() // 20230724 human can move
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            //GotoNextPoint();
            //Vector3 targetPosition = new Vector3(-72f, 0f, -17f);
            Vector3 targetPosition = initialPosition;

            agent.Warp(targetPosition); // You may not need to warp the agent here again.
            //Debug.Log("Reached the target. Warp agent2");
            GotoNextPoint();

        }
    }

    //void Update() // 20230724 to do In case of going out of bounds, instantly teleport back to the initial position.
    //{
    //    Vector3 targetPosition = initialPosition;

    //    if (!(targetPosition.x >= -70f && targetPosition.x <= -62f))
    //    {
    //        if ((agent.transform.position.x < -70f || agent.transform.position.x > -42f
    //            || agent.transform.position.z < -10f || agent.transform.position.z > -30f))
    //        {              
    //            agent.Warp(targetPosition);
    //            Debug.Log("Out of range. Warp agent1");
    //        }
    //    }


    //    // Choose the next destination point when the agent gets
    //    // close to the current one.
    //    if (!agent.pathPending && agent.remainingDistance < 1f)
    //    {
    //        agent.Warp(targetPosition); // You may not need to warp the agent here again.
    //        Debug.Log("Reached the target. Warp agent2");
    //        GotoNextPoint();
    //    }
    //}
}