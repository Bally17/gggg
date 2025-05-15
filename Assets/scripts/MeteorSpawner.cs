using UnityEngine;
using System.Collections;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 8f;
    public MovementDirection direction = MovementDirection.LeftToRight;

    public float meteorSpeed = 3f;

    public enum MovementDirection
    {
        LeftToRight,
        RightToLeft
    }

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

        while (true)
        {
            SpawnMeteor();

            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void SpawnMeteor()
    {
        GameObject meteor = Instantiate(meteorPrefab, transform.position, Quaternion.identity);

        MeteorMover mover = meteor.GetComponent<MeteorMover>();
        if (mover != null)
        {
            switch (direction)
            {
                case MovementDirection.LeftToRight:
                    mover.SetMovement(Vector2.right, meteorSpeed);
                    break;
                case MovementDirection.RightToLeft:
                    mover.SetMovement(Vector2.left, meteorSpeed);
                    break;
            }
        }
    }
}
