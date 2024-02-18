using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeAction : GAction
{
    [SerializeField] float fleeDistance = 20f;
    public override string GetName()
    {
        return "Flee " + targetTag;
    }

    public override bool PostPerform()
    {
        return true;
    }

    public override bool PrePerform()
    {
        if (gAgentRef.sensor.TryGetObjectOfTag(targetTag, out target) && target)
        {
            Vector3 dir = transform.position - target.transform.position;
            dir = dir.normalized * fleeDistance;
            navAgent.destination = dir;
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (navAgent.ReachedNavDestination(1f))
        {
            StopAction();
        }
    }
}
