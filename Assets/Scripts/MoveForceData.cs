using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct MoveForceData : IComponentData
{
    public float ForceAmount;
    public KeyCode inputKey;
}
