using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int enemiesDestroyed = 0;
    public TextMeshProUGUI enemyCounterText;
    public TextMeshProUGUI timerText;

    public static GameManager Instance;

    private float elapsedTime = 0f;
    private bool isRunning = true;

    public TextMeshProUGUI waveText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    public void AddEnemyKill()
    {
        enemiesDestroyed++;
        UpdateEnemyText();
    }

    void UpdateEnemyText()
    {
        enemyCounterText.text = "" + enemiesDestroyed;
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("cas: {0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void UpdateWaveText(int wave)
    {
        if (waveText != null)
            waveText.text = "Vlna: " + wave;
    }

}
