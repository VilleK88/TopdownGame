using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyPeasant : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator anim;

    [Header("Seeing Player Parameters")]
    [SerializeField] GameObject player;
    public float radius = 5;
    [Range(0, 360)] public float angle = 45;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;
    public bool CanSeePlayer; //{ get; set; }

    [Header("Speed and Distance Parameters")]
    float speed = 3;
    float walkSpeed = 2;
    float distance = 5;
    Vector2 target;

    [Header("Agro Parameters")]
    bool isAgro = false;
    float maxAgroCounter = 5;
    public float agroCounter = 0;

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
    }

    private void Update()
    {
        float shootingDistance = Vector2.Distance(transform.position, player.transform.position);

        if (CanSeePlayer && shootingDistance < 10)
        {
            isAgro = true;
            agroCounter = 0;
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
            Chase();
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
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.right * transform.localScale.x, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

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
        Debug.DrawRay(transform.position, direction, Color.green);
        lookAtAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Debug.Log("Angle: " + angle);
        angleAxis = Quaternion.AngleAxis(lookAtAngle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 50);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);

        Vector2 angle01 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector2 angle02 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(transform.position, angle01 * radius);
        //Gizmos.DrawLine(transform.position, angle02 * radius);

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
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < distance)
        {
            target = player.transform.position;
            Vector2 direction = (target - (Vector2)transform.position).normalized;
            rb2d.velocity = direction * speed;

            if (direction.x > 0)
            {
                //transform.localScale = new Vector2(1, 1);
            }
            else if (direction.x < 0)
            {
                //transform.localScale = new Vector2(-1, 1);
            }
        }
        else if (distanceToPlayer > distance && Vector2.Distance(transform.position, target) < 0.1f)
        {
            target = new Vector2(player.transform.position.x, player.transform.position.y + 2);
        }
        else
        {
            Vector2 direction = (target - (Vector2)transform.position).normalized;
            rb2d.velocity = direction * speed;

            if (direction.x > 0)
            {
                //transform.localScale = new Vector2(1, 1);
            }
            else if (direction.x < 0)
            {
                //transform.localScale = new Vector2(-1, 1);
            }
        }
    }

    void StopChasingPlayer()
    {
        isAgro = false;
    }
}
