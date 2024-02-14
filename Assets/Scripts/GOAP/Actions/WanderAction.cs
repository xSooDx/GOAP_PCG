using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderAction : GAction
{
    public float wanderRadius = 5f;
    [Range(0f, 2f)] public float randomness = 2f;
    public LayerMask navLayerMask;
    public LayerMask objectsOfInterestMask;

    public override float GetCost()
    {
        return 1f;
    }

    public override string GetName()
    {
        return "Wander Action";
    }

    public override bool PrePerform()
    {
        Vector3 dir = transform.forward + (transform.right * Random.Range(-randomness, randomness));
        dir.Normalize();
        if (NavMesh.SamplePosition(transform.position + dir * wanderRadius, out NavMeshHit hit, wanderRadius, navLayerMask))
        {
            Debug.DrawLine(transform.position, hit.position, Color.red);
            navAgent.destination = hit.position;
            return true;
        }
        return false;
    }

    public override bool PostPerform()
    {
        //Collider[] colliders =  Physics.OverlapSphere(transform.position, wanderRadius, objectsOfInterestMask);
        //if(colliders.Length > 0)
        //{
        //    gAgentRef.target = colliders[0].attachedRigidbody ? colliders[0].attachedRigidbody.gameObject : colliders[0].gameObject;
        //    //return true;
        //}
        return true;
        
    }



    // Update is called once per frame
    void Update()
    {
        if(navAgent.ReachedNavDestination(1f))
        {
            StopAction();
        }
    }
}
