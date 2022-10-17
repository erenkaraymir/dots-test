using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

partial class AddForceSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        Entities.ForEach((Entity sphere, ref PhysicsVelocity physicsVelocity,ref PhysicsMass physicsMass,in MoveForceData moveForceData) => {
            if (Input.GetKeyDown(moveForceData.inputKey))
            {
                var forceVector = (float3)Vector3.up * moveForceData.ForceAmount * deltaTime;
                physicsVelocity.ApplyLinearImpulse(physicsMass, forceVector);
                Debug.Log("TIklandý");
            }
        }).Schedule();
    }  
}
