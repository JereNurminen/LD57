using System.Collections;
using UnityEngine;

public class DaggerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float range = 62f;

    [SerializeField]
    private float waitTime = 1f;

    [SerializeField]
    private float rotationSpeed = 360f;

    [SerializeField]
    private LayerMask wallMask;

    private float timeSincePlayerInRange = 0f;
    private bool isShooting = false;
    private Vector2 shootDirection;

    private PlayerController playerController;
    private Rigidbody2D rb;

    private Coroutine resetCoroutine;
    private Quaternion targetRotation = Quaternion.Euler(0, 0, 0); // Default to pointing upwards

    private Animator animator;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isShooting)
        {
            var distanceToPlayer = Vector2.Distance(
                transform.position,
                playerController.transform.position
            );

            if (distanceToPlayer < range)
            {
                RotateTowardsPlayer();

                timeSincePlayerInRange += Time.deltaTime;
                if (timeSincePlayerInRange >= waitTime)
                {
                    ShootTowardsPlayer();
                }
            }
            else
            {
                timeSincePlayerInRange = 0f;
            }
        }

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        speed = 0;
        rb.linearVelocity = Vector2.zero; // Stop the dagger's movement
        animator.SetTrigger("death");
    }

    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }

    private void RotateTowardsPlayer()
    {
        Vector2 direction = (playerController.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        targetRotation = Quaternion.Euler(0, 0, angle); // Update the target rotation
    }

    private void ShootTowardsPlayer()
    {
        isShooting = true;
        shootDirection = (playerController.transform.position - transform.position).normalized;
        rb.linearVelocity = shootDirection * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (wallMask == (wallMask | (1 << collision.gameObject.layer)))
        {
            // Reflect the dagger's velocity based on the collision normal
            Vector2 normal = collision.contacts[0].normal;
            shootDirection = Vector2.Reflect(shootDirection, normal);
            rb.linearVelocity = shootDirection * speed;

            // Update the target rotation to point in the new direction
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg - 90f;
            targetRotation = Quaternion.Euler(0, 0, angle);

            // Reset to idle state after 2 seconds
            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
            }
            resetCoroutine = StartCoroutine(ResetToIdleAfterDelay(2f));
        }
    }

    private IEnumerator ResetToIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        rb.linearVelocity = Vector2.zero;
        isShooting = false;
        timeSincePlayerInRange = 0f;

        // Set the target rotation to point upwards
        targetRotation = Quaternion.Euler(0, 0, 0);
    }
}
