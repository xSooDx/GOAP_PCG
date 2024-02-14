using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWoodAction : GAction
{
    public override float GetCost()
    {
        return 1f;
    }

    public override string GetName()
    {
        return "Pickup wood";
    }

    public override bool PostPerform()
    {
        return true;
    }

    public override bool PrePerform()
    {
        if(gAgentRef.sensor.TryGetObjectOfTag("Wood", out target))
        {
            navAgent.destination = target.transform.position;
            return target;
        }
        return false;
    }

    private void Update()
    {
        if(navAgent.ReachedNavDestination(1f))
        {
            Destroy(target.gameObject);
            StopAction();
        }
    }
}
