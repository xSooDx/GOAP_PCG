using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : GAction
{
    public float idleTime = 2f;

    float timer;
    public override float GetCost()
    {
        return 1;
    }

    public override string GetName()
    {
        return "Idle Action";
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
        if(timer < 0)
        {
            StopAction();
        }
    }
}
