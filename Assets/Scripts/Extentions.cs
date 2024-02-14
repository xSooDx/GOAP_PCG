using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class GAgentExtensions
{
    public static bool ReachedNavDestination(this NavMeshAgent navAgent, float dist, bool stop = true)
    {
        bool reached = !navAgent.pathPending && /*navAgent.hasPath &&*/ navAgent.remainingDistance < dist;
        if (reached && stop) navAgent.isStopped = true;

        return reached;
    }
}

