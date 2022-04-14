using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance { get; private set; }

    [SerializeField] private MapGeneratorData data;

    [SerializeField] private ChunkGenerator chunkGenerator;

    private readonly Dictionary<int, GameObject> visibleChunks = 
        new Dictionary<int, GameObject>();
    
    private readonly Dictionary<int, GameObject> visibleDecorations = 
        new Dictionary<int, GameObject>();

    private Transform player;
    private float playerPosition;
    private float oldPlayerPosition;

    private int currentChunkCoord;
    private int currentDecorationCoord;
    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        player = PlayerMovement.Instance.transform;
        oldPlayerPosition = player.position.z;
        
        // initialize the start map
        InitializeSpawnChunks();
        UpdateChunks();
    }

    private void Update()
    {
        playerPosition = player.position.z;

        if (Mathf.Abs(playerPosition - oldPlayerPosition) >= data.minMoveDistanceThreshold)
        {
            oldPlayerPosition = playerPosition;
            UpdateChunks();
        }
    }

    [ContextMenu("Update chunks")]
    private void UpdateChunks()
    {
        currentChunkCoord = Mathf.RoundToInt(playerPosition / data.chunkSize);
        currentDecorationCoord = Mathf.RoundToInt(playerPosition / data.decorationSize);

        // iterate through each allowed current indexes and instantiate, if necessary, the chunk 
        for (var offset = -data.chunksVisibleBack; offset <= data.chunksVisibleInFront; offset++)
        {
            var inRangeChunkCoord = currentChunkCoord + offset;
            
            if (!visibleChunks.ContainsKey(inRangeChunkCoord) && inRangeChunkCoord >= data.chunkStartOffset)
            {
                var chunkToSpawn = chunkGenerator.GenerateChunk(inRangeChunkCoord);
                
                if (chunkToSpawn == null) 
                    continue;
                
                chunkToSpawn.transform.parent = transform;
                chunkToSpawn.transform.localPosition = Vector3.forward * inRangeChunkCoord * data.chunkSize;
                visibleChunks[inRangeChunkCoord] = chunkToSpawn;
            }
        }

        for (var offset = -data.decorationsVisibleBack; offset <= data.decorationsVisibleInFront; offset++)
        {
            var inRangeDecorationCoord = currentDecorationCoord + offset;
            
            if (!visibleDecorations.ContainsKey(inRangeDecorationCoord) && inRangeDecorationCoord >= data.chunkStartOffset)
            {
                var decorationToSpawn = DecorationGenerator.GenerateRandomDecoration();
                decorationToSpawn.transform.parent = transform;
                decorationToSpawn.transform.localPosition = Vector3.forward * inRangeDecorationCoord * data.decorationSize;
                visibleDecorations[inRangeDecorationCoord] = decorationToSpawn;
            }
        }

        // check for chunks out of "render distance"
        var leftBehindChunks = new List<int>();
        foreach (var chunk in visibleChunks)
        {
            if (chunk.Key < currentChunkCoord - data.chunksVisibleBack || 
                chunk.Key > currentChunkCoord + data.chunksVisibleInFront)
            {
                var chunkToDespawn = chunk.Value;
                // add the chunks in a separate list, because we cannot remove them
                // from the dictionary while iterating through it
                leftBehindChunks.Add(chunk.Key);
                PoolManagerTerrain.Instance.SendBackInPool(chunkToDespawn);
            }
        }
        
        var leftBehindDecorations = new List<int>();
        foreach (var decoration in visibleDecorations)
        {
            if (decoration.Key < currentDecorationCoord - data.decorationsVisibleBack || 
                decoration.Key > currentDecorationCoord + data.decorationsVisibleInFront)
            {
                var decorationToDespawn = decoration.Value;
                // add the chunks in a separate list, because we cannot remove them
                // from the dictionary while iterating through it
                leftBehindDecorations.Add(decoration.Key);
                PoolManagerDecorations.Instance.SendBackInPool(decorationToDespawn);
            }
        }

        // remove the left behind chunks from the dictionary
        foreach (var chunk in leftBehindChunks)
        {
            visibleChunks.Remove(chunk);
        }
        leftBehindChunks.Clear();
        
        foreach (var deco in leftBehindDecorations)
        {
            visibleDecorations.Remove(deco);
        }
        leftBehindDecorations.Clear();
    }

    private void InitializeSpawnChunks()
    {
        for (var i = data.spawnStartOffset; i < data.chunkStartOffset; i++)
        {
            var chunkToSpawn = chunkGenerator.GenerateChunk(i, false, 0);
            chunkToSpawn.transform.parent = transform;
            chunkToSpawn.transform.localPosition = Vector3.forward * i * data.chunkSize;
            visibleChunks[i] = chunkToSpawn;
            
            var decorationToSpawn = DecorationGenerator.GenerateRandomDecoration();
            decorationToSpawn.transform.parent = transform;
            decorationToSpawn.transform.localPosition = Vector3.forward * i * data.decorationSize;
            visibleDecorations[i] = decorationToSpawn;
        }
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
