using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Knight : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    [SerializeField] GameObject childSprite;
    GameObject player;
    Transform playerTransform;
    [Header("Field of View Parameters")]
    public float radius = 10;
    [Range(0, 360)]
    public float angle = 140;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    Collider[] rangeChecks;
    Transform target;
    Vector3 directionToTarget;
    [Header("Chase, Attack and Agro Parameters")]
    public bool isAgro = false;
    float maxAgroCounter = 5;
    public float agroCounter = 0;
    NavMeshAgent agent;
    float attackCooldown = 1f;
    float attackCooldownOriginal;
    float originalSpeed;
    public float attackDistance = 2;
    bool gettingHit; // fetch from EnemyHealth -script
    float distanceToPlayer;
    bool isPlayeAttacking;
    int blockOrNot;
    public bool activeBlocking;
    [Header("Patrol Parameters")]
    public Transform[] waypoints;
    int waypointIndex;
    public float waypointCounter = 0;
    float waypointMaxTime = 3;
    public bool randomPatrol = false;
    [Header("Knight/Player Death Parameters")]
    bool deadFetch; // from EnemyHealth -script
    bool dead = false;
    bool playerDeadFetch;
    [Header("Audio Info")]
    [SerializeField] AudioClip attackSound;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = PlayerManager.instance.GetPlayer();
        playerTransform = player.transform;
        agent = GetComponent<NavMeshAgent>();
        childSprite.GetComponent<Animator>();
        StartCoroutine(FOVRoutine());
        agent.SetDestination(waypoints[waypointIndex].position);
        capsuleCollider = GetComponent<CapsuleCollider>();
        attackCooldownOriginal = attackCooldown;
        originalSpeed = agent.speed;
        childSprite.GetComponent<Animator>().SetBool("CrusaderWalk", true);
    }
    private void Update()
    {
        //ifBlockingPlayersAttackFetch = GetComponent<EnemyHealth>().blockingPlayer;
        deadFetch = GetComponent<EnemyHealth>().dead;
        gettingHit = GetComponent<EnemyHealth>().gettingHit;
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        playerDeadFetch = player.GetComponent<PlayerHealth>().dead;
        isPlayeAttacking = player.GetComponent<Player>().isPlayerAttacking;
        if (gettingHit)
        {
            childSprite.GetComponent<Animator>().SetBool("CrusaderWalk", false);
            childSprite.GetComponent<Animator>().SetBool("CrusaderRun", false);
        }
        Death();
        if(!dead)
        {
            if (canSeePlayer && !playerDeadFetch)
            {
                isAgro = true;
                agroCounter = 0;
                StartCoroutine(CallHelp());
                agent.stoppingDistance = 2;
            }
            else
            {
                if (isAgro)
                {
                    if (agroCounter < maxAgroCounter)
                        agroCounter += Time.deltaTime;
                    else
                    {
                        agroCounter = 0;
                        isAgro = false;
                    }
                }
            }

            if (isAgro)
            {
                if (distanceToPlayer > attackDistance && !playerDeadFetch)
                {
                    if(!activeBlocking)
                    {
                        Chase();
                        attackCooldown = 0.2f;
                    }
                    else
                        transform.LookAt(player.transform.position);
                }
                else if (distanceToPlayer < attackDistance && !playerDeadFetch)
                {
                    childSprite.GetComponent<Animator>().SetBool("CrusaderRun", false);
                    transform.LookAt(player.transform.position);

                    if(isPlayeAttacking)
                    {
                        blockOrNot = Random.Range(0, 2);
                        if(blockOrNot == 0)
                        {
                            activeBlocking = true;
                            childSprite.GetComponent<Animator>().SetBool("CrusaderBlock", true);
                            StartCoroutine(StopBlocking());
                        }
                    }

                    if (!activeBlocking)
                    {
                        if(attackCooldown >= 0)
                            attackCooldown -= Time.deltaTime;
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
                    waypointCounter += Time.deltaTime;
                else
                {
                    agent.SetDestination(waypoints[waypointIndex].position);
                    childSprite.GetComponent<Animator>().SetBool("CrusaderWalk", true);
                }
            }
        }
        if (playerDeadFetch)
            isAgro = false;
    }
    IEnumerator StopBlocking()
    {
        yield return new WaitForSeconds(1);
        childSprite.GetComponent<Animator>().SetBool("CrusaderBlock", false);
        activeBlocking = false;
    }
    void Patrol()
    {
        agent.speed = originalSpeed;
        childSprite.GetComponent<Animator>().SetBool("CrusaderRun", false);
        if (Vector3.Distance(transform.position, waypoints[waypointIndex].transform.position) < 1.5f)
        {
            if(randomPatrol)
                waypointIndex = Random.Range(0, 5);
            else
                waypointIndex++;
            waypointCounter = 0;
            childSprite.GetComponent<Animator>().SetBool("CrusaderWalk", false);
            if (waypointIndex >= waypoints.Length)
                waypointIndex = 0;
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
            Longbowman longbowman = enemy.GetComponent<Longbowman>();
            Priest priest = enemy.GetComponent<Priest>();
            if(enemyTransform != null && knight != null)
            {
                float distance = Vector3.Distance(transform.position, enemyTransform.position);
                if(distance < 20)
                {
                    knight.isAgro = true;
                    knight.canSeePlayer = true;
                }
            }
            if(enemyTransform != null && peasant != null)
            {
                float distance = Vector3.Distance(transform.position, enemyTransform.position);
                if (distance < 20)
                    peasant.isAgro = true;
            }
            if (enemyTransform != null && longbowman != null)
            {
                float distance = Vector3.Distance(transform.position, enemyTransform.position);
                if (distance < 20)
                    longbowman.isAgro = true;
            }
            if (enemyTransform != null && priest != null)
            {
                float distance = Vector3.Distance(transform.position, enemyTransform.position);
                if (distance < 20)
                    priest.isAgro = true;
            }
        }
    }
    void Attack()
    {
        childSprite.GetComponent<Animator>().SetTrigger("CrusaderAttack1");
        AudioManager.instance.PlaySound(attackSound);
    }
    void Chase()
    {
        childSprite.GetComponent<Animator>().SetBool("CrusaderWalk", false);
        agent.speed = 3.8f;
        agent.SetDestination(playerTransform.position);
        childSprite.GetComponent<Animator>().SetBool("CrusaderRun", true);
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
                distanceToPlayer = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, directionToTarget, distanceToPlayer, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if(canSeePlayer)
            canSeePlayer = false;
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
        gameObject.SetActive(false);
    }
}