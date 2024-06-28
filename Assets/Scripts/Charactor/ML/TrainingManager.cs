using System.Collections;
using System.IO;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [Header("Training Settings")]
    public float maxEpisodeTime = 15f;

    [Header("References")]
    public MLMonsterAI monsterAI;
    public PlayerAI playerAI;
    public StatisticsDisplay statsDisplay;

    // Manual spawn ranges based on your specific bounds
    private Vector2 playerSpawnMin = new Vector2(-25f, -11f);
    private Vector2 playerSpawnMax = new Vector2(-14f, -3f);
    private Vector2 monsterSpawnMin = new Vector2(-25f, -11f);
    private Vector2 monsterSpawnMax = new Vector2(-14f, -3f);

    private QLearner _qLearner;

    private string logFilePath = "Assets/Scripts/training_metrics.csv";
    private StreamWriter logWriter;

    private int episodeCount = 0; // Episode counter

    void Start()
    {
        _qLearner = monsterAI.qLearner;
        InitializeLogFile();
        StartCoroutine(RunTraining());
    }

    void InitializeLogFile()
    {
        // Initialize the CSV file and write headers if it doesn't exist
        if (!File.Exists(logFilePath))
        {
            logWriter = new StreamWriter(logFilePath, true);
            logWriter.WriteLine(
                "TotalEpisodes,TotalSteps,AverageStepsPerEpisode,AverageRewardPerEpisode,HighestReward, ConvergenceQValue"
            );
            logWriter.Close();
        }
    }

    private IEnumerator RunTraining()
    {
        while (true)
        {
            // Spawn player and monster at valid positions
            SpawnEntity(playerAI.transform, playerSpawnMin, playerSpawnMax);
            SpawnEntity(monsterAI.transform, monsterSpawnMin, monsterSpawnMax);

            // Start episode timer
            float episodeStartTime = Time.time;

            // Continue episode until time limit is reached
            while (Time.time - episodeStartTime < maxEpisodeTime)
            {
                yield return null; // Wait for the next frame
            }

            // End of episode
            _qLearner.EndEpisode();

            // Log metrics to CSV file
            LogTrainingMetrics();

            // Update statistics display
            statsDisplay.UpdateStatistics(_qLearner);

            // Respawn player and monster
            SpawnEntity(playerAI.transform, playerSpawnMin, playerSpawnMax);
            SpawnEntity(monsterAI.transform, monsterSpawnMin, monsterSpawnMax);

            episodeCount++; // Increment episode counter
        }
    }

    private void LogTrainingMetrics()
    {
        float averageStepsPerEpisode = (float)_qLearner.TotalSteps / _qLearner.TotalEpisodes;
        float averageRewardPerEpisode = _qLearner.TotalReward / _qLearner.TotalEpisodes;

        string logMessage = string.Format(
            "{0},{1},{2},{3},{4},{5}",
            _qLearner.TotalEpisodes,
            _qLearner.TotalSteps,
            averageStepsPerEpisode,
            averageRewardPerEpisode,
            _qLearner.HighestReward,
            _qLearner.ConvergenceQValue
        );

        logWriter = new StreamWriter(logFilePath, true);
        logWriter.WriteLine(logMessage);
        logWriter.Close();
    }

    private void SpawnEntity(Transform entityTransform, Vector2 minSpawn, Vector2 maxSpawn)
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(minSpawn.x, maxSpawn.x),
            Random.Range(minSpawn.y, maxSpawn.y),
            0f
        );
        entityTransform.position = spawnPosition;
    }
}
