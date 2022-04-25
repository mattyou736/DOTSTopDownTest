using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

//makes it so we can drag on prefab
[GenerateAuthoringComponent]
public struct MoveForward : IComponentData
{
    public float speed;

}
