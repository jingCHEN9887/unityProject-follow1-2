using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl_goal : MonoBehaviour
{
    // Current agent 0617
    //private GameObject currentAgent;
    // Agents destination
    public GameObject goal;
    // Get the prefab
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the agent 0617
        //currentAgent = Instantiate(gameObject, transform.position, Quaternion.identity);
        // Access the agents NavMesh
        agent = this.GetComponent<NavMeshAgent>();
        // Instruct the agent where it has to go
        agent.SetDestination(goal.transform.position);
    }
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log("Movement has ended.");
            Destroy(agent.gameObject);  //0619is used to destroy the game object associated with the NavMeshAgent component.
            Debug.Log("The object has been destroyed.");
            //SpawnNewAgent();  // Spawn a new agent 0617
            //Debug.Log("Spawn a new agent.");
        }
    }

    //void SpawnNewAgent()  // 0617 It will generate pedestrians but will not delete them, causing an error.
    //{
    // Instantiate a new agent
    //currentAgent = Instantiate(gameObject, transform.position, Quaternion.identity);
    // Get the NavMeshAgent component of the new agent
    //agent = currentAgent.GetComponent<NavMeshAgent>();
    // Set the agent's destination
    //agent.SetDestination(goal.transform.position);
    //}

}
