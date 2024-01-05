using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bear : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    [SerializeField] GameObject childSprite;

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
    float distanceToTarget;

    [Header("Chase, Attack and Agro Parameters")]
    public bool isAgro = false;
    float maxAgroCounter = 5;
    public float agroCounter = 0;
    NavMeshAgent agent;
    float attackCooldown = 1f;
    float attackCooldownOriginal;
    bool ifBlockingPlayersAttackFetch; // from EnemyHealth -script
    float originalSpeed;
    public float attackDistance = 2;
    bool gettingHit; // fetch from EnemyHealth -script
    float distanceToPlayer;

    [Header("Strong Attack")]
    public bool strongAttack = false;
    //float maxChargeTime = 1;
    //public float chargeTimeCounter = 0;
    public float maxNextChargeTime;
    public float nextChargeTimeCounter = 0;
    public bool randomTime = true;
    Vector3 playerChargePosition;
    bool checkPlayerPosition = true;
    bool playRoarOnlyOnce = true;

    [Header("Patrol Parameters")]
    public Transform[] waypoints;
    int waypointIndex;
    public float waypointCounter = 0;
    float waypointMaxTime = 3;
    public bool randomPatrol = false;

    bool deadFetch; // from EnemyHealth -script
    bool dead = false;

    [Header("Audio Info")]
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip roar;

    bool playerDeadFetch;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //player = PlayerManager.instance.player;
        player = PlayerManager.instance.GetPlayer();
        playerTransform = player.transform;
        agent = GetComponent<NavMeshAgent>();
        childSprite.GetComponent<Animator>();
        StartCoroutine(FOVRoutine());
        agent.SetDestination(waypoints[waypointIndex].position);
        capsuleCollider = GetComponent<CapsuleCollider>();
        attackCooldownOriginal = attackCooldown;
        originalSpeed = agent.speed;
        //childSprite.GetComponent<Animator>().SetBool("CrusaderWalk", true);
        childSprite.GetComponent<Animator>().SetBool("Walk", true);
    }

    private void Update()
    {
        ifBlockingPlayersAttackFetch = GetComponent<EnemyHealth>().blockingPlayer;
        deadFetch = GetComponent<EnemyHealth>().dead;
        gettingHit = GetComponent<EnemyHealth>().gettingHit;
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        playerDeadFetch = player.GetComponent<PlayerHealth>().dead;
        if (gettingHit)
        {
            childSprite.GetComponent<Animator>().SetBool("Walk", false);
            //childSprite.GetComponent<Animator>().SetBool("CrusaderRun", false);
        }
        Death();

        if (!dead)
        {
            if (canSeePlayer && !playerDeadFetch)
            {
                isAgro = true;
                agroCounter = 0;
                agent.stoppingDistance = 2.9f;
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

            if (isAgro && !playerDeadFetch)
            {
                if(randomTime)
                {
                    maxNextChargeTime = Random.Range(5, 8);
                    randomTime = false;
                }
                else
                {
                    if(maxNextChargeTime >= nextChargeTimeCounter)
                    {
                        nextChargeTimeCounter += Time.deltaTime;
                    }
                    else
                    {
                        strongAttack = true;
                        if(checkPlayerPosition)
                        {
                            playerChargePosition = player.transform.position;
                            checkPlayerPosition = false;
                            StartChargeTowardsPosition();
                        }
                    }
                }


                if(strongAttack)
                {
                    Charge();
                }

                else if (!strongAttack)
                {
                    if (distanceToPlayer > attackDistance)
                    {
                        if (!ifBlockingPlayersAttackFetch)
                        {
                            Chase();
                            attackCooldown = 0.2f;
                        }
                        else
                        {
                            transform.LookAt(player.transform.position);
                        }
                    }
                    else if (distanceToPlayer < attackDistance)
                    {
                        childSprite.GetComponent<Animator>().SetBool("Walk", false);
                        transform.LookAt(player.transform.position);
                        if (!ifBlockingPlayersAttackFetch)
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
                    childSprite.GetComponent<Animator>().SetBool("Walk", true);
                }
            }
        }

        if (playerDeadFetch)
        {
            isAgro = false;
        }
    }

    void Patrol()
    {
        agent.speed = originalSpeed;
        //childSprite.GetComponent<Animator>().SetBool("CrusaderRun", false);
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
            childSprite.GetComponent<Animator>().SetBool("Walk", false);

            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }
    }

    void Attack()
    {
        childSprite.GetComponent<Animator>().SetTrigger("BiteAttack");
        AudioManager.instance.PlaySound(attackSound);
    }

    void StartChargeTowardsPosition()
    {
        agent.SetDestination(playerChargePosition);
    }

    void Charge()
    {
        childSprite.GetComponent<Animator>().SetBool("Walk", false);
        childSprite.GetComponent<Animator>().SetBool("Charge", true);
        agent.speed = 8.2f;
        //agent.SetDestination(playerChargePosition);

        if(playRoarOnlyOnce)
        {
            AudioManager.instance.PlaySound(roar);
            playRoarOnlyOnce = false;
        }

        /*if (agent.remainingDistance <= agent.stoppingDistance)
        {
            childSprite.GetComponent<Animator>().SetBool("Charge", false);
            StrongAttack();
            agent.speed = 3.5f;
            strongAttack = false;
            checkPlayerPosition = true;
            nextChargeTimeCounter = 0;
            randomTime = true;
            playRoarOnlyOnce = true;
        }*/

        if(Vector3.Distance(transform.position, playerChargePosition) <= agent.stoppingDistance)
        {
            childSprite.GetComponent<Animator>().SetBool("Charge", false);
            ResetChargeState();
        }
    }

    void StrongAttack()
    {
        childSprite.GetComponent<Animator>().SetTrigger("StrongAttack");
    }

    void ResetChargeState()
    {
        agent.speed = 3.5f;
        strongAttack = false;
        checkPlayerPosition = true;
        nextChargeTimeCounter = 0;
        randomTime = true;
        playRoarOnlyOnce = true;
    }

    void Chase()
    {
        //childSprite.GetComponent<Animator>().SetBool("CrusaderWalk", false);
        agent.speed = 3.8f;
        agent.SetDestination(playerTransform.position);
        childSprite.GetComponent<Animator>().SetBool("Walk", true);
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
}
