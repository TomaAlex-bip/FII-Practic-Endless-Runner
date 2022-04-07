using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance { get; private set; }

    // TODO: replace with singleton
    [SerializeField] private Transform player;

    [SerializeField] private MapGeneratorData data;

    private readonly Dictionary<int, GameObject> visibleChunks = new Dictionary<int, GameObject>();

    private float playerPosition;
    private float oldPlayerPosition;

    private int currentChunkCoord;
    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        oldPlayerPosition = player.position.z;
        // initialize the start map
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

    private void UpdateChunks()
    {
        currentChunkCoord = Mathf.RoundToInt(playerPosition / data.chunkSize);

        // iterate through each allowed current indexes and instantiate, if necessary, the chunk 
        for (var offset = -data.chunksVisibleBack; offset <= data.chunksVisibleInFront; offset++)
        {
            var inRangeCoord = currentChunkCoord + offset;

            if (!visibleChunks.ContainsKey(inRangeCoord) && inRangeCoord >= data.chunkStartOffset)
            {
                var chunkToSpawn = PoolManagerTerrain.Instance.GetPooledObject();
                if (chunkToSpawn == null) 
                    continue;
                
                chunkToSpawn.transform.parent = transform;
                chunkToSpawn.transform.localPosition = Vector3.forward * inRangeCoord * data.chunkSize;
                visibleChunks[inRangeCoord] = chunkToSpawn;
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

        // remove the left behind chunks from the dictionary
        foreach (var chunk in leftBehindChunks)
        {
            visibleChunks.Remove(chunk);
        }
        leftBehindChunks.Clear();
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
