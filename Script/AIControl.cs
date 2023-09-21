using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour
{

    GameObject[] goalLocations;
    UnityEngine.AI.NavMeshAgent agent;
    //Animator anim;


    // Use this for initialization
    void Start()
    {
        //Debug.Log("123");
        goalLocations = GameObject.FindGameObjectsWithTag("goal1");
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        //anim = this.GetComponent<Animator>();
        //anim.SetTrigger("isWalking");
        float sm = Random.Range(0.2f, 1);  //The function is to make the speed of each pedestrian different.
        agent.speed *= sm;  //The function is to make the speed of each pedestrian different.
    }

    // Update is called once per frame
    void Update()
    {

        if (agent.remainingDistance < 1)
        {

            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        }
    }
}
