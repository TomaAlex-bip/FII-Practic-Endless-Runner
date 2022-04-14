using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkGenerator
{
    [Header("Chunk Generator settings")]
    [SerializeField] private NoiseSettings noiseSettings;
    [SerializeField] private PoolManagerTerrainSettings terrainSettings;
    
    [Header("Spawn chances")]
    [Range(0f, 1f)]
    [SerializeField] private float pointsSpawnChance = 0.3f;
    [Range(0f, 1f)] 
    [SerializeField] private float powerupSpawnChance = 0.25f;

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
        
        var difficulty = GameManager.Instance.Difficulty;
        var maxDifficulty = GameManager.Instance.MaxDifficulty;

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

        var obstaclesToBeSpawned = 1;
        if (difficulty <= maxDifficulty / 2)
        {
            obstaclesToBeSpawned = Random.Range(0, difficulty+1);
        }
        else
        {
            obstaclesToBeSpawned = Random.Range(Mathf.Abs(difficulty - maxDifficulty/2), difficulty+1);
        }
        obstaclesToBeSpawned = Mathf.Min(obstaclesToBeSpawned, obstaclePivotList.Count);

        // Debug.Log($"obstacles to be spawned: {obstaclesToBeSpawned} difficulty: {difficulty} pivots: {obstaclePivotList.Count} ");

        while (obstaclesToBeSpawned > 0)
        {
            obstaclesToBeSpawned--;
            
            var l = PoolManagerObstacles.Instance.SampleObjectsLength;
            var obstacle = PoolManagerObstacles.Instance.GetPooledObject(Random.Range(0, l));
            
            var obstacleParent = obstaclePivotList[Random.Range(0, obstaclePivotList.Count)];
            obstacle.transform.parent = obstacleParent;
            obstacle.transform.localPosition = Vector3.zero;
            
            obstaclePivotList.Remove(obstacleParent);

            if (obstacle.name.Contains("Cubes"))
            {
                var rng = Random.Range(0f, 1f);
                if (rng <= pointsSpawnChance)
                {
                    var points = PoolManagerPoints.Instance.GetPooledObject(1);
                    points.transform.parent = obstacleParent;
                    points.transform.localPosition = Vector3.zero;
                }
            }
            else if (obstacle.name.Contains("Laser"))
            {
                var rng = Random.Range(0f, 1f);
                if (rng <= pointsSpawnChance)
                {
                    var points = PoolManagerPoints.Instance.GetPooledObject(2);
                    points.transform.parent = obstacleParent;
                    points.transform.localPosition = Vector3.zero;
                }
            }
            else if (obstacle.name.Contains("Door"))
            {
                var rng = Random.Range(0f, 1f);
                if (rng <= pointsSpawnChance)
                {
                    var points = PoolManagerPoints.Instance.GetPooledObject(0);
                    points.transform.parent = obstacleParent;
                    points.transform.localPosition = Vector3.zero;
                }
            }
        }

        //spawn points on the empty pivots, only here can spawn power-ups
        foreach (var pivot in obstaclePivotList)
        {
            var rng = Random.Range(0f, 1f);
            if (rng <= pointsSpawnChance)
            {
                rng = Random.Range(0f, 1f);
                if (rng <= powerupSpawnChance)
                {
                    var points = PoolManagerPoints.Instance.GetPooledObject(Random.Range(3, 5));
                    points.transform.parent = pivot;
                    points.transform.localPosition = Vector3.zero;
                }
                else
                {
                    var points = PoolManagerPoints.Instance.GetPooledObject(0);
                    points.transform.parent = pivot;
                    points.transform.localPosition = Vector3.zero;
                }
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
