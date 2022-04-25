using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EnemyFireAuthoring : MonoBehaviour, IDeclareReferencedPrefabs
{
    public GameObject bulletPrefab;

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(bulletPrefab);
    }

    
}
