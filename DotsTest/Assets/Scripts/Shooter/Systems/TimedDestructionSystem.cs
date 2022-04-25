using Unity.Entities;

public class TimedDestructionSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        // loop through all entites with a Lifetime component
        Entities.WithAll<Lifetime>().ForEach((Entity entity, ref Lifetime lt) =>
        {
            lt.Value -= Time.DeltaTime;

            // removes entity when timed
            if (lt.Value <= 0)
            {
                PostUpdateCommands.DestroyEntity(entity);
            }

        });
    }
}
