using Unity.Entities;

[GenerateAuthoringComponent]
public struct EnemyFire : IComponentData
{
    public Entity bulletPrefab;
    public float spawnDelay;
    public float spawnTimer;
}
