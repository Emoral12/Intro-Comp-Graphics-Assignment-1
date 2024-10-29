using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Targeting : MonoBehaviour
{
    public GameObject Target;
    private NavMeshAgent agent;
    bool isWalking = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            agent.destination = Target.transform.position;
        }
        else
        {
            agent.destination = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            isWalking = false;        }
    }

}
