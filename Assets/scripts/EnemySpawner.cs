using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    public float spawnDelay = 3f;
    public float waveDelay = 5f;

    public int maxConcurrentEnemies = 5;

    private List<GameObject> activeEnemies = new List<GameObject>();

    private int currentWave = 1;
    private int killsThisWave = 0;
    private int killsNeeded = 10;
    private int maxEnemiesThisWave = 10;
    private int enemiesSpawnedThisWave = 0;

    private bool spawningNextWave = false;

    public GameObject wavePanel;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        spawningNextWave = false;
        killsThisWave = 0;
        enemiesSpawnedThisWave = 0;

        GameManager.Instance.UpdateWaveText(currentWave);

        while (enemiesSpawnedThisWave < maxEnemiesThisWave)
        {
            if (activeEnemies.Count < maxConcurrentEnemies && IsSpawnAreaClear())
            {
                SpawnEnemy();
                enemiesSpawnedThisWave++;
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        activeEnemies.Add(enemy);

        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.spawner = this;
        }
    }

    bool IsSpawnAreaClear()
    {
        float checkRadius = 0.5f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(spawnPoint.position, checkRadius);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                return false;
            }
        }

        return true;
    }

    public void EnemyDied(GameObject enemy)
    {
        if (!activeEnemies.Contains(enemy)) return;

        activeEnemies.Remove(enemy);
        killsThisWave++;

        if (killsThisWave >= maxEnemiesThisWave && !spawningNextWave)
        {
            spawningNextWave = true;
            StartCoroutine(NextWave());
        }
    }

    IEnumerator NextWave()
    {
        if (wavePanel != null)
            wavePanel.SetActive(true);

        yield return new WaitForSeconds(waveDelay);

        if (wavePanel != null)
            wavePanel.SetActive(false);

        // Zvýš vlna a priprav parametre
        currentWave++;
        maxEnemiesThisWave = 10 + (currentWave - 1) * 2;
        maxConcurrentEnemies = Mathf.Min(10, 5 + (currentWave - 1));
        killsNeeded = maxEnemiesThisWave;

        StartCoroutine(SpawnWave());
    }
}
