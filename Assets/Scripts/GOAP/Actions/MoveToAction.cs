using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAction : GAction
{
    public override float GetCost()
    {
        return 1.0f;
    }

    public override string GetName()
    {
        return "Move To " + targetTag;
    }

    public override bool IsValid()
    {
        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }

    public override bool PrePerform()
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
                    if (distToObject < closestDistance && distToObject > Range)
                    {
                        closestObj = obj;
                        closestDistance = distToObject;
                    }
                }
                if (closestObj)
                {
                    target = closestObj;
                    Vector3 offset = Random.insideUnitSphere * Range;
                    offset.y = 0;
                    navAgent.destination = target.transform.position + offset;
                    return true;
                }
            }
            return false;
        }
        return false;
    }


    // Update is called once per frame
    void Update()
    {
        NavigationUpdate(() =>
        {
            StopAction();
        }, false);
    }
}
