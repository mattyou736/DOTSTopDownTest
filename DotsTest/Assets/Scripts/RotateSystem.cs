using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;


namespace DOTSTest
{
    public class RotateSystem : JobComponentSystem
    {
        [BurstCompile]
        private struct RotateJob : IJobForEach<RotationEulerXYZ, Rotate>
        {
            public float deltaTime;
            public void Execute(ref RotationEulerXYZ euler, ref Rotate rotate)
            {
                euler.Value.y += rotate.radiansPerSecond * deltaTime;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new RotateJob { deltaTime = Time.DeltaTime};
            return job.Schedule(this, inputDeps);
        }

    }
}
