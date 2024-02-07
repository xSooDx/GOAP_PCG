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
        return "MoveToAction";
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
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
