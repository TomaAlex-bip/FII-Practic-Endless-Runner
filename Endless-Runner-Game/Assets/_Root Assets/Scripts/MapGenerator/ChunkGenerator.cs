using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkGenerator
{
    [Header("Chunk Generator settings")]
    [SerializeField] private NoiseSettings noiseSettings;
    [SerializeField] private PoolManagerTerrainSettings terrainSettings;

    [Header("Noise debug values")]
    [SerializeField] private AnimationCurve noiseCurve;
    [SerializeField] private AnimationCurve terrainsCurve;
    [SerializeField] private int[] terrains;
    
    public GameObject GenerateChunk(int position)
    {
        var chunkIndex = GenerateChunkIndex(position);
        
        // for debug only
        AppendTerrainType(position, chunkIndex);
        
        // TODO: generate obstacles on terrain
        
        return PoolManagerTerrain.Instance.GetPooledObject(chunkIndex);
    }

    private int GenerateChunkIndex(int position)
    {
        var noise = Noise.GenerateNoiseValue(noiseSettings, position);
        
        // for debug only
        Debug.Log($"position: {position} noise: {noise}");
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
