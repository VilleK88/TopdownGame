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

    [Header("Player GameObject and Transform")]
    GameObject player;
    Transform playerTransform;

    [Header("Field of View Parameters")]
    public float radius = 7;
    [Range(0, 360)]
    public float angle = 120;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    Collider[] rangeChecks;
    Transform target;
    Vector3 directionToTarget;
    float distanceToTarget;

    [Header("Patrol Parameters")]
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 waypointTarget;
    public float waypointCounter = 0;
    float waypointMaxTime = 2;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = PlayerManager.instance.player;
        playerTransform = player.transform;
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(FOVRoutine());
        agent.SetDestination(waypoints[waypointIndex].position);
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if(canSeePlayer)
        {
            //gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            transform.LookAt(player.transform.position);
        }
        else
        {
            //gameObject.GetComponent<NavMeshAgent>().isStopped = false;
            Patrol();
            if(waypointCounter < waypointMaxTime)
            {
                waypointCounter += Time.deltaTime;
            }
            else
            {
                agent.SetDestination(waypoints[waypointIndex].position);
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

    IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    void FieldOfViewCheck()
    {
        rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            target = rangeChecks[0].transform;
            directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}
