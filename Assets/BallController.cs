using UnityEngine;

[RequireComponent(typeof(Stompable))]
public class BallController : MonoBehaviour
{
    [SerializeField]
    private float bounceHeight = 64f;

    [SerializeField]
    private float bounceDelay = 1f;

    [SerializeField]
    private bool isBouncy = true;

    private Vector2 originalPosition;
    private float elapsedTime;

    private Stompable stompable;

    void Start()
    {
        originalPosition = transform.position;
        stompable = GetComponent<Stompable>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Calculate normalized time for the bounce cycle
        float normalizedTime = (elapsedTime + bounceDelay) % bounceDelay / bounceDelay;

        // Use a parabolic easing function for smoother motion
        float yOffset = 4 * bounceHeight * normalizedTime * (1 - normalizedTime);

        // Update the position, ensuring the bounce starts from the original position
        transform.position = new Vector2(originalPosition.x, originalPosition.y + yOffset);
    }

    private void OnStomped()
    {
        Debug.Log("Ball stomped!");
        // Handle what happens when the ball is stomped (e.g., destroy or disable it)
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the bounce range in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            new Vector3(transform.position.x, originalPosition.y, transform.position.z),
            new Vector3(
                transform.position.x,
                originalPosition.y + bounceHeight,
                transform.position.z
            )
        );
    }
}
