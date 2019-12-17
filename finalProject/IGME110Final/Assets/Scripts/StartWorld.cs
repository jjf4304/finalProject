/* Mono script to handle mostly spawning of all entities
 * 
 * As far as I know you cant pass gameObjects to components
 * and use them in jobs so I handle all entity spawning/
 * component adding here with the entity manager.
 * 
 * I tried to make it a component system but I was just unable to 
 * make it work. I want to try and do it that way instead and have
 * spawners that handle each time of item to spawn to make it more
 * parallel.
 * 
 * All entity spawning follows the same style of 
 * convert it from its prefab, instantiate it with the entity manager, set its 
 * position randomly and then add all needed components for that entity.
 * 
 * 
 */ 


using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class StartWorld : MonoBehaviour
{

    public GameObject playerPrefab, levelPrefab, cameraPrefab, cowPrefab, chickenPrefab,
        plantPrefab;
    public int numPlants, numChickens, numCows;
    public float chickenSpeed, cowSpeed, amtToEatForUpgradeOne, amtToEatForUpgradeTwo;
    private Vector3 offset;
    private EntityManager manager;
    private Entity player;
    private float radiusForSpawn;

    // Start is called before the first frame update
    void Start()
    {
        manager = World.Active.EntityManager;

        //Instantiate Level
        Entity levelEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(levelPrefab, World.Active);
        Entity levelInstance = manager.Instantiate(levelEntity);

        Vector3 position = transform.TransformPoint(new Vector3(0, -1f, 0));
        manager.SetComponentData(levelInstance, new Translation { Value = position });

        //spawn bounderies being half the size of the square level x scale
        radiusForSpawn = levelPrefab.transform.localScale.x/2f;

        //Instantiate Player
        Entity playerEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(playerPrefab, World.Active);
        Entity playerInstance = manager.Instantiate(playerEntity);

        position = transform.TransformPoint(new Vector3(0, 0, 0));
        manager.SetComponentData(playerInstance, new Translation { Value = position });
        manager.AddComponentData(playerInstance, new Moveable{ moveSpeed = 10f });
        manager.AddComponentData(playerInstance, new PlayerData { size = 1, amountEaten = 0f,
            upgradeOneAmt = amtToEatForUpgradeOne, upgradeTwoAmt = amtToEatForUpgradeOne });
        manager.AddComponentData(playerInstance, new NonUniformScale { Value = 1 });

        player = playerInstance;


        //Camera
        offset = Camera.main.transform.position - position;


        //Plant spawning. 

        Unity.Mathematics.Random random = new Unity.Mathematics.Random(13);

        Entity plantEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(plantPrefab, World.Active);

        for (int i = 0; i < numPlants; i++)
        {
            Entity plantInstance = manager.Instantiate(plantEntity);
            position = transform.TransformPoint(new Vector3(random.NextInt() % radiusForSpawn,
                0, random.NextInt() % radiusForSpawn));
            manager.SetComponentData(plantInstance, new Translation { Value = position });
            manager.AddComponentData(plantInstance, new PlayerTracking { thePlayerEntity = playerInstance });
            manager.AddComponentData(plantInstance, new Collidable{radius = plantPrefab.transform.localScale.x / 2,
                    foodValue = 1, knockBackForce = 0f, size = 1});
        }

        //Instantiate Chickens.

        Entity chickenEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(chickenPrefab, World.Active);

        for(int i = 0; i < numChickens; i++)
        {
            Entity chickenInstance = manager.Instantiate(chickenEntity);
            position = transform.TransformPoint(new Vector3(random.NextInt() % radiusForSpawn,
                chickenPrefab.transform.localScale.y/2, random.NextInt() % radiusForSpawn));
            Vector3 destination2 = transform.TransformPoint(new Vector3(random.NextInt() % radiusForSpawn,
                chickenPrefab.transform.localScale.y / 2, random.NextInt() % radiusForSpawn));
            Vector3 destination3 = transform.TransformPoint(new Vector3(random.NextInt() % radiusForSpawn,
                chickenPrefab.transform.localScale.y / 2, random.NextInt() % radiusForSpawn));

            manager.SetComponentData(chickenInstance, new Translation { Value = position });
            manager.AddComponentData(chickenInstance, new Moveable { moveSpeed = chickenSpeed });
            manager.AddComponentData(chickenInstance, new PlayerTracking { thePlayerEntity = playerInstance });
            manager.AddComponentData(chickenInstance, new Collidable{ radius = chickenPrefab.transform.localScale.x / 2,
                    foodValue = 3, knockBackForce = 2f, size = 2});
            manager.AddComponentData(chickenInstance, new NPC { pos1 = position, pos2 = destination2, pos3 = destination3,
                goingTo1 = false, goingTo2 = true, goingTo3 = false, runningFromPlayer = false});
        }


        //Instantiate Cows
        Entity cowEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(cowPrefab, World.Active);

        for (int i = 0; i < numCows; i++)
        {
            Entity cowInstance = manager.Instantiate(cowEntity);
            position = transform.TransformPoint(new Vector3(random.NextInt() % radiusForSpawn,
                cowPrefab.transform.localScale.y/2, random.NextInt() % radiusForSpawn));
            Vector3 destination2 = transform.TransformPoint(new Vector3(random.NextInt() % radiusForSpawn,
                cowPrefab.transform.localScale.y / 2, random.NextInt() % radiusForSpawn));
            Vector3 destination3 = transform.TransformPoint(new Vector3(random.NextInt() % radiusForSpawn,
                cowPrefab.transform.localScale.y / 2, random.NextInt() % radiusForSpawn));
            manager.SetComponentData(cowInstance, new Translation { Value = position });
            manager.AddComponentData(cowInstance, new Moveable { moveSpeed = cowSpeed });
            manager.AddComponentData(cowInstance, new PlayerTracking { thePlayerEntity = playerInstance });
            manager.AddComponentData(cowInstance, new Collidable { radius = cowPrefab.transform.localScale.x / 2,
                foodValue = 5, knockBackForce = 5f, size = 3});
            manager.AddComponentData(cowInstance, new NPC { pos1 = position, pos2 = destination2, pos3 = destination3,
                goingTo1 = false, goingTo2 = true, goingTo3 = false, runningFromPlayer = false
            });
        }

    }

    //Update Camera Position. As far as it seems, its noteasy to entity use a camera and I had 
    // a lot of trouble doing it, so I just update it here.
    private void Update()
    {
        Vector3 playerPos = manager.GetComponentData<Translation>(player).Value;
        Debug.Log(offset);
        Camera.main.transform.position = playerPos + offset;
    }

}
