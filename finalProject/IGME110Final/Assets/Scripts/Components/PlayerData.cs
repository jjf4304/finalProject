/* Component script to describe the data a player will have.
 * Includes:
 * 
 * size - the size of the player slime, used for colliding with npcs.
 * amountEaten - the amount the player has eaten. used to determine size.
 * upgradeOneAmt - the amount of food needed to upgrade from size 1 to 2
 * updgradeTwoAmt - the amount of food needed to upgrade from size 2 to 3
 */ 


using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct PlayerData : IComponentData
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

    public float size;
    public float amountEaten, upgradeOneAmt, upgradeTwoAmt;
}
