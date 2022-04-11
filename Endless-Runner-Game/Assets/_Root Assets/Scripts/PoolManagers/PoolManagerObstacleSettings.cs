using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolManagerObstacleSettings", menuName = "ScriptableObjects/PoolManager/ObstaclePoolManager", order = 2)]
public class PoolManagerObstacleSettings : ScriptableObject
{
    public List<GameObject> sampleObjects;
    public int sizeOfPoolPerObject;
}
