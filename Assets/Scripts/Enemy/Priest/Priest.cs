using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Priest : MonoBehaviour
{
    [Header("Rigidbody, Colliders and Sprites")]
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    [SerializeField] GameObject childSprite;
    [Header("Death Parameters")]
    bool dead = false;
    bool deadFetch; // from TestPriestHealth -script
    [Header("Converting Peasants Parameters")]
    public Peasant[] peasants;
    Peasant peasant;
    float convertingMaxTime = 3;
    float convertingCounter = 0;
    int currentPeasantIndex = 0;
    public bool convertedFetch;
    bool converting = false;
    [Header("Healing Enemies Parameters")]
    public bool startHealing = false;
    float checkInterval = 2;
    float maxHealth = 100;
    string enemyTag = "Enemy";
    float detectionRadius = 20;
    GameObject enemy;
    EnemyHealth enemyHealth;
    GameObject previousEnemy;
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
    [Header("Player")]
    GameObject player;
    [Header("Chase, Attack and Agro Parameters")]
    public bool isAgro = false;
    float maxAgroCounter = 5;
    public float agroCounter = 0;
    NavMeshAgent agent;
    bool healing = false;
    float distanceToPlayer;
    public bool activated;
    [Header("Hurt")]
    public float hitCounter = 0;
    [Header("Audio")]
    AudioSource convertingSound;
    private void Start()
    {
        player = PlayerManager.instance.player;
        agent = GetComponent<NavMeshAgent>();
        convertingSound = GetComponent<AudioSource>();
        peasants = FindObjectsOfType<Peasant>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        InvokeRepeating("HealEnemies", 0, checkInterval);
        StartCoroutine(FOVRoutine());
    }
    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        deadFetch = GetComponent<EnemyHealth>().dead;
        Death();
        if(distanceToPlayer < 30)
            activated = true;
        if (!dead && activated)
        {
            if (canSeePlayer)
            {
                isAgro = true;
                agroCounter = 0;
                StartCoroutine(CallHelp());
                if (!healing && !converting)
                    transform.LookAt(player.transform.position);
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
            if (agent.remainingDistance <= agent.stoppingDistance && !deadFetch)
            {
                if (!startHealing)
                    Convert();
                else
                    agent.SetDestination(enemyHealth.transform.position);
            }
            if (deadFetch)
                childSprite.GetComponent<Animator>().SetBool("Converting", false);
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
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToPlayer, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
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
                        enemyHealth = enemy.GetComponent<EnemyHealth>();
                        if (enemyHealth != null && enemyHealth.currentHealth < maxHealth &&
                            enemyHealth.currentHealth > 0)
                        {
                            enemyHealth.currentHealth += 10;
                            childSprite.GetComponent<Animator>().SetBool("Healing", true);
                            healing = true;
                        }
                        else
                        {
                            childSprite.GetComponent<Animator>().SetBool("Healing", false);
                            healing = false;
                        }
                    }
                }
            }
        }
    }
    void Convert()
    {
        peasant = GetCurrentPeasantScript();
        if (peasant != null)
        {
            if (peasant.converted == false)
            {
                agent.SetDestination(peasants[currentPeasantIndex].transform.position);
                transform.LookAt(peasants[currentPeasantIndex].transform.position);
                if (convertingMaxTime >= convertingCounter)
                {
                    convertingCounter += Time.deltaTime;
                    childSprite.GetComponent<Animator>().SetBool("Converting", true);
                    convertingSound.Play();
                    converting = true;
                }
                else
                {
                    peasant.converted = true;
                    childSprite.GetComponent<Animator>().SetBool("Converting", false);
                    convertingSound.Stop();
                }
            }
            else
            {
                if (currentPeasantIndex < peasants.Length)
                {
                    currentPeasantIndex++;
                    convertingCounter = 0;
                }
            }
        }
        else
        {
            startHealing = true;
            converting = false;
        }
    }
    Peasant GetCurrentPeasantScript()
    {
        if (currentPeasantIndex >= 0 && currentPeasantIndex < peasants.Length)
            return peasants[currentPeasantIndex].GetComponent<Peasant>();
        return null;
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
                    knight.isAgro = true;
            }
            if (enemyTransform != null && peasant != null)
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
    void Death()
    {
        if (deadFetch)
        {
            dead = true;
            convertingSound.Stop();
            if (capsuleCollider != null)
                capsuleCollider.enabled = false;
            this.enabled = false;
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            CancelInvoke("HealEnemies");
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