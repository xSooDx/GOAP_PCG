using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    [SerializeField] GWorldState[] preConditionsStates;
    [SerializeField] GWorldState[] afterEffectsStates;
    [SerializeField] float baseCost = 1f;
    [SerializeAs("AttackRange")] public float Range = 1.5f;

    public delegate void NavDestinationReachedDelegate();
    public NavMeshAgent navAgent;

    public GWorldStates preConditions;
    public GWorldStates afterEffects;

    public GameObject target;
    [TagSelector]
    public string targetTag;

    public GAgent gAgentRef;
    public float navUpdateTime = 1f;
    float navTimer;

    public bool WasSuccess { get; private set; }
    // Visual Debugger
    [SerializeField] bool IsRunning;

    public GAction()
    {
        preConditions = new GWorldStates();
        afterEffects = new GWorldStates();
    }

    public abstract string GetName();
    public abstract bool PrePerform();
    public abstract bool PostPerform();

    public virtual float GetCost()
    {
        return baseCost;
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
        return conditions.DoesSatisfyWorldState(preConditions);
    }

    public void StartAction()
    {
        IsRunning = PrePerform();
        this.enabled = IsRunning;
        navAgent.isStopped = false;
    }

    public void StopAction(bool interupted = false)
    {
        navAgent.isStopped = true;
        if (!interupted && PostPerform())
        {
            //foreach (KeyValuePair<EState, int> eff in afterEffects.WorldStates)
            //{
            //    //gAgentRef.agentBeliefs.ModifyState(eff.Key, eff.Value);
            //    //if (!gAgentRef.agentBeliefs.DoesSatisfyState(eff.Key, eff.Value))
            //    //{
            //    //    Debug.LogWarning($"{gameObject.name} - '{GetName()}' Was Success but state '{eff.Key}' was not updated to '{eff.Value}'");
            //    //}
            //}
            WasSuccess = true;
        }
        else
        {
            WasSuccess = false;
        }
        this.enabled = false;
        IsRunning = false;
    }

    public void NavigationUpdate(NavDestinationReachedDelegate delgate, bool hasTarget = true)
    {
        navTimer -= Time.deltaTime;
        if (hasTarget && !target)
        {
            StopAction(true);
        }
        else if (navAgent.ReachedNavDestination(Range))
        {
            delgate.Invoke();
        }
        else if(navTimer < 0)
        {
            if(hasTarget)
                navAgent.destination = target.transform.position;
            navTimer = navUpdateTime;
        }
    }
}