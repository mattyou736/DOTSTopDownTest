using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Entities;



namespace DOTSTest
{
    public class RotateAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private float degreesPerSecond;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            //passing value of radians
            dstManager.AddComponentData(entity, new Rotate { radiansPerSecond = math.radians(degreesPerSecond) });
            dstManager.AddComponentData(entity, new RotationEulerXYZ());
        }
    }

}
