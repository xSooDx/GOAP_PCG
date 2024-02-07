using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{
    public static GWorld Instance { get; private set; } = new GWorld();
    public static GWorldStates World { get; private set;}

    static GWorld()
    {
        World = new GWorldStates();
    }

    private GWorld() {}
}
