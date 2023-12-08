using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    [SerializeField] GameObject childSprite;
    NavMeshAgent agent;

    [Header("Patrol Parameters")]
    [SerializeField] private Transform[] waypoints;
    int waypointIndex;
    Vector3 waypointTarget;
    public float waypointCounter = 0;
    float waypointMaxTime = 2;
    public bool patrol = false;

    [Header("Interact with NPC")]
    [SerializeField] NPCInteraction npcInteraction;
    float distance;
    float radius;
    [SerializeField] GameObject speechIcon;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(waypoints[waypointIndex].position);
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        distance = npcInteraction.GetComponent<NPCInteraction>().distance;
        radius = npcInteraction.GetComponent<NPCInteraction>().radius;

        if (distance <= radius)
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            speechIcon.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
            speechIcon.SetActive(false);

            if(patrol)
            {
                Patrol();
                if (waypointCounter < waypointMaxTime)
                {
                    waypointCounter += Time.deltaTime;
                }
                else
                {
                    agent.SetDestination(waypoints[waypointIndex].position);
                }
            }
        }
    }

    void Patrol()
    {
        if (Vector3.Distance(transform.position, waypoints[waypointIndex].transform.position) < 1.5f)
        {
            //waypointIndex++;
            waypointIndex = Random.Range(0, 5);
            waypointCounter = 0;

            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }
    }
}
