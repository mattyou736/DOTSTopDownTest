using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public class EnemyFireSystem : SystemBase
{
    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;
    float rateOfFire = 1f;
    // timer until weapon and shoot again
    float shotTimer = 0;

    // Start is called before the first frame update
    protected override void OnCreate()
    {
        //cach the BeginInitializationEntityCommandBufferSystem so we dont need new ones
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
        shotTimer += Time.DeltaTime;
        if (shotTimer >= rateOfFire)
        {
            float3 playerPos = (float3)GameManager.GetPlayerPosition();

            Entities.WithName("EnemyFireSystem").WithBurst(FloatMode.Default, FloatPrecision.Standard, true).ForEach((Entity entity, int entityInQueryIndex, in EnemyFire enemyFire, in LocalToWorld location) =>
            {
                
                

                //que up spawn in buffer

                var instance = commandBuffer.Instantiate(entityInQueryIndex, enemyFire.bulletPrefab);
                var position = math.transform(location.Value, new float3(0, 0, 0));
                float3 direction = playerPos - position;
                direction.y = 0f;
                commandBuffer.SetComponent(entityInQueryIndex, instance, new Translation { Value = position });
                commandBuffer.SetComponent(entityInQueryIndex, instance, new Rotation { Value = quaternion.LookRotation(direction, math.up()) });

            }).ScheduleParallel();



            m_EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
            shotTimer = 0;
        }
        
    }

}
