using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isEnabled = true;
    public Vector2 offset;
    public bool followImmediately = true;
    public float followSpeed = 5f;
    private PlayerController playerController;
    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        gameManager = FindFirstObjectByType<GameManager>();
        /*
        cameraBounds = GameObject
            .Find("Level Manager")
            .GetComponent<LevelManager>()
            .GetComponent<BoxCollider2D>();
            */
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isEnabled)
        {
            return;
        }
        Vector3 targetPosition =
            playerController.transform.position + new Vector3(offset.x, offset.y, -10);
        Vector3 newPosition = followImmediately
            ? targetPosition
            : Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        float cameraHalfWidth =
            Camera.main.orthographicSize * ((float)Screen.width / Screen.height);
        float cameraHalfHeight = Camera.main.orthographicSize;

        float minX = gameManager.currentChunk.cameraMinX + cameraHalfWidth;
        float maxX = gameManager.currentChunk.cameraMaxX - cameraHalfWidth;

        // If the camera's target position is outside the bounds, don't move it
        if (targetPosition.x < minX || targetPosition.x > maxX)
        {
            targetPosition.x = transform.position.x;
        }

        transform.position = newPosition;
    }
}
