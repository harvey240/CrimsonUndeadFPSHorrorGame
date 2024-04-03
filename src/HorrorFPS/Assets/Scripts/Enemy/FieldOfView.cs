using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    public float radius;
    [Range(0,360)]
    public float angle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    [HideInInspector]
    public Transform playerRef;

    public bool canSeePlayer;

    // Start is called before the first frame update
    void Start()
    {
        // playerRef = GameObject.FindGameObjectWithTag("Player");
        playerRef = PlayerManager.instance.player.transform;
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
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
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle/2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask, QueryTriggerInteraction.Ignore))
                    canSeePlayer=true;
                else
                    canSeePlayer=false;
            }
        }
    }

}
