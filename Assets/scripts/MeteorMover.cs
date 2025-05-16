using UnityEngine;

public class MeteorMover : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    public float rotationSpeed = 180f;

    public void SetMovement(Vector2 direction, float speed)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
    }

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        if (transform.position.x < -20 || transform.position.x > 20 || transform.position.y < -20 || transform.position.y > 20)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerB"))
        {
            Destroy(collision.gameObject); 
        }
        if (collision.CompareTag("EnemyB"))
        {
            Destroy(collision.gameObject); 
        }
    }
}
