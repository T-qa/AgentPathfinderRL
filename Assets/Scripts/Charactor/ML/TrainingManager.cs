using System.Collections;
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
    private Vector2 playerSpawnMin = new Vector2(-25f, -12f);
    private Vector2 playerSpawnMax = new Vector2(-14f, -3f);
    private Vector2 monsterSpawnMin = new Vector2(-25f, -12f);
    private Vector2 monsterSpawnMax = new Vector2(-14f, -3f);

    // Parent offset
    private Vector3 parentOffset = new Vector3(19.5f, 0f, 0f);

    private QLearner _qLearner;

    void Start()
    {
        _qLearner = monsterAI.qLearner;
        StartCoroutine(RunTraining());
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
            // Update statistics display
            statsDisplay.UpdateStatistics(_qLearner);

            // Respawn player and monster
            SpawnEntity(playerAI.transform, playerSpawnMin, playerSpawnMax);
            SpawnEntity(monsterAI.transform, monsterSpawnMin, monsterSpawnMax);
        }
    }

    private void SpawnEntity(Transform entityTransform, Vector2 minSpawn, Vector2 maxSpawn)
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(minSpawn.x, maxSpawn.x) + parentOffset.x,
            Random.Range(minSpawn.y, maxSpawn.y),
            0f
        );
        entityTransform.position = spawnPosition;
    }
}
