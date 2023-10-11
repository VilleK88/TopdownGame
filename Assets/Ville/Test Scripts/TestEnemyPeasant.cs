using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyPeasant : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator anim;

    [Header("Seeing Player Parameters")]
    [SerializeField] GameObject player;
    float radius = 10;
    [Range(0, 360)] float angle = 90;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    public bool CanSeePlayer; //{ get; set; }

    [Header("Speed and Distance Parameters")]
    float speed = 3;
    float walkSpeed = 2;
    float distance = 3;
    Vector2 target;

    [Header("Agro Parameters")]
    public bool isAgro = false;
    float maxAgroCounter = 5;
    public float agroCounter = 0;
    float distanceToTarget;
    //float attackDistance;

    [Header("Look At Player Parameters")]
    Vector3 direction;
    float lookAtAngle;
    Quaternion angleAxis;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());

        target = player.transform.position;

        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        distanceToTarget = Vector2.Distance(transform.position, player.transform.position);
        //Debug.Log("AttackDistance: " + distanceToTarget);

        if (CanSeePlayer)
        {
            isAgro = true;
            agroCounter = 0;
            LookAtPlayer();
        }
        else
        {
            if (isAgro)
            {
                LookAtPlayer();

                if (agroCounter < maxAgroCounter)
                {
                    agroCounter += Time.deltaTime;
                }
                else
                {
                    agroCounter = 0;
                    StopChasingPlayer();
                }
            }
        }

        if(isAgro)
        {
            if(distanceToTarget <= 1.5f)
            {
                Attack();
            }
            else
            {
                Chase();
            }
        }
    }

    void Attack()
    {
        anim.SetTrigger("PikeAttack1");
        Debug.Log("Attack");
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
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up * transform.localScale.x, directionToTarget) < angle / 2)
            {
                distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget,
                    obstructionLayer))
                {
                    CanSeePlayer = true;
                }
                else
                {
                    CanSeePlayer = false;
                }
            }
            else
            {
                CanSeePlayer = false;
            }
        }
        else if (CanSeePlayer)
        {
            CanSeePlayer = false;
        }
    }

    void LookAtPlayer()
    {
        direction = player.transform.position - transform.position;
        //Debug.DrawRay(transform.position, direction, Color.green);
        lookAtAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        //Debug.Log("Angle: " + angle);
        angleAxis = Quaternion.AngleAxis(lookAtAngle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 50);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);

        Vector2 angle01 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector2 angle02 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, angle01 * radius);
        Gizmos.DrawLine(transform.position, angle02 * radius);

        if (CanSeePlayer)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }

    Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),
            Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void Chase()
    {
        direction = player.transform.position - transform.position;
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;
    }

    void StopChasingPlayer()
    {
        isAgro = false;
    }
}
