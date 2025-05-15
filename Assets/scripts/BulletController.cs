using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;

    void Start()
    {
        Destroy(gameObject, 3f);
    }


    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
