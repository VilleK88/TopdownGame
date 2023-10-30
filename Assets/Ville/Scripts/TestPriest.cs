using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class TestPriest : MonoBehaviour
{
    [Header("Rigidbody, Colliders and Sprites")]
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    [SerializeField] GameObject childSprite;

    [Header("Death Parameters")]
    bool dead = false;
    bool deadFetch; // from TestPriestHealth -script

    [Header("Converting Peasants Parameters")]
    public GameObject[] peasants;
    TestPeasant testPeasant;
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 waypointTarget;
    bool stopConverting = false;
    float convertingMaxTime = 3;
    float convertingCounter = 0;
    int currentPeasantIndex = 0;
    public bool convertedFetch;

    [Header("Particle Parameters")]
    [SerializeField] GameObject particleConverting;
    ParticleSystem particleSystemConverting;
    [SerializeField] GameObject particleHealing;
    ParticleSystem particleSystemHealing;

    [Header("Healing Enemies Parameters")]
    bool startHealing = false;
    float checkInterval = 2;
    float maxHealth = 100;
    string enemyTag = "Enemy";
    float detectionRadius = 20;
    //public GameObject healer;
    GameObject enemy;
    TestEnemyHealth testEnemyHealth;

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

    [Header("Player")]
    GameObject player;
    Transform playerTransform;

    [Header("Chase, Attack and Agro Parameters")]
    public bool isAgro = false;
    float maxAgroCounter = 5;
    public float agroCounter = 0;
    Vector3 direction;
    Quaternion lookRotation;
    NavMeshAgent agent;

    private void Start()
    {
        player = PlayerManager.instance.player;
        playerTransform = player.transform;
        agent = GetComponent<NavMeshAgent>();

        peasants = GameObject.FindGameObjectsWithTag("Peasant");
        Debug.Log("Found " + peasants.Length + " peasants.");
        particleSystemConverting = particleConverting.GetComponent<ParticleSystem>();
        particleSystemHealing = particleHealing.GetComponent<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        InvokeRepeating("HealEnemies", 0, checkInterval);
        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
        deadFetch = GetComponent<TestEnemyHealth>().dead;
        Death();


        if (!dead)
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

            if (agent.remainingDistance <= agent.stoppingDistance && !deadFetch)
            {
                if (!startHealing)
                {
                    Convert();
                }
                else
                {
                    agent.SetDestination(testEnemyHealth.transform.position);
                }
            }

            if (deadFetch)
            {
                particleSystemConverting.Stop();
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

    void HealEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach(Collider collider in colliders)
        {
            if(collider.CompareTag(enemyTag))
            {
                enemy = collider.gameObject;
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if(distance <= detectionRadius)
                {
                    NavMeshPath path = new NavMeshPath();
                    agent.CalculatePath(enemy.transform.position, path);

                    if(path.status == NavMeshPathStatus.PathComplete)
                    {
                        testEnemyHealth = enemy.GetComponent<TestEnemyHealth>();

                        if (testEnemyHealth != null && testEnemyHealth.currentHealth < maxHealth &&
                            testEnemyHealth.currentHealth > 0)
                        {
                            testEnemyHealth.currentHealth += 10;
                            particleSystemHealing.Play();
                        }
                        else
                        {
                            particleSystemHealing.Stop();
                        }
                    }
                }
            }
        }
    }

    void Convert()
    {
        //TestPeasant testPeasant = GetCurrentPeasantScript();
        testPeasant = GetCurrentPeasantScript();
        if (testPeasant != null)
        {
            if (testPeasant.converted == false)
            {
                agent.SetDestination(peasants[currentPeasantIndex].transform.position);
                transform.LookAt(peasants[currentPeasantIndex].transform.position);
                if (convertingMaxTime >= convertingCounter)
                {
                    convertingCounter += Time.deltaTime;
                    particleSystemConverting.Play();
                }
                else
                {
                    testPeasant.converted = true;
                    particleSystemConverting.Stop();
                }
            }
            else
            {
                if (currentPeasantIndex < peasants.Length)
                {
                    currentPeasantIndex++;
                    Debug.Log("Next peasant to convert.");
                    convertingCounter = 0;
                }
            }
        }
        else
        {
            startHealing = true;
        }
    }

    TestPeasant GetCurrentPeasantScript()
    {
        if(currentPeasantIndex >= 0 && currentPeasantIndex < peasants.Length)
        {
            return peasants[currentPeasantIndex].GetComponent<TestPeasant>();
        }
        return null;
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
            CancelInvoke("HealEnemies");
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
