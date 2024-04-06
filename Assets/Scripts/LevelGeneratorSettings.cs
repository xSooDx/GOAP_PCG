using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelGeneratorSettings
{
    [Range(0, 360)] public float baseAngle;
    [Range(1, 10)] public int levelMinLength;
    [Range(1, 10)] public int levelMaxLength;
    [Range(0, 180)] public float minAngle;
    [Range(0, 180)] public float maxAngle;

    [Header("Rooms & Halls")]
    [Range(1, 100)] public float roomMinSize;
    [Range(1, 100)] public float roomMaxSize;
    [Range(1, 500)] public float connectorMinLength;
    [Range(1, 500)] public float connectorMaxLength;

    [Header("Branching")]
    [Range(0, 1)] public float branchingFactor;
    [Range(1, 100)] public int branchRoomMinSize;
    [Range(1, 100)] public int branchRoomMaxSize;
    [Range(1, 200)] public int branchMinLength;
    [Range(1, 200)] public int branchMaxLength;
}
