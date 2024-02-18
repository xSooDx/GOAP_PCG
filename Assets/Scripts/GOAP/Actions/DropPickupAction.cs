using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPickupAction : GAction
{
    public bool ignoreSenseRadius = true;
    public override string GetName()
    {
        return "Drom Pickup " + (string.IsNullOrWhiteSpace(targetTag)? "" : "at " + targetTag) ;
    }

    public override bool PostPerform()
    {
        return !gAgentRef.sensor.HasPickup;
    }

    public override bool PrePerform()
    {
        if(gAgentRef.sensor.HasPickup)
        {
            if (!string.IsNullOrWhiteSpace(targetTag))
            {
                GameObject[] objs = GameObject.FindGameObjectsWithTag(targetTag);
                if (objs != null)
                {
                    GameObject closestObj = null;
                    float closestDistance = float.MaxValue;
                    foreach (GameObject obj in objs)
                    {
                        float distToObject = Vector3.Distance(transform.position, obj.transform.position);
                        if (distToObject < closestDistance && (ignoreSenseRadius || distToObject <= gAgentRef.sensor.sensorRadius))
                        {
                            closestObj = obj;
                            closestDistance = distToObject;
                        }
                    }
                    if (closestObj)
                    {
                        target = closestObj;
                        navAgent.destination = target.transform.position;
                        return true;
                    }
                }
                return false;
            }
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        NavigationUpdate(() =>
        {
            gAgentRef.sensor.DropPickup();
            StopAction();
        });
    }
}
