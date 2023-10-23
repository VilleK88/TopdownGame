using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestPriest : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject[] peasants;
    public bool convertedFetch;

    [Header("Peasant Waypoint Parameters")]
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 waypointTarget;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        peasants = GameObject.FindGameObjectsWithTag("Peasant");
        Debug.Log("Found " + peasants.Length + " peasants.");
    }
}
