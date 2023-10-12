using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(TestEnemy))]

public class TestEnemy : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject player;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    Collider[] rangeChecks;
    Transform target;
    Vector3 directionToTarget;
    float distanceToTarget;
    //Vector3 viewAngle01;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
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

    /*void OnSceneGUI()
    {
        Handles.color = Color.white;
        Handles.DrawWireArc(target.transform.position, Vector3.up, Vector3.forward, 360, radius);

        viewAngle01 = DirectionFromAngle(target.transform.eulerAngles.y, -target.eulerAngles / 2);
    }*/

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.white;
        //Handles.DrawWireArc(target.transform.position, Vector3.up, Vector3.forward, 360, radius);

        //Vector3 viewAngle01 = DirectionFromAngle(target.transform.eulerAngles.y, -target.angle / 2);
    }

    /*Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }*/
}
