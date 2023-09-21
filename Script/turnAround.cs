using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//public class turnAround : MonoBehaviour
//{
//    //public Transform[] points;
//    //private int destPoint = 0;
//    //private NavMeshAgent agent;
//    //private Vector3 initialPosition;
//    //private bool allPointsVisited = false;

//    //void Start()
//    //{
//    //    agent = GetComponent<NavMeshAgent>();
//    //    agent.autoBraking = false;

//    //    if (points.Length > 0)
//    //    {
//    //        GotoNextPoint();
//    //    }
//    //    else
//    //    {
//    //        Debug.LogWarning("No points have been set up for the agent.");
//    //    }

//    //    initialPosition = transform.position;
//    //}

//    //void GotoNextPoint()
//    //{
//    //    if (points.Length == 0)
//    //        return;

//    //    agent.destination = points[destPoint].position;
//    //    destPoint++;

//    //    if (destPoint >= points.Length)
//    //    {
//    //        destPoint = 0;

//    //        if (allPointsVisited)
//    //        {
//    //            Vector3 targetPosition = initialPosition;
//    //            agent.Warp(targetPosition);
//    //        }
//    //        else
//    //        {
//    //            allPointsVisited = true;
//    //        }
//    //    }
//    //}

//    //void Update()
//    //{
//    //    if (!agent.pathPending && agent.remainingDistance < 1f)
//    //    {
//    //        GotoNextPoint();
//    //    }
//    //}

//}
//public class turnAround : MonoBehaviour
//{
//    public Transform[] points;
//    private int destPoint = 0;
//    private NavMeshAgent agent;
//    private Vector3 initialPosition;
//    private bool allPointsVisited = false;
//    private bool completedOneLoop = false;

//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        agent.autoBraking = false;

//        if (points.Length > 0)
//        {
//            GotoNextPoint();
//        }
//        else
//        {
//            Debug.LogWarning("No points have been set up for the agent.");
//        }

//        initialPosition = transform.position;
//    }

//    void GotoNextPoint()
//    {
//        if (points.Length == 0)
//            return;

//        agent.destination = points[destPoint].position;
//        destPoint++;

//        if (destPoint >= points.Length)
//        {
//            if (completedOneLoop)
//            {
//                allPointsVisited = true;
//                completedOneLoop = false;
//            }
//            else
//            {
//                completedOneLoop = true;
//            }

//            destPoint = 0;
//        }
//    }

//    void Update()
//    {
//        if (!agent.pathPending && agent.remainingDistance < 1f)
//        {
//            if (allPointsVisited)
//            {
//                Vector3 targetPosition = initialPosition;
//                agent.Warp(targetPosition);
//                allPointsVisited = false;
//            }
//            GotoNextPoint();
//        }
//    }
//}
public class turnAround : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;
    private Vector3 initialPosition;
    private bool allPointsVisited = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
        if (points.Length == 0)
            return;

        agent.destination = points[destPoint].position;
        destPoint++;

        if (destPoint >= points.Length)
        {
            destPoint = 0;
            allPointsVisited = true;
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            if (allPointsVisited)
            {
                Vector3 targetPosition = initialPosition;
                agent.Warp(targetPosition);
                allPointsVisited = false;
            }

            GotoNextPoint();
        }
    }
}



