using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[RequiresEntityConversion]
public class EnemyFireAuthoring : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public GameObject bulletPrefab;
    public float spawnDelay;
    public float spawnTimer;

    //declare refrenced prefab for sytem ahead of time
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(bulletPrefab);
    }

    //let you convert editor data rep to entity optimal runtime rep
    public void Convert(Entity entity,EntityManager dstManager,GameObjectConversionSystem conversionSystem)
    {
        var enemyFire = new EnemyFire
        {
            //because of DeclareReferencedPrefabs bulletPrefab will convert
            //we map the gameobject to the entity reference of said prefab
            bulletPrefab = conversionSystem.GetPrimaryEntity(bulletPrefab),
            spawnDelay = spawnDelay,
            spawnTimer = spawnTimer

        };
        dstManager.AddComponentData(entity, enemyFire);
    }
    
}
