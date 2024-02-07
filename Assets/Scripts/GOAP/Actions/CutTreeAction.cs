using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTreeAction : GAction
{
    Tree t;
    GameObject wood;

    public override float GetCost()
    {
        return 1f;
    }

    public override string GetName()
    {
        return "Cut Tree";
    }

    public override bool PrePerform()
    {
        target = gAgentRef.memory.objMemory["Tree"];
        if(target)
        {
            t = target.GetComponent<Tree>();
            if(t)
            {
                navAgent.destination = target.transform.position;
                return true;
            }
        }
        return false;
    }

    public override bool PostPerform()
    {
        if(wood)
        {
            return gAgentRef.sensor.CheckObject(wood, "Wood");
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
        if(ReachedNavDestination())
        {
            wood = t.Interact();
            StopAction();
        }
    }
}
