using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    void Start()
    {
        Destroy(gameObject, 5f); // Zničenie po čase
    }
}
