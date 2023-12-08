using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;

public class Peasant : MonoBehaviour
{
    public bool converted = false;
    float range = 5; // radius of sphere
    public Transform centerPoint; // center of the area the agent wants to move around

    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    //Animator anim;
    [SerializeField] GameObject childSprite;
    SpriteRenderer sprite;
    //float speed = 50; // 10 original

    [Header("Player GameObject and Transform")]
    GameObject player;
    Transform playerTransform;

    [Header("Field of View Parameters")]
    public float radius = 12;
    [Range(0, 360)]
    public float angle = 160;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    Collider[] rangeChecks;
    Transform target;
    Vector3 directionToTarget;
    float distanceToPlayer;

    [Header("Chase, Attack and Agro Parameters")]
    public bool isAgro = false;
    float maxAgroCounter = 5;
    public float agroCounter = 0;
    Vector3 direction;
    Quaternion lookRotation;
    NavMeshAgent agent;
    float attackCooldown = 1;
    float attackCooldownOriginal;
    float originalSpeed;

    bool gettingHitFetch; // from EnemyHealth -script
    public int convertOnlyOnce = 0;

    [Header("Death Parameters")]
    bool deadFetch; // from EnemyHealth -script
    bool dead = false;

    [Header("Patrol")]
    [Header("Patrol Parameters")]
    public Transform[] waypoints;
    int waypointIndex;
    public float waypointCounter = 0;
    float waypointMaxTime = 3;
    public bool randomPatrol = false;

    [Header("Audio")]
    [SerializeField] AudioClip attackSound;

    public bool activated;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = PlayerManager.instance.player;
        playerTransform = player.transform;
        agent = GetComponent<NavMeshAgent>();
        childSprite.GetComponent<Animator>();
        sprite = childSprite.GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        StartCoroutine(FOVRoutine());
        attackCooldownOriginal = attackCooldown;
        originalSpeed = agent.speed;
        //waypoint1 = waypoints[0];
        //waypoint1 = transform.position;
        //waypoint2 = waypoints[1];
        //waypoint2 = new Vector3(0, 1, 0);
    }

    private void Update()
    {
        gettingHitFetch = GetComponent<EnemyHealth>().gettingHit;
        deadFetch = GetComponent<EnemyHealth>().dead;
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if(distanceToPlayer < 30)
        {
            activated = true;
        }

        if (gettingHitFetch)
        {
            childSprite.GetComponent<Animator>().SetBool("Walking", false);
        }

        Death();

        if (!dead && activated)
        {
            if (converted)
            {
                if (converted && convertOnlyOnce == 0)
                {
                    Convert();
                    convertOnlyOnce = 1;
                }
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
                    if (distanceToPlayer > 2.2f)
                    {
                        Chase();
                        attackCooldown = 0.2f;
                    }
                    else if (distanceToPlayer <= 2.2f)
                    {
                        childSprite.GetComponent<Animator>().SetBool("Walking", false);
                        transform.LookAt(player.transform.position);
                        if (!gettingHitFetch)
                        {
                            if (attackCooldown >= 0)
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
                    agent.stoppingDistance = 1.5f;
                    if (waypointCounter < waypointMaxTime)
                    {
                        waypointCounter += Time.deltaTime;
                    }
                    else
                    {
                        agent.SetDestination(waypoints[waypointIndex].position);
                        childSprite.GetComponent<Animator>().SetBool("Walking", true);
                    }
                }
            }
            else
            {
                RandomMovement();
                //Patrol();
            }
        }
    }

    void RandomMovement()
    {
        if(converted)
        {
            childSprite.GetComponent<Animator>().SetBool("Walking", true);
        }
        else
        {
            childSprite.GetComponent<Animator>().SetBool("NotTurnedWalk", true);
        }

        agent.speed = originalSpeed;
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centerPoint.position, range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
            }
        }
    }

    void Patrol()
    {
        agent.speed = originalSpeed;
        // Add running false
        if (Vector3.Distance(transform.position, waypoints[waypointIndex].transform.position) < 1.5f)
        {
            if (randomPatrol)
            {
                waypointIndex = Random.Range(0, 5);
            }
            else
            {
                waypointIndex++;
            }
            waypointCounter = 0;
            childSprite.GetComponent<Animator>().SetBool("Walking", false);

            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
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
        AudioManager.instance.PlaySound(attackSound);
    }

    void Chase()
    {
        agent.speed = 3;
        agent.SetDestination(playerTransform.position);
        childSprite.GetComponent<Animator>().SetBool("Walking", true);
    }

    void Convert()
    {
        string currentTag = gameObject.tag;
        if (converted)
        {
            if (currentTag == "Peasant")
            {
                gameObject.tag = "Enemy";
                //sprite.color = Color.red;
                childSprite.GetComponent<Animator>().SetBool("NotTurnedWalk", false);
            }
        }
    }

    IEnumerator CallHelp()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Transform enemyTransform = enemy.transform;
            Knight knight = enemy.GetComponent<Knight>();
            Peasant peasant = enemy.GetComponent<Peasant>();
            Longbowman longbowman = enemy.GetComponent<Longbowman>();
            Priest priest = enemy.GetComponent<Priest>();

            if (enemyTransform != null && knight != null)
            {
                float distance = Vector3.Distance(transform.position, enemyTransform.position);
                if (distance < 20)
                {
                    knight.isAgro = true;
                }
            }
            if (enemyTransform != null && peasant != null)
            {
                float distance = Vector3.Distance(transform.position, enemyTransform.position);
                if (distance < 20)
                {
                    peasant.isAgro = true;
                }
            }
            if (enemyTransform != null && longbowman != null)
            {
                float distance = Vector3.Distance(transform.position, enemyTransform.position);
                if (distance < 20)
                {
                    longbowman.isAgro = true;
                }
            }
            if (enemyTransform != null && priest != null)
            {
                float distance = Vector3.Distance(transform.position, enemyTransform.position);
                if (distance < 20)
                {
                    priest.isAgro = true;
                }
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
                distanceToPlayer = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToPlayer, obstructionMask))
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

    private void OnDrawGizmos()
    {
        /*if(waypoint1 != null && waypoint2 != null)
        {
            Gizmos.DrawLine(transform.position, waypoint1);
            Gizmos.DrawLine(transform.position, waypoint2);
        }*/
    }
}
