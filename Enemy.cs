using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class Enemy : MonoBehaviour
{
    public Transform goal;    
    UnityEngine.AI.NavMeshAgent agent;
    void Start () {
          agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
          agent.destination = goal.position; 
       }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(goal.position);     
    }
}
