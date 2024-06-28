using System.Collections;
using System.IO;
using UnityEngine;

public class SarsaTrainingManager : MonoBehaviour
{
    [Header("Training Settings")]
    public float maxEpisodeTime = 15f;

    [Header("References")]
    public SarsaMonsterAI monsterAI;
    public PlayerAI playerAI;
    public SarsaStatisticsDisplay statsDisplay;

    // Manual spawn ranges based on your specific bounds
    private Vector2 playerSpawnMin = new Vector2(-25f, -11f);
    private Vector2 playerSpawnMax = new Vector2(-14f, -3f);
    private Vector2 monsterSpawnMin = new Vector2(-25f, -11f);
    private Vector2 monsterSpawnMax = new Vector2(-14f, -3f);

    private SarsaLearner _sarsaLearner;

    private string logFilePath = "Assets/Scripts/sarsa_training_metrics.csv";
    private StreamWriter logWriter;

    private int episodeCount = 0; // Episode counter

    void Start()
    {
        _sarsaLearner = monsterAI.sarsaLearner;
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
            _sarsaLearner.EndEpisode();

            // Log metrics to CSV file
            LogTrainingMetrics();

            // Update statistics display
            statsDisplay.UpdateStatistics(_sarsaLearner);

            // Respawn player and monster
            SpawnEntity(playerAI.transform, playerSpawnMin, playerSpawnMax);
            SpawnEntity(monsterAI.transform, monsterSpawnMin, monsterSpawnMax);

            episodeCount++; // Increment episode counter
        }
    }

    private void LogTrainingMetrics()
    {
        float averageStepsPerEpisode =
            (float)_sarsaLearner.TotalSteps / _sarsaLearner.TotalEpisodes;
        float averageRewardPerEpisode = _sarsaLearner.TotalReward / _sarsaLearner.TotalEpisodes;

        string logMessage = string.Format(
            "{0},{1},{2},{3},{4},{5}",
            _sarsaLearner.TotalEpisodes,
            _sarsaLearner.TotalSteps,
            averageStepsPerEpisode,
            averageRewardPerEpisode,
            _sarsaLearner.HighestReward,
            _sarsaLearner.ConvergenceQValue
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
