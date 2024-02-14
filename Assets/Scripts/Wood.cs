using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : SmartObject
{
    public override bool Drop()
    {
        return DefaultDrop();
    }

    public override bool Interact(out SmartObject pickup, GameObject interactor)
    {
        return DefaultPickupInteract(out pickup, interactor);
    }

    public override bool UsePickedup(Vector3 atPoint)
    {
        return false;
    }
}
