using UnityEngine;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Entities;
using Unity.Collections;


public class EnemySpawner : MonoBehaviour
{
    //entity components
    [SerializeField] 
    Mesh enemyMesh;
    [SerializeField] 
    Material enemyMat;
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField] 
    float minSpeed = 5f;
    [SerializeField] 
    float maxSpeed = 15f;

    Entity enemyEntityPrefab;

    //this will proccess entitiy data
    private EntityManager entityManager;
    bool canSpawn;
    float spawnTimer;

    [SerializeField]
    int spawnAmount = 50;
    [SerializeField] 
    float spawnInterval = 5f;
    [SerializeField] 
    float spawnRadius = 30f;

    private float3 RandomPoint(float radius)
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized * radius;

        // return random point on circle, centered around the player position
        return new float3(randomPoint.x, 0.5f, randomPoint.y) + (float3)GameManager.GetPlayerPosition();
    }

    // Start is called before the first frame update
    void Start()
    {
        // grab reference to the entity world
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        //setting up deafault conversion settings
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        enemyEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(enemyPrefab, settings);

        //spawn;
        StartSpawn();
        SpawnWave();
    }


    // Update is called once per frame
    void Update()
    {
        // disable if the game has just started or if player is dead
        if (!canSpawn)
        {
            return;
        }

        // count up until next spawn
        spawnTimer += Time.deltaTime;

        // spawn and reset timer
        if (spawnTimer > spawnInterval)
        {
            SpawnWave();
            spawnTimer = 0;
        }
    }

    public void StartSpawn()
    {
        canSpawn = true;
    }

    void SpawnWave()
    {
        //makes sure it doesnt presist past setup
        NativeArray<Entity> enemyArray = new NativeArray<Entity>(spawnAmount, Allocator.Temp);
        
        // instanciate enemys
        for (int i = 0; i < enemyArray.Length; i++)
        {
            enemyArray[i] = entityManager.Instantiate(enemyEntityPrefab);

            // find point to spawn within radius
            entityManager.SetComponentData(enemyArray[i], new Translation { Value = RandomPoint(spawnRadius) });

            entityManager.SetComponentData(enemyArray[i], new MoveForward { speed = Random.Range(minSpeed, maxSpeed) });
        }
        
        //frees up memory
        enemyArray.Dispose();
    }
}
