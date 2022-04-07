using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance { get; private set; }

    [SerializeField] private Transform player;
    
    [SerializeField] private int chunksVisibleInFront = 10;
    [SerializeField] private int chunksVisibleBack = 5;

    [SerializeField] private float minMoveDistanceThreshold = 0.5f;

    [SerializeField] private int chunkStartOffset = 0;
    [SerializeField] private float chunkSize = 10f;

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

        if (Mathf.Abs(playerPosition - oldPlayerPosition) >= minMoveDistanceThreshold)
        {
            oldPlayerPosition = playerPosition;
            UpdateChunks();
        }
    }

    private void UpdateChunks()
    {
        currentChunkCoord = Mathf.RoundToInt(playerPosition / chunkSize);

        // iterate through each allowed current indexes and instantiate, if necessary, the chunk 
        for (var offset = -chunksVisibleBack; offset <= chunksVisibleInFront; offset++)
        {
            var inRangeCoord = currentChunkCoord + offset;

            if (!visibleChunks.ContainsKey(inRangeCoord) && inRangeCoord >= chunkStartOffset)
            {
                var chunkToSpawn = PoolManagerTerrain.Instance.GetPooledObject();
                if (chunkToSpawn == null) 
                    continue;
                
                chunkToSpawn.transform.parent = transform;
                chunkToSpawn.transform.localPosition = Vector3.forward * inRangeCoord * chunkSize;
                visibleChunks[inRangeCoord] = chunkToSpawn;

            }
        }

        // check for chunks out of "render distance"
        var leftBehindChunks = new List<int>();
        foreach (var chunk in visibleChunks)
        {
            if (chunk.Key < currentChunkCoord - chunksVisibleBack || 
                chunk.Key > currentChunkCoord + chunksVisibleInFront)
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
