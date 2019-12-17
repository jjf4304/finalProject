/* Component script to describe a collidable object.
 * 
 * Has data for:
 * radius - radius to be collided with
 * knockBackForce - Not used yet, but would be how much a larger object
 *                  would knock back a player
 * foodValue - how much food a collided object gives the player
 * size - the size category of the collidable object
 */ 


using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Collidable : IComponentData
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

    public float radius;
    public float knockBackForce;
    public float foodValue;
    public int size;
}
