using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Knight : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    //Animator anim;
    [SerializeField] GameObject childSprite;
    //float speed = 50; // 10 original

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

    [Header("Chase, Attack and Agro Parameters")]
    public bool isAgro = false;
    float maxAgroCounter = 5;
    public float agroCounter = 0;
    Vector3 direction;
    Quaternion lookRotation;
    NavMeshAgent agent;
    float attackCooldown = 1;
    float attackCooldownOriginal;

    bool ifBlockingPlayersAttackFetch; // from EnemyHealth -script

    [Header("Patrol Parameters")]
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 waypointTarget;
    public float waypointCounter = 0;
    float waypointMaxTime = 3;

    bool deadFetch; // from EnemyHealth -script
    bool dead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //player = GameObject.FindGameObjectWithTag("Player");
        player = PlayerManager.instance.player;
        playerTransform = player.transform;
        agent = GetComponent<NavMeshAgent>();
        childSprite.GetComponent<Animator>();
        StartCoroutine(FOVRoutine());
        agent.SetDestination(waypoints[waypointIndex].position);
        capsuleCollider = GetComponent<CapsuleCollider>();
        attackCooldownOriginal = attackCooldown;
    }

    private void Update()
    {
        ifBlockingPlayersAttackFetch = GetComponent<EnemyHealth>().blockingPlayer;
        deadFetch = GetComponent<EnemyHealth>().dead;
        Death();


        if(!dead)
        {
            if (canSeePlayer)
            {
                isAgro = true;
                agroCounter = 0;
                StartCoroutine(CallHelp());
            }
            else
            {
                if (isAgro)
                {
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
                        if(attackCooldown >= 0)
                        {
                            attackCooldown -= Time.deltaTime;
                        }
                        else
                        {
                            Attack();
                            attackCooldown = attackCooldownOriginal;
                        }
                    }
                }
            }
            else
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
        if(Vector3.Distance(transform.position, waypoints[waypointIndex].transform.position) < 1.5f)
        {
            waypointIndex++;
            waypointCounter = 0;

            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
            //agent.SetDestination(waypoints[waypointIndex].position);
            //StartCoroutine(NextWayPoint());
        }
    }

    IEnumerator CallHelp()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            Transform enemyTransform = enemy.transform;
            Knight knight = enemy.GetComponent<Knight>();
            Peasant peasant = enemy.GetComponent<Peasant>();
            if(enemyTransform != null && knight != null)
            {
                float distance = Vector3.Distance(transform.position, enemyTransform.position);
                if(distance < 20)
                {
                    knight.isAgro = true;
                }
            }
            if(enemyTransform != null && peasant != null)
            {
                float distance = Vector3.Distance(transform.position, enemyTransform.position);
                if (distance < 20)
                {
                    peasant.isAgro = true;
                }
            }
        }
    }

    IEnumerator NextWayPoint()
    {
        yield return new WaitForSeconds(3);
        if(!dead)
        {
            agent.SetDestination(waypoints[waypointIndex].position);
        }
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

        while(true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    void FieldOfViewCheck()
    {
        rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if(rangeChecks.Length != 0)
        {
            target = rangeChecks[0].transform;
            directionToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
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
        else if(canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isAgro = true;
            agroCounter = 0;
        }
    }

    void Death()
    {
        if (deadFetch)
        {
            dead = true;
            //childSprite.GetComponent<Animator>();
            if (capsuleCollider != null)
            {
                capsuleCollider.enabled = false;
            }
            this.enabled = false;
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;

            StartCoroutine(Vanish());
        }
    }

    IEnumerator Vanish()
    {
        yield return new WaitForSeconds(2);
        childSprite.GetComponent<SpriteRenderer>().enabled = false;
        childSprite.GetComponentInChildren<SpriteRenderer>().enabled = false;
        //yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}