using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class TestPriest : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject[] peasants;
    int currentPeasantIndex = 0;
    public bool convertedFetch;

    [Header("Peasant Waypoint Parameters")]
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 waypointTarget;
    //TestPeasant testPeasant;

    public bool dead = false;
    bool stopConverting = false;

    float convertingMaxTime = 5;
    float convertingCounter = 0;

    bool deadFetch; // from TestPriestHealth -script

    [SerializeField] GameObject particle;
    ParticleSystem particleSystem;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        peasants = GameObject.FindGameObjectsWithTag("Peasant");
        Debug.Log("Found " + peasants.Length + " peasants.");
        particleSystem = particle.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        deadFetch = GetComponent<TestPriestHealth>().dead;

        if (agent.remainingDistance <= agent.stoppingDistance && !deadFetch)
        {
            Convert();
        }

        if(deadFetch)
        {
            particleSystem.Stop();
        }
    }

    void Convert()
    {
        TestPeasant testPeasant = GetCurrentPeasantScript();
        if (testPeasant != null)
        {
            if (testPeasant.converted == false)
            {
                agent.SetDestination(peasants[currentPeasantIndex].transform.position);
                transform.LookAt(peasants[currentPeasantIndex].transform.position);
                if (convertingMaxTime >= convertingCounter)
                {
                    convertingCounter += Time.deltaTime;
                    particleSystem.Play();
                }
                else
                {
                    testPeasant.converted = true;
                    particleSystem.Stop();
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
    }

    TestPeasant GetCurrentPeasantScript()
    {
        if(currentPeasantIndex >= 0 && currentPeasantIndex < peasants.Length)
        {
            return peasants[currentPeasantIndex].GetComponent<TestPeasant>();
        }
        return null;
    }
}
