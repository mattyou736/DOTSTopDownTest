using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;


public class MovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        //run this logic on ewvery entity with moveforward
        Entities.WithAll<MoveForward>().ForEach((ref Translation trans, ref Rotation rot, ref MoveForward moveForward) =>
        {
            //calculate speed en move it forward with math functions
            trans.Value += moveForward.speed * Time.DeltaTime * math.forward(rot.Value);
        });
    }
}
