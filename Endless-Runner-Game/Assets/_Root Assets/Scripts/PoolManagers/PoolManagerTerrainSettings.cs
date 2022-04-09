using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolManagerTerrainSettings", menuName = "ScriptableObjects/PoolManager/TerrainPoolManager", order = 1)]
public class PoolManagerTerrainSettings : ScriptableObject
{
    public List<MapTerrain> sampleObjects;
    public int sizeOfPoolPerObject;
}
