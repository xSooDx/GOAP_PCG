using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    public abstract float GetCost();
    public abstract string GetName();

    [SerializeField] GWorldState[] preConditionsStates;
    [SerializeField] GWorldState[] afterEffectsStates;

    public NavMeshAgent navAgent;

    public GWorldStates preConditions;
    public GWorldStates afterEffects;

    public GameObject target;
    public string targetTag;

    public GAgent gAgentRef;

    public bool WasSuccess { get; private set; }
    // Visual Debugger
    [SerializeField] bool IsRunning;

    public GAction()
    {
        preConditions = new GWorldStates();
        afterEffects = new GWorldStates();
    }

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();

        if (preConditionsStates != null)
        {
            preConditions.SetAllStates(preConditionsStates);
        }

        if (afterEffectsStates != null)
        {
            afterEffects.SetAllStates(afterEffectsStates);
        }
    }

    public virtual bool IsValid()
    {
        return true;
    }

    public bool IsValidGiven(GWorldStates conditions)
    {
        return preConditions.IsSatisfiedByWorldState(conditions);
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();

    public void StartAction()
    {
        IsRunning = PrePerform();
        this.enabled = IsRunning;
    }

    public void StopAction()
    {
        if (PostPerform())
        {
            foreach (KeyValuePair<EState, int> eff in afterEffects.WorldStates)
            {
                gAgentRef.agentBeliefs.ModifyState(eff.Key, eff.Value);
            }
        }
        this.enabled = false;
        IsRunning = false;
    }

    public bool ReachedNavDestination()
    {
        return !navAgent.pathPending && navAgent.hasPath && navAgent.remainingDistance < 1f;
    }
}