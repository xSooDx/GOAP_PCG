using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : GAction
{
    public float idleTime = 2f;
    public List<GWorldState> breakOnAnyState = new();
    float timer;
    public override float GetCost()
    {
        return 1;
    }

    public override string GetName()
    {
        return "Idle";
    }

    public override bool PrePerform()
    {
        timer = idleTime;
        return true;
    }

    public override bool PostPerform()
    {
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            StopAction();
            return;
        }

        foreach (var state in breakOnAnyState)
        {
            if(gAgentRef.agentBeliefs.DoesSatisfyState(state.key, state.value))
            {
                StopAction(true);
                break;
            }
        }
        
    }
}
