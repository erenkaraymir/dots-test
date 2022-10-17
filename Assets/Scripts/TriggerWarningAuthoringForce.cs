using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

public class TriggerWarningAuthoringForce : MonoBehaviour, IConvertGameObjectToEntity
{
    private Entity entityParticle;
    private BlobAssetStore blobAsset;
    private World defaultWorld;
    private EntityManager entityManager;
    private Translation objTransform;
    [SerializeField] float3 forceDirection;

    bool _isCompelete = false;

    [SerializeField] string _message = "How dare you!";
    void IConvertGameObjectToEntity.Convert(Entity e, EntityManager dst, GameObjectConversionSystem cs)
    {
        if (!enabled) return;

        dst.AddComponentData(e, new TriggerWarning
        {
            ForceDirection = forceDirection,
            ObjTransform = objTransform,
            EntityManager = entityManager,
            Message = _message
        });
    }

    private void Awake()
    {
        blobAsset = new BlobAssetStore();
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;
    }
}

public struct TriggerWarning : IComponentData
{
    public FixedString4096Bytes Message;
    public Entity EntityParticle;
    public EntityManager EntityManager;
    public Translation ObjTransform;
    public float3 ForceDirection;
}

[BurstCompile]
public struct TriggerWarningJob : ICollisionEventsJob
{
    public EntityCommandBuffer ecb;
    [ReadOnly] public ComponentDataFromEntity<TriggerWarning> TriggerWarningData;
    // public ComponentDataFromEntity<RotationSpeed> BallWarningData;
    public BoolComp boolComp;
    public void Execute(CollisionEvent collisionEvent)
    {
        Entity entityA = collisionEvent.EntityA;
        Entity entityB = collisionEvent.EntityB;
        if (entityA != Entity.Null && entityB != Entity.Null)
        {
            //bool isBodyACube = TriggerWarningData.HasComponent(entityA);
            bool isBodyBCube = TriggerWarningData.HasComponent(entityB);

            if (isBodyBCube)
            {
                //Entity entityTemp = ecb.Instantiate(TriggerWarningData[entityB].EntityParticle);
                ecb.SetComponent(entityA, new PhysicsVelocity { Linear = TriggerWarningData[entityB].ForceDirection});
                Debug.Log("x");

            }
        }
    }
}
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(ExportPhysicsWorld))]
[UpdateBefore(typeof(EndFramePhysicsSystem))]
partial class TwitterSimulationSystemForce : SystemBase
{
    public EntityManager EntityManagerX = World.DefaultGameObjectInjectionWorld.EntityManager;
    StepPhysicsWorld _stepPhysicsWorldSystem;
    private EndSimulationEntityCommandBufferSystem endECBSystem;

    protected override void OnCreate()
    {
        endECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        _stepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
        RequireForUpdate(GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[] { typeof(TriggerWarning) }
        }));
    }
    protected override void OnStartRunning()
    {
        base.OnStartRunning();
        this.RegisterPhysicsRuntimeSystemReadOnly();
    }

    protected override void OnUpdate()
    {
        var job = new TriggerWarningJob
        {
            ecb = endECBSystem.CreateCommandBuffer(),
            TriggerWarningData = GetComponentDataFromEntity<TriggerWarning>(isReadOnly: true),

        };
        Dependency = job.Schedule(_stepPhysicsWorldSystem.Simulation, Dependency);
        endECBSystem.AddJobHandleForProducer(Dependency);
    }
}
