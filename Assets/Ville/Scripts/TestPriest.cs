using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class TestPriest : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    [SerializeField] GameObject childSprite;
    NavMeshAgent agent;
    public GameObject[] peasants;
    int currentPeasantIndex = 0;
    public bool convertedFetch;

    [Header("Converting Peasants Parameters")]
    TestPeasant testPeasant;
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 waypointTarget;
    bool stopConverting = false;
    float convertingMaxTime = 3;
    float convertingCounter = 0;

    public bool dead = false;

    bool deadFetch; // from TestPriestHealth -script

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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        peasants = GameObject.FindGameObjectsWithTag("Peasant");
        Debug.Log("Found " + peasants.Length + " peasants.");
        particleSystemConverting = particleConverting.GetComponent<ParticleSystem>();
        particleSystemHealing = particleHealing.GetComponent<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        InvokeRepeating("HealEnemies", 0, checkInterval);
    }

    private void Update()
    {
        deadFetch = GetComponent<TestEnemyHealth>().dead;

        if (agent.remainingDistance <= agent.stoppingDistance && !deadFetch)
        {
            if(!startHealing)
            {
                Convert();
            }
            else
            {
                agent.SetDestination(testEnemyHealth.transform.position);
            }
        }

        if(deadFetch)
        {
            particleSystemConverting.Stop();
        }

        Death();
    }

    void HealEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach(Collider collider in colliders)
        {
            if(collider.CompareTag(enemyTag))
            {
                //GameObject enemy = collider.gameObject;
                enemy = collider.gameObject;
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if(distance <= detectionRadius)
                {
                    NavMeshPath path = new NavMeshPath();
                    agent.CalculatePath(enemy.transform.position, path);

                    if(path.status == NavMeshPathStatus.PathComplete)
                    {
                        //TestEnemyHealth testEnemyHealth = enemy.GetComponent<TestEnemyHealth>();
                        testEnemyHealth = enemy.GetComponent<TestEnemyHealth>();

                        if (testEnemyHealth != null && testEnemyHealth.currentHealth < maxHealth &&
                            testEnemyHealth.currentHealth > 0)
                        {
                            //testEnemyHealth.currentHealth = maxHealth;
                            testEnemyHealth.currentHealth += 10;
                            particleSystemHealing.Play();
                            //StartCoroutine(Heal());
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

    IEnumerator Heal()
    {
        if(!dead)
        {
            yield return new WaitForSeconds(2);
            testEnemyHealth.currentHealth += 10;
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

    void Death()
    {
        if (deadFetch)
        {
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
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
