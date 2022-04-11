using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkGenerator
{
    [Header("Chunk Generator settings")]
    [SerializeField] private NoiseSettings noiseSettings;
    [SerializeField] private PoolManagerTerrainSettings terrainSettings;

    [Header("Noise debug values")] 
    [SerializeField] private bool debug;
    [SerializeField] private AnimationCurve noiseCurve;
    [SerializeField] private AnimationCurve terrainsCurve;
    [SerializeField] private int[] terrains;

    public GameObject GenerateChunk(int position, bool withObstacles = true, int chunkIndex = -1)
    {
        if(chunkIndex <= -1 || chunkIndex >= terrainSettings.sampleObjects.Count)
            chunkIndex = GenerateChunkIndex(position);
        
        // for debug only
        if(debug) AppendTerrainType(position, chunkIndex);
        
        // TODO: get a difficulty level to know how to spawn obstacles
        var difficulty = 1;

        var chunk = PoolManagerTerrain.Instance.GetPooledObject(chunkIndex);

        if (!withObstacles)
            return chunk;
        
        var pivots = chunk.transform.childCount;
        var obstaclePivotList = new List<Transform>();
        for (var i = 0; i < pivots; i++)
        {
            var child = chunk.transform.GetChild(i);
            if (child.name.Contains("ObstaclePivot"))
            {
                obstaclePivotList.Add(child);
            }
        }

        if (difficulty <= 3)
        {
            var obstaclesToBeSpawned = Random.Range(0, obstaclePivotList.Count);
            
            Debug.Log($"obstacles to be spawned: {obstaclesToBeSpawned}");

            while (obstaclesToBeSpawned > 0)
            {
                obstaclesToBeSpawned--;
                
                var l = PoolManagerObstacles.Instance.SampleObstaclesLength;
                var obstacle = PoolManagerObstacles.Instance.GetPooledObject(Random.Range(0, l));
                
                var obstacleParent = obstaclePivotList[Random.Range(0, obstaclePivotList.Count)];
                obstacle.transform.parent = obstacleParent;
                obstacle.transform.localPosition = Vector3.zero;
                
                obstaclePivotList.Remove(obstacleParent);
            }
        }

        return chunk;
    }

    private int GenerateChunkIndex(int position)
    {
        var noise = Noise.GenerateNoiseValue(noiseSettings, position);
        
        // for debug only
        // Debug.Log($"position: {position} noise: {noise}");
        noiseCurve.AddKey(position, noise);
        
        var sampleTerrains = terrainSettings.sampleObjects;
        
        for (var i = 0; i < sampleTerrains.Count; i++)
        {
            if (noise >= sampleTerrains[i].minSpawnValue &&
                noise < sampleTerrains[i].maxSpawnValue)
            {
                return i;
            }
        }
        return 0;
    }

    private void AppendTerrainType(int position, int terrainIndex)
    {
        terrainsCurve.AddKey(position, terrainIndex);
        terrains[terrainIndex]++;
    }


}
