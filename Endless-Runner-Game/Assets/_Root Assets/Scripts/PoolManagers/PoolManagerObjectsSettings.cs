using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolManagerObjectsSettings", menuName = "ScriptableObjects/PoolManager/ObjectsPoolManager", order = 1)]
public class PoolManagerObjectsSettings : ScriptableObject
{
    public List<GameObject> sampleObjects;
    public int sizeOfPoolPerObject;
}
