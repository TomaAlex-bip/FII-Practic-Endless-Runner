using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapGeneratorData", menuName = "ScriptableObjects/MapGeneratorSO", order = 1)]
public class MapGeneratorData : ScriptableObject
{
    public int chunksVisibleInFront = 10;
    public int chunksVisibleBack = 5;
    public float minMoveDistanceThreshold = 0.5f;
    public int chunkStartOffset = 0;
    public int spawnStartOffset = 0;
    public float chunkSize = 10f;
    
    public int decorationsVisibleInFront = 5;
    public int decorationsVisibleBack = 2;
    public float decorationSize = 20f;
}
