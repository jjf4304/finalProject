/* System to handle collisions of all food items with the player.
 * 
 * ForEach for all entities that could collide with the player, then ForEach
 * inside that to get the player entity so I can access the Translation. I know
 * I should be able to use EntityQuery's to do that in a jobsystem, but I couldn't
 * get it to work and find the player entity.
 * 
 * ForEach for all collidable entities, then assign the data members needed in 
 * the next ForEach. In the next foreach to get player translation component and 
 * scale component. Get the distance from the collidable entity to the player and 
 * if its within the collider radius, check if the player is big enough to eat the
 * npc and if so, add food to the player, increase size if needed then delete the 
 * collidable entity.
 * 
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;

public class CollisionSystem : ComponentSystem
{

    protected override void OnUpdate()
    {
        Entities.ForEach((Entity npcEntity, ref Translation npcTranslation, ref Collidable npcCollider, ref PlayerTracking playerTrack) =>
        {
            float3 npcLocation = npcTranslation.Value;
            float3 direction = new float3(0, 0, 0);
            float distance = 0f;
            int npcSize = npcCollider.size;
            float foodAmount = npcCollider.foodValue;
            float colliderDist = npcCollider.radius;
            bool deleteEntity = false;

            Entities.ForEach((Entity playerEntity, ref Translation playerTranslate, ref PlayerData pData, ref NonUniformScale scale) =>
            {
                distance = math.distancesq(playerTranslate.Value, npcLocation);

                if (distance < (colliderDist * colliderDist) + 2)
                {
                    if(pData.size >= npcSize)
                    {
                        pData.amountEaten += foodAmount;
                        if (pData.size == 1 && pData.amountEaten >= pData.upgradeOneAmt)
                        {
                            scale.Value = 2;
                            pData.size = 2;
                        }
                        else if(pData.size == 2 && pData.amountEaten >= pData.upgradeTwoAmt)
                        {
                            scale.Value = 3;
                            pData.size = 3;
                        }
                        deleteEntity = true;
                    }
                }
            });
            if (deleteEntity)
                EntityManager.DestroyEntity(npcEntity);
        });
    }
}
