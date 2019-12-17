/* Component to describe a NPC entity, effectively a moving
 * entity that isnt the player (so chickens and cows).
 * 
 * Has data for:
 * 
 * positions 1 2 and 3 - float3 data for the three positions
 *                      the entity moves between.
 * radiusToRun - not used at the moment, but distance from player to begin
 *               running from the player.
 * radiusUntilStopRunning - not used at the moment, but the distance from 
 *                          player that the entity will return to wandering
 *                          between its positions.
 * goingTo 1, 2, 3 - bools to control/set when the entity will move towards what
 *                   positions.
 * runningFromPlayer - bool to control if the entity is running from player or not
 *                     and if true, dont wander to next position until they stop 
 *                     running away.
 */ 

using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct NPC : IComponentData
{
    // Add fields to your component here. Remember that:
    //
    // * A component itself is for storing data and doesn't 'do' anything.
    //
    // * To act on the data, you will need a System.
    //
    // * Data in a component must be blittable, which means a component can
    //   only contain fields which are primitive types or other blittable
    //   structs; they cannot contain references to classes.
    //
    // * You should focus on the data structure that makes the most sense
    //   for runtime use here. Authoring Components will be used for 
    //   authoring the data in the Editor.

    public float3 pos1, pos2, pos3;
    public float radiusUntilRun, radiusUntilStopRunning;
    public bool goingTo1, goingTo2, goingTo3;
    public bool runningFromPlayer;
}
