using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public class TimeManagerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        throw new System.NotImplementedException();
    }
}

partial class TimeManager : SystemBase
{
    protected override void OnUpdate()
    {
        float elapsedTime = Time.DeltaTime;
        Debug.Log(elapsedTime);
        Entities.ForEach((ref Translation translation, ref BoolComp boolComp, in Rotation rotation) => {
            if (elapsedTime > 5f)
            {
                boolComp.isOkey = true;
                elapsedTime = 0f;
                Debug.Log("1''");
            }
        }).Schedule();
    }
}
