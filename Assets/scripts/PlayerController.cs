using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float moveSpeed = 5f;
    public float fireRate = 0.5f;
    public float bulletSpeed = 10f;
    public AudioClip fireSound;
    public AudioClip impactSound;

    private float fireTimer;
    public GameObject gameOverPanel;
    private float playerHealth = 4;
    public GameObject three;
    public GameObject two;
    public GameObject one;
    public GameObject shootParticlePrefab;

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * moveX * moveSpeed * Time.deltaTime);

        fireTimer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && fireTimer >= fireRate)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            AudioSource.PlayClipAtPoint(fireSound, transform.position);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.up * bulletSpeed;
            }

            GameObject particle = Instantiate(shootParticlePrefab, firePoint.position, firePoint.rotation);
            Destroy(particle, 1f);

            fireTimer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyB"))
        {
            playerHealth --;

            if (playerHealth >= 3) {
                three.SetActive(true);
                AudioSource.PlayClipAtPoint(impactSound, transform.position);
                StartCoroutine(ShakeEffect(0.2f, 0.1f));
            } else if (playerHealth == 2) {
                three.SetActive(false);
                two.SetActive(true);
                AudioSource.PlayClipAtPoint(impactSound, transform.position);
                StartCoroutine(ShakeEffect(0.2f, 0.1f));
            } else if (playerHealth == 1) {
                two.SetActive(false);
                one.SetActive(true);
                AudioSource.PlayClipAtPoint(impactSound, transform.position);
                StartCoroutine(ShakeEffect(0.2f, 0.1f));
            } else {
                Time.timeScale = 0f;

                if (gameOverPanel != null)
                    gameOverPanel.SetActive(true);
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator ShakeEffect(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(originalPos.x + offsetX, originalPos.y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

}
