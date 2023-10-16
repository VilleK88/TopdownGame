using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class TestEnemy : MonoBehaviour
{
    Rigidbody rb;
    //Animator anim;
    [SerializeField] GameObject childSprite;
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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.angularVelocity = Vector3.zero;
        //player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        agent = GetComponent<NavMeshAgent>();
        childSprite.GetComponent<Animator>();
        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
        if(canSeePlayer)
        {
            isAgro = true;
            agroCounter = 0;
            //LookAtPlayer();
        }
        else
        {
            if(isAgro)
            {
                //LookAtPlayer();
                if(agroCounter < maxAgroCounter)
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

        if(isAgro)
        {
            Chase();

            if (distanceToTarget <= 2)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        childSprite.GetComponent<Animator>().SetTrigger("PikeAttack1");
    }

    void Chase()
    {
        //direction = (player.transform.position - transform.position).normalized;
        //rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position,
        //speed * Time.deltaTime);
        agent.SetDestination(playerTransform.position);
    }

    void LookAtPlayer()
    {
        lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
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
                distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
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
        else if(canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}
