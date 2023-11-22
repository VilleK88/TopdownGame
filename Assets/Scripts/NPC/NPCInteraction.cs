using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCInteraction : MonoBehaviour
{
    [Header("Player")]
    GameObject player;

    [Header("Interact with NPC")]
    [HideInInspector] public float radius = 3;
    [HideInInspector] public float distance;


    private void Start()
    {
        //player = PlayerManager.instance.player;
    }

    private void Update()
    {
        distance = Vector3.Distance(PlayerManager.instance.player.transform.position, transform.position);

        if (distance <= radius)
        {
            transform.LookAt(PlayerManager.instance.player.transform.position);
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
        else
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        }
    }

    public virtual void Interact()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
