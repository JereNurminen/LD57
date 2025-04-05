using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isEnabled = true;
    public Vector2 offset;
    public float followSpeed = 5f;
    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
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
        Vector3 newPosition = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );

        float cameraHalfWidth =
            Camera.main.orthographicSize * ((float)Screen.width / Screen.height);
        float cameraHalfHeight = Camera.main.orthographicSize;

        /*
        float minX = cameraBounds.bounds.min.x + cameraHalfWidth;
        float maxX = cameraBounds.bounds.max.x - cameraHalfWidth;
        float minY = cameraBounds.bounds.min.y + cameraHalfHeight;
        float maxY = cameraBounds.bounds.max.y - cameraHalfHeight;
        */

        transform.position = newPosition;
    }
}
