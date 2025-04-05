using System;
using UnityEngine;
using UnityEngine.Events;

public class Chunk : MonoBehaviour
{
    public Vector2 entryPoint;
    public Vector2 exitPoint;
    public float cameraMinX;
    public float cameraMaxX;
    public int chunkIndex;

    private GameManager gameManager;
    private bool playerInChunk = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (
            entryPoint.y > gameManager.player.transform.position.y
            && gameManager.player.transform.position.y > exitPoint.y
        )
        {
            if (!playerInChunk)
            {
                playerInChunk = true;
                gameManager.OnPlayerEnterChunk(this);
            }
        }
        else
        {
            playerInChunk = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name + " entered chunk: " + chunkIndex);
        //gameManager.OnPlayerEnterChunk(this);
    }

    void OnDrawGizmos()
    {
        var localEntryPoint = transform.TransformPoint(entryPoint);
        var localExitPoint = transform.TransformPoint(exitPoint);
        var localCameraMinX = transform.TransformPoint(new Vector2(cameraMinX, 0)).x;
        var localCameraMaxX = transform.TransformPoint(new Vector2(cameraMaxX, 0)).x;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(localEntryPoint, 8f);
        Gizmos.DrawLine(localEntryPoint, localExitPoint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(localExitPoint, 8f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            new Vector2(localCameraMinX, localEntryPoint.y),
            new Vector2(localCameraMinX, localExitPoint.y)
        );

        Gizmos.DrawLine(
            new Vector2(localCameraMaxX, localEntryPoint.y),
            new Vector2(localCameraMaxX, localExitPoint.y)
        );
    }
}
