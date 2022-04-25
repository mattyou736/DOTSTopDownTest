using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class DestructionSystem : ComponentSystem
{
    float thresholdDis = 2f;

    protected override void OnUpdate()
    {
        //get player pos to use
        float3 playerPos = (float3)GameManager.GetPlayerPosition();

        //loop through everything with the tag to be able to destroy it
        Entities.WithAll<EnemyTag>().ForEach((Entity enemy, ref Translation enemyPos) =>
        {
            playerPos.y = enemyPos.Value.y;

            //if player and enemy overlap
            if (math.distance(enemyPos.Value, playerPos) <= thresholdDis)
            {
                //destroy the entity
                PostUpdateCommands.DestroyEntity(enemy);
            }

            float3 enemyPosition = enemyPos.Value; 

            //if they overlap with bullets
            Entities.WithAll<BulletTag>().ForEach((Entity bullet, ref Translation bulletPos) =>
            {
                if (math.distance(enemyPosition, bulletPos.Value) <= thresholdDis)
                {
                    PostUpdateCommands.DestroyEntity(enemy);
                    PostUpdateCommands.DestroyEntity(bullet);
                }

            });
        });
    }
}
