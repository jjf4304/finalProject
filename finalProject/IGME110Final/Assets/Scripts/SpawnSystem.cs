//using Unity.Burst;
//using Unity.Collections;
//using Unity.Entities;
//using Unity.Jobs;
//using Unity.Mathematics;
//using Unity.Transforms;
//using UnityEngine;

//public class SpawnSystem : JobComponentSystem
//{


//    BeginInitializationEntityCommandBufferSystem buffer;


//    protected override void OnCreate()
//    {
//        buffer = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
//    }

//    // This declares a new kind of job, which is a unit of work to do.
//    // The job is declared as an IJobForEach<Translation, Rotation>,
//    // meaning it will process all entities in the world that have both
//    // Translation and Rotation components. Change it to process the component
//    // types you want.
//    //
//    // The job is also tagged with the BurstCompile attribute, which means
//    // that the Burst compiler will optimize it for the best performance.
//    [BurstCompile]
//    struct SpawnSystemJob : IJobForEach<Translation, Rotation, SpawnerData, PlayerTracking>
//    {
//        // Add fields here that your job needs to do its work.
//        // For example,
//        //    public float deltaTime

//        public EntityCommandBuffer cmdBuffer;

//        public void Execute(ref Translation translation, [ReadOnly] ref Rotation rotation,
//            [ReadOnly] ref SpawnerData spawn, [ReadOnly] ref PlayerTracking track)
//        {
//            // Implement the work to perform for each entity here.
//            // You should only access data that is local or that is a
//            // field on this job. Note that the 'rotation' parameter is
//            // marked as [ReadOnly], which means it cannot be modified,
//            // but allows this job to run in parallel with other jobs
//            // that want to read Rotation component data.
//            // For example,
//            //     translation.Value += mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;



//            Entity entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(spawn.spawnPrefab, World.Active);

//            for (; spawn.numSpawned < spawn.numToSpawn; spawn.numSpawned++)
//            {
//                Entity instance = cmdBuffer.Instantiate(entity);
//                cmdBuffer.SetComponent(instance,
//                    new Translation
//                    {
//                        Value = new float3(spawn.rand.NextInt() % spawn.distance,
//                    0, spawn.rand.NextInt() % spawn.distance)
//                    });
//                cmdBuffer.AddComponent(instance,
//                    new PlayerTracking { thePlayerEntity = track.thePlayerEntity });
//                cmdBuffer.AddComponent(instance,
//                    new Collidable
//                    {
//                        radius = spawn.spawnPrefab.transform.localScale.x / 2,
//                        foodValue = 1,
//                        knockBackForce = 0f,
//                        size = 1
//                    });


//            }

//        }
//    }

//    protected override JobHandle OnUpdate(JobHandle inputDependencies)
//    {
//        var job = new SpawnSystemJob();

//        // Assign values to the fields on your job here, so that it has
//        // everything it needs to do its work when it runs later.
//        // For example,
//        //     job.deltaTime = UnityEngine.Time.deltaTime;

//        job.cmdBuffer = buffer.CreateCommandBuffer();


//        // Now that the job is set up, schedule it to be run. 
//        return job.Schedule(this, inputDependencies);
//    }
//}