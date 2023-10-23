using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestPeasant : MonoBehaviour
{
    public bool converted = false;
    float range = 4; // radius of sphere
    public Transform centerPoint; // center of the area the agent wants to move around

    Rigidbody rb;
    //Animator anim;
    [SerializeField] GameObject childSprite;
    public GameObject player;
    public Transform playerTransform;
    float speed = 50; // 10 original

    [Header("Field of View Parameters")]
    public float radius = 6;
    [Range(0, 360)]
    public float angle = 90;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    Collider[] rangeChecks;
    Transform target;
    Vector3 directionToTarget;
    float distanceToTarget;

    [Header("Chase, Attack and Agro Parameters")]
    public bool isAgro = false;
    float maxAgroCounter = 5;
    public float agroCounter = 0;
    Vector3 direction;
    Quaternion lookRotation;
    NavMeshAgent agent;

    bool ifBlockingPlayersAttackFetch; // from EnemyHealth -script

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        agent = GetComponent<NavMeshAgent>();
        childSprite.GetComponent<Animator>();
        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
        ifBlockingPlayersAttackFetch = GetComponent<TestEnemyHealth>().blockingPlayer;

        if(converted)
        {
            if (canSeePlayer)
            {
                isAgro = true;
                agroCounter = 0;
                //LookAtPlayer();
            }
            else
            {
                if (isAgro)
                {
                    //LookAtPlayer();
                    if (agroCounter < maxAgroCounter)
                    {
                        agroCounter += Time.deltaTime;
                    }
                    else
                    {
                        agroCounter = 0;
                        isAgro = false;
                    }
                }
            }

            if (isAgro)
            {
                if (distanceToTarget > 2)
                {
                    Chase();
                }
                else if (distanceToTarget <= 2)
                {
                    transform.LookAt(player.transform.position);
                    if (!ifBlockingPlayersAttackFetch)
                    {
                        Attack();
                    }
                }
            }
        }
        else
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 point;
                if(RandomPoint(centerPoint.position, range, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                }
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    void Attack()
    {
        childSprite.GetComponent<Animator>().SetTrigger("PikeAttack1");
    }

    void Chase()
    {
        agent.SetDestination(playerTransform.position);
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAgro = true;
            agroCounter = 0;
        }
    }
}
