using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class FacePlayerSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        float3 playerPos = (float3)GameManager.GetPlayerPosition();

        //go through each of the entities with a enemy tag 
        Entities.WithAll<EnemyTag>().ForEach((Entity entity,ref Translation trans, ref Rotation rot) =>
        {
            //calculate vectors
            float3 direction = playerPos - trans.Value;
            direction.y = 0f;
            //set correct heading
            rot.Value = quaternion.LookRotation(direction, math.up());
        });
    }
}
