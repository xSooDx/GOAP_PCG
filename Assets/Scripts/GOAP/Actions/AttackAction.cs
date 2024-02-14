using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : GAction
{
    public float AttackRange = 1.5f;

    public override float GetCost()
    {
        if (gAgentRef.sensor.TryGetObjectOfTag(targetTag, out GameObject target))
        {
            return base.GetCost() + (Vector3.Distance(target.transform.position, transform.position) - AttackRange) / 5f;
        }
        return base.GetCost();
    }

    public override string GetName()
    {
        return "Attack Action: " + targetTag;
    }

    public override bool PostPerform()
    {
        return true;
    }

    public override bool PrePerform()
    {
        target = null;
        if (gAgentRef.sensor.TryGetObjectOfTag(targetTag, out target))
        {
            navAgent.destination = target.transform.position;
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
        if (navAgent.ReachedNavDestination(AttackRange))
        {
            if (target)
            {
                gAgentRef.sensor.UsePickup(target.transform.position);
                StopAction();
            }
            else
            {
                StopAction(true);
            }

        }
    }
}
