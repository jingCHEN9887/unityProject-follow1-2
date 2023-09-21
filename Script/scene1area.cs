using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class scene1area : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // to do 20230725
        //Vector3 targetPosition = initialPosition;

        //if (!(targetPosition.x >= -70f && targetPosition.x <= -62f))
        //{
        //    if ((agent.transform.position.x < -70f || agent.transform.position.x > -42f
        //        || agent.transform.position.z < -10f || agent.transform.position.z > -30f))
        //    {
        //        agent.Warp(targetPosition);
        //        Debug.Log("Out of range. Warp agent1");
        //    }
        //}
    }
}
