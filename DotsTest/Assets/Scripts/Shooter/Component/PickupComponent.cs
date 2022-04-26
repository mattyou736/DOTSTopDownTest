using Unity.Entities;

[GenerateAuthoringComponent]
public struct PickupComponent : IComponentData
{
    public Entity pickupPrefab;

}
