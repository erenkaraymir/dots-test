using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

partial class VfxSystem : SystemBase
{
    UnityEngine.ParticleSystem particleSystem;
    Transform particleSystemTransform;
    protected override void OnCreate()
    {
        base.OnCreate();
        Enabled = false; // Dont run the system until we have set everything up
    }

    public void Init(UnityEngine.ParticleSystem particleSystem)
    {
        this.particleSystem = particleSystem;
        particleSystemTransform = particleSystem.transform;
        Enabled = true; // Everything is ready, can begin running the system
    }

    protected override void OnUpdate()
    {
        // Empty for now
    }
}