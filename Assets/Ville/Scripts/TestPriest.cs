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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        peasants = GameObject.FindGameObjectsWithTag("Peasant");
        Debug.Log("Found " + peasants.Length + " peasants.");
        //MoveToNextPeasant();
    }

    private void Update()
    {
        deadFetch = GetComponent<TestPriestHealth>().dead;

        if (agent.remainingDistance <= agent.stoppingDistance && !deadFetch)
        {
            //TestPeasant testPeasant = peasants[currentPeasantIndex].GetComponent<TestPeasant>();
            //testPeasant = peasants[currentPeasantIndex].GetComponent<TestPeasant>();
            TestPeasant testPeasant = GetCurrentPeasantScript();
            if (testPeasant != null)
            {
                if (convertingMaxTime >= convertingCounter)
                {
                    convertingCounter += Time.deltaTime;
                }
                else
                {
                    testPeasant.converted = true;
                    convertingCounter = 0;
                    MoveToNextPeasant();
                }
                //testPeasant.converted = true;
                //StartCoroutine(ConvertingPeasant());
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

    IEnumerator ConvertingPeasant()
    {
        yield return new WaitForSeconds(5);
        if(!dead && !stopConverting)
        {
            //testPeasant.converted = true;
        }
    }

    void MoveToNextPeasant()
    {
        if (currentPeasantIndex < peasants.Length)
        {
            agent.SetDestination(peasants[currentPeasantIndex].transform.position);
            currentPeasantIndex++;
        }
        else
        {
            agent.isStopped = true;
            //stopConverting = true;
            Debug.Log("No more peasants to convert");
        }
    }
}
