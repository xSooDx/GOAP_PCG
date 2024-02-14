using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EState
{
    Idle,
    Tired,

    SeenTree,
    SeesWood,
    SeesPlayer,
    SeesAxe,
    SeesSword,
    SeesBow,

    PickedUpWood,
    BuiltHouse,

    HasSword,
    HasBow,
    HasAxe,

    TreeCut,

    PlayerIsClose,
    KilledPlayer,
}
