/* System to handle player movement using the ecs Physics components
 * 
 * Uses velocity like the standard unity rigidbody velocity physics.
 * As long as there is input, add to velocity appropriately. when no
 * input, slow down that velocity until the slime has stopped.
 * 
 * Effectively simulate friction since I don't believe currently 
 * the Physics as I understand it has it.
 * 
 */ 


using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using Unity.Physics;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class PlayerMoveSystem : JobComponentSystem
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
    struct PlayerMoveSystemJob : IJobForEach<Translation, Rotation, Moveable, PhysicsStep, PhysicsVelocity, PlayerData>
    {
        // Add fields here that your job needs to do its work.
        // For example,
        //    public float deltaTime;
        public float deltaTime;
        public float horizMove, vertMove;

        //LOOK UP PHYSICS VELOCITY AND SUCH
        public void Execute(ref Translation translation, [ReadOnly] ref Rotation rotation,
            [ReadOnly] ref Moveable pMove, ref PhysicsStep step, ref PhysicsVelocity velocity, [ReadOnly] ref PlayerData player)
        {
            // Implement the work to perform for each entity here.
            // You should only access data that is local or that is a
            // field on this job. Note that the 'rotation' parameter is
            // marked as [ReadOnly], which means it cannot be modified,
            // but allows this job to run in parallel with other jobs
            // that want to read Rotation component data.
            // For example,
            //     translation.Value += mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;


            //translation.Value += mul(rotation.Value, new float3(horizMove, 0, vertMove)*deltaTime*pMove.moveSpeed);

            //Add to the velocity based on player input
            velocity.Linear += new float3(horizMove * pMove.moveSpeed, step.Gravity.y, vertMove * pMove.moveSpeed)*deltaTime;

            float slowDownDivider = 10f;

            //Slow down when no input

            if (horizMove == 0 && velocity.Linear.x != 0)
            {
                velocity.Linear.x -= velocity.Linear.x/slowDownDivider;

                if (velocity.Linear.x > -.1 && velocity.Linear.x < .1)
                    velocity.Linear.x = 0;
                //else
                    //velocity.Linear.x /= 2;
            }
            if(vertMove == 0 && velocity.Linear.z != 0)
            {
                velocity.Linear.z -= velocity.Linear.z/slowDownDivider;
                if (velocity.Linear.z > -.1 && velocity.Linear.z < .1)
                    velocity.Linear.z = 0;
                //else
                    //velocity.Linear.z /= 2;
            }
            
        }
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new PlayerMoveSystemJob();

        job.horizMove = Input.GetAxis("Horizontal");
        job.vertMove = Input.GetAxis("Vertical");
        job.deltaTime = Time.deltaTime;
        

        // Now that the job is set up, schedule it to be run. 
        return job.Schedule(this, inputDependencies);
    }
}