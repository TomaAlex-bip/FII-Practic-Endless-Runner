using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolManagerDecorationsSettings", menuName = "ScriptableObjects/PoolManager/DecorationsPoolManager", order = 4)]
public class PoolManagerDecorationsSettings : ScriptableObject
{
    public List<GameObject> sampleObjects;
    public int sizeOfPoolPerObject;
}
