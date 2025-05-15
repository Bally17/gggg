using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    private int moveDirection = 1;

    public EnemySpawner spawner;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float minFireInterval = 1f;
    public float maxFireInterval = 3f;
    public AudioClip fireSound;
    public AudioClip explosionSound;
    private bool isDying = false;
    public GameObject shootParticlePrefab;


    void Start()
    {
        moveDirection = Random.value > 0.5f ? 1 : -1;
        UpdateVisualDirection();

        // Spusti náhodnú streľbu
        Invoke("Shoot", Random.Range(minFireInterval, maxFireInterval));
    }

    void Update()
    {
        transform.Translate(Vector2.right * moveDirection * moveSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(fireSound, transform.position);

            GameObject particle = Instantiate(shootParticlePrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(180f, 0f, 0f));
            Destroy(particle, 1f);
        }



        // Znova zavolaj Shoot po náhodnom čase
        Invoke("Shoot", Random.Range(minFireInterval, maxFireInterval));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            moveDirection *= -1;
            UpdateVisualDirection();
        }

        if (collision.CompareTag("PlayerB") && !isDying)
        {
            isDying = true;

            AudioSource.PlayClipAtPoint(explosionSound, transform.position);

            Destroy(collision.gameObject);
            GameManager.Instance.AddEnemyKill();
            spawner.EnemyDied(gameObject);
            Destroy(gameObject);
        }

    }

    void UpdateVisualDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * moveDirection;
        transform.localScale = scale;
    }
}
