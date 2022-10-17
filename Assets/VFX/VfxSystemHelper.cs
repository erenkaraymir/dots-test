using Unity.Entities;
using UnityEngine;

partial class VfxSystemHelper : MonoBehaviour
{
    public ParticleSystem particles;

    void Start()
    {
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<VfxSystem>().Init(particles);
    }
}