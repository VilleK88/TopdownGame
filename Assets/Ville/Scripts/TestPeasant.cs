using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;

public class TestPeasant : MonoBehaviour
{
    public bool converted = false;
    float range = 5; // radius of sphere
    public Transform centerPoint; // center of the area the agent wants to move around

    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    //Animator anim;
    [SerializeField] GameObject childSprite;
    SpriteRenderer sprite;
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
    float attackCooldown = 1;
    float attackCooldownOriginal;

    //bool ifBlockingPlayersAttackFetch; // from EnemyHealth -script
    bool gettingHitFetch; // from EnemyHealth -script
    public int convertOnlyOnce = 0;

    bool deadFetch; // from EnemyHealth -script
    bool dead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        agent = GetComponent<NavMeshAgent>();
        childSprite.GetComponent<Animator>();
        sprite = childSprite.GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        StartCoroutine(FOVRoutine());
        attackCooldownOriginal = attackCooldown;
    }

    private void Update()
    {
        //ifBlockingPlayersAttackFetch = GetComponent<TestEnemyHealth>().blockingPlayer;
        gettingHitFetch = GetComponent<TestEnemyHealth>().gettingHit;
        deadFetch = GetComponent<TestEnemyHealth>().dead;
        Death();


        if (!dead)
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
                    if (distanceToTarget > 2)
                    {
                        Chase();
                    }
                    else if (distanceToTarget <= 2)
                    {
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
                            //Attack();
                        }
                    }
                }
                else
                {
                    RandomMovement();
                }
            }
            else
            {
                RandomMovement();
            }
        }
    }

    void RandomMovement()
    {
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

    void Convert()
    {
        string currentTag = gameObject.tag;
        if(converted)
        {
            if(currentTag == "Peasant")
            {
                gameObject.tag = "Enemy";
                sprite.color = Color.red;
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
            TestEnemy testEnemy = enemy.GetComponent<TestEnemy>();
            TestPeasant testPeasant = enemy.GetComponent<TestPeasant>();
            if (enemyTransform != null && testEnemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemyTransform.position);
                if (distance < 20)
                {
                    testEnemy.isAgro = true;
                }
            }
            if (enemyTransform != null && testPeasant != null)
            {
                float distance = Vector3.Distance(transform.position, enemyTransform.position);
                if (distance < 20)
                {
                    testPeasant.isAgro = true;
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
