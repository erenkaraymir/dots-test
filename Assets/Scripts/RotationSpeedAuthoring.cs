using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class RotationSpeedAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float DegreesPerSecond = 360;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var data = new RotationSpeed
        {
            RadiansPerSecond = math.radians(DegreesPerSecond)
        };

        dstManager.AddComponentData(entity, data);
    }
}