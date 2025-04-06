using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Chunk[] possibleChunks;

    [SerializeField]
    private GameObject startingChunk;

    [SerializeField]
    [Tooltip("Number of chunks to load in front of the player")]
    private int chunksForward = 3;

    [SerializeField]
    [Tooltip("Number of chunks to keep alive behind the player")]
    private int chunksBackward = 2;

    [SerializeField]
    private Chunk[] debugChunks;

    [SerializeField]
    private bool useDebugChunks = false;

    public PlayerController player;
    private List<Chunk> loadedChunks;
    private int lastChunkIndex = 0;

    public Chunk currentChunk;

    int GetChunkIndex()
    {
        return lastChunkIndex++;
    }

    void LoadNextChunk()
    {
        var lastChunk = loadedChunks.Last();

        var nextChunkPrefab = possibleChunks[UnityEngine.Random.Range(0, possibleChunks.Length)];

        if (useDebugChunks && debugChunks.Length > 0)
        {
            // Use a random debug chunk
            nextChunkPrefab = debugChunks.First();
            debugChunks = debugChunks.Skip(1).ToArray();
        }

        // Get the world position of the last chunk's exitPoint
        var currentExitPos = lastChunk.transform.TransformPoint(lastChunk.exitPoint);

        // Get the local position of the next chunk's entryPoint
        var entryPointLocal = nextChunkPrefab.entryPoint;

        // Calculate the spawn position
        var spawnPosition = currentExitPos - (Vector3)entryPointLocal;

        // Instantiate and add the new chunk
        var newChunk = Instantiate(nextChunkPrefab, spawnPosition, Quaternion.identity);
        AddChunk(newChunk);
    }

    public void OnPlayerEnterChunk(Chunk chunk)
    {
        currentChunk = chunk;
        if (lastChunkIndex < chunk.chunkIndex + chunksForward)
        {
            LoadNextChunk();
        }
    }

    void AddChunk(Chunk chunk)
    {
        chunk.chunkIndex = GetChunkIndex();
        var oldName = chunk.gameObject.name;
        chunk.gameObject.name = "Chunk " + chunk.chunkIndex + " (" + oldName + ")";
        loadedChunks.Add(chunk);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();

        if (startingChunk == null)
        {
            throw new System.Exception("Starting chunk is not set");
        }
        loadedChunks = new List<Chunk>();
        var firstChunk = startingChunk.GetComponent<Chunk>();
        AddChunk(firstChunk);
        currentChunk = firstChunk;
        foreach (var i in Enumerable.Range(0, chunksForward))
        {
            LoadNextChunk();
        }
    }

    // Update is called once per frame
    void Update() { }
}
