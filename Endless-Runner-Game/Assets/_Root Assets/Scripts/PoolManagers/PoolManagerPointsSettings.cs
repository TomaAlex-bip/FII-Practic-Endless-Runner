using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolManagerPointsSettings", menuName = "ScriptableObjects/PoolManager/PointsPoolManager", order = 2)]
public class PoolManagerPointsSettings : ScriptableObject
{
    public List<GameObject> sampleObjects;
    public int sizeOfPoolPerObject;
}
