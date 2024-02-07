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
        if(gAgentRef.memory.objMemory.ContainsKey("Wood"))
        {
            target = gAgentRef.memory.objMemory["Wood"];
            navAgent.destination = target.transform.position;
            return target;
        }
        return false;
    }

    private void Update()
    {
        if(ReachedNavDestination())
        {
            Destroy(target.gameObject);
            StopAction();
        }
    }
}
