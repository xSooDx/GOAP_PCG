using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAction : GAction
{
    public override float GetCost()
    {
        if (gAgentRef.sensor.TryGetObjectOfTag(targetTag, out GameObject target))
        {
            return base.GetCost() + (Vector3.Distance(target.transform.position, transform.position)) / 5f;
        }
        return base.GetCost() + gAgentRef.sensor.sensorRadius / 5f;
    }

    public override string GetName()
    {
        return "Pick up " + targetTag;
    }

    public override bool PostPerform()
    {
        return true;
    }

    public override bool PrePerform()
    {
        if (gAgentRef.sensor.TryGetObjectOfTag(targetTag, out target) && target)
        {
            if(!target.GetComponent<SmartObject>()) return false;

            navAgent.destination = target.transform.position;
            return true;
        }
        return false;
    }

    private void Update()
    {
        NavigationUpdate(() =>
        {
            target.GetComponent<SmartObject>().Interact(out SmartObject pickup, gameObject);
            if (pickup)
            {
                gAgentRef.sensor.PickUp(pickup);
                StopAction();
            }
        });
    }
}
