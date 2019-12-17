/* Component for a spawners data but not currently used.
 * 
 * Since I cant use gameObjects in jobs (At least visual studio threw errors)
 * I didn't end up using this. I want to eventually use it since I think it
 * will benefit performance.
 * 
 * 
 */ 


using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct SpawnerData : IComponentData
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

    //public GameObject spawnPrefab;
    public int numToSpawn, numSpawned;
    public float distance;
    public Unity.Mathematics.Random rand;
}
