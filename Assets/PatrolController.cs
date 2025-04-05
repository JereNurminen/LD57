using UnityEngine;

enum PatrolDirection
{
    Left,
    Right
}

[RequireComponent(typeof(CollisionDetector2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PatrolController : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;

    private CollisionDetector2D collisionDetector;
    private PatrolDirection patrolDirection = PatrolDirection.Right;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collisionDetector = GetComponent<CollisionDetector2D>();
    }

    bool DetectLedge()
    {
        return false;
        // Check if the player is at the edge of a ledge
        Vector2 origin = (Vector2)transform.position + new Vector2(0, -0.5f);
        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            1f,
            collisionDetector.collisionMask
        );
        return hit.collider == null;
    }

    bool DetectWall()
    {
        return collisionDetector.IsTouchingWall(
            patrolDirection == PatrolDirection.Right ? Vector2.right : Vector2.left
        );
    }

    public void Die()
    {
        speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the enemy is at the edge of a ledge
        if (DetectWall() || DetectLedge())
        {
            // Change direction
            patrolDirection =
                patrolDirection == PatrolDirection.Right
                    ? PatrolDirection.Left
                    : PatrolDirection.Right;
        }

        // Move the enemy in the current direction
        Vector2 movement = new Vector2(
            (patrolDirection == PatrolDirection.Right ? 1 : -1) * speed * Time.deltaTime,
            0
        );
        transform.Translate(movement);
    }
}
