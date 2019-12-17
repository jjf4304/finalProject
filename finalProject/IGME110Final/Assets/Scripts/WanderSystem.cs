/*
 * System for handling wandering for npc cows or chickens.
 * 
 * 
 */ 


using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class WanderSystem : JobComponentSystem
{

    // This declares a new kind of job, which is a unit of work to do.
    // The job is declared as an IJobForEach<Translation, Rotation>,
    // meaning it will process all entities in the world that have both
    // Translation and Rotation components. Change it to process the component
    // types you want.
    //
    // The job is also tagged with the BurstCompile attribute, which means
    // that the Burst compiler will optimize it for the best performance.
    [BurstCompile]
    struct WanderSystemJob : IJobForEach<Translation, Rotation, NPC, Moveable, PlayerTracking>
    {
        // Add fields here that your job needs to do its work.
        // For example,
        public float deltaTime;
        public Translation playerPos;        
        
        public void Execute(ref Translation translation, [ReadOnly] ref Rotation rotation,
            ref NPC npc, [ReadOnly] ref Moveable move, [ReadOnly] ref PlayerTracking player)
        {
            // Implement the work to perform for each entity here.
            // You should only access data that is local or that is a
            // field on this job. Note that the 'rotation' parameter is
            // marked as [ReadOnly], which means it cannot be modified,
            // but allows this job to run in parallel with other jobs
            // that want to read Rotation component data.
            // For example,
            //translation.Value += mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;

            //Not yet implemented
            if (npc.runningFromPlayer)
                return;
            else
            {

                /* Set a destination depending on if the npc entity is going to 
                 * its first, second or third position.
                 * 
                 * After that, find a normal float3 pointing in that direction and move
                 * towards it until the entity is within 1 of it. After that, depending on
                 * what position its at, set the goingTo bool correctly and next 
                 * iteration start moving in that direction
                 */ 
                float3 destination;
                if (npc.goingTo1)
                    destination = npc.pos1;
                else if (npc.goingTo2)
                    destination = npc.pos2;
                else
                    destination = npc.pos3;

                float3 direction = math.normalize(destination - translation.Value);

                translation.Value += mul(rotation.Value, direction) * move.moveSpeed * deltaTime;

                float distance = math.distancesq(translation.Value, destination);

                if(distance < 1f)
                {
                    if (npc.goingTo1)
                    {
                        npc.goingTo1 = false;
                        npc.goingTo2 = true;
                    }
                    else if (npc.goingTo2)
                    {
                        npc.goingTo2 = false;
                        npc.goingTo3 = true;
                    }
                    else
                    {
                        npc.goingTo3 = false;
                        npc.goingTo1 = true;
                    }
                }
            }

        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new WanderSystemJob();
        
        // Assign values to the fields on your job here, so that it has
        // everything it needs to do its work when it runs later.
        // For example,
        job.deltaTime = UnityEngine.Time.deltaTime;

        
        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(this, inputDependencies);
    }
}