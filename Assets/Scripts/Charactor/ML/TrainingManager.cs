using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrainingManager : MonoBehaviour
{
    [Header("Training Settings")]
    public float maxEpisodeTime = 10f; // Time limit per episode (changed to 10 seconds)

    [Header("References")]
    public MLMonsterAI monsterAI;
    public PlayerAI playerAI;
    public TilemapCollider2D[] tilemapColliders; // Assign in inspector

    // Manual spawn ranges based on your specific bounds
    private Vector2 playerSpawnMin = new Vector2(-25f, -12f);
    private Vector2 playerSpawnMax = new Vector2(-14f, -3f);
    private Vector2 monsterSpawnMin = new Vector2(-25f, -12f);
    private Vector2 monsterSpawnMax = new Vector2(-14f, -3f);

    // Parent offset
    private Vector3 parentOffset = new Vector3(19.5f, 0f, 0f);

    // Reference to QLearner instance
    private QLearner _qLearner;

    void Start()
    {
        StartCoroutine(RunTraining());
    }

    private IEnumerator RunTraining()
    {
        while (true)
        {
            // Spawn player and monster at valid positions
            SpawnPlayer();
            SpawnMonster();

            // Start episode timer
            float episodeStartTime = Time.time;

            // Continue episode until time limit is reached
            while (Time.time - episodeStartTime < maxEpisodeTime)
            {
                // Perform training logic here
                // Example: Move player and monster, interact, update Q-learning, etc.
                yield return null; // Wait for the next frame
            }

            // Respawn player and monster
            RespawnPlayer();
            RespawnMonster();

            // Notify QLearner of episode completion
            // _qLearner.NotifyEpisodeComplete();
        }
    }

    private void SpawnPlayer()
    {
        Vector3 spawnPosition;
        bool positionSafe = false;
        int safetyAttempts = 10; // Limit attempts to avoid infinite loops

        do
        {
            // Get random spawn position within manual player spawn ranges
            float randomX = Random.Range(playerSpawnMin.x, playerSpawnMax.x);
            float randomY = Random.Range(playerSpawnMin.y, playerSpawnMax.y);
            spawnPosition = new Vector3(randomX, randomY, 0f) + parentOffset;

            // Check if the spawn position is safe (not colliding with tilemap colliders)
            positionSafe = !CheckCollision(spawnPosition);

            safetyAttempts--;

            if (safetyAttempts <= 0)
            {
                Debug.LogWarning(
                    "Failed to find safe player spawn position after several attempts."
                );
                break;
            }
        } while (!positionSafe);

        // Set player position
        playerAI.transform.position = spawnPosition;
    }

    private void SpawnMonster()
    {
        Vector3 spawnPosition;
        bool positionSafe = false;
        int safetyAttempts = 10; // Limit attempts to avoid infinite loops

        do
        {
            // Get random spawn position within manual monster spawn ranges
            float randomX = Random.Range(monsterSpawnMin.x, monsterSpawnMax.x);
            float randomY = Random.Range(monsterSpawnMin.y, monsterSpawnMax.y);
            spawnPosition = new Vector3(randomX, randomY, 0f) + parentOffset;

            // Check if the spawn position is safe (not colliding with tilemap colliders)
            positionSafe = !CheckCollision(spawnPosition);

            safetyAttempts--;

            if (safetyAttempts <= 0)
            {
                Debug.LogWarning(
                    "Failed to find safe monster spawn position after several attempts."
                );
                break;
            }
        } while (!positionSafe);

        // Set monster position
        monsterAI.transform.position = spawnPosition;
    }

    private void RespawnPlayer()
    {
        Vector3 spawnPosition;
        bool positionSafe = false;
        int safetyAttempts = 10; // Limit attempts to avoid infinite loops

        do
        {
            // Get random spawn position within manual player spawn ranges
            float randomX = Random.Range(playerSpawnMin.x, playerSpawnMax.x);
            float randomY = Random.Range(playerSpawnMin.y, playerSpawnMax.y);
            spawnPosition = new Vector3(randomX, randomY, 0f) + parentOffset;

            // Check if the spawn position is safe (not colliding with tilemap colliders)
            positionSafe = !CheckCollision(spawnPosition);

            safetyAttempts--;

            if (safetyAttempts <= 0)
            {
                Debug.LogWarning(
                    "Failed to find safe player spawn position after several attempts."
                );
                break;
            }
        } while (!positionSafe);

        // Set player position
        playerAI.transform.position = spawnPosition;
    }

    private void RespawnMonster()
    {
        Vector3 spawnPosition;
        bool positionSafe = false;
        int safetyAttempts = 10; // Limit attempts to avoid infinite loops

        do
        {
            // Get random spawn position within manual monster spawn ranges
            float randomX = Random.Range(monsterSpawnMin.x, monsterSpawnMax.x);
            float randomY = Random.Range(monsterSpawnMin.y, monsterSpawnMax.y);
            spawnPosition = new Vector3(randomX, randomY, 0f) + parentOffset;

            // Check if the spawn position is safe (not colliding with tilemap colliders)
            positionSafe = !CheckCollision(spawnPosition);

            safetyAttempts--;

            if (safetyAttempts <= 0)
            {
                Debug.LogWarning(
                    "Failed to find safe monster spawn position after several attempts."
                );
                break;
            }
        } while (!positionSafe);

        // Set monster position
        monsterAI.transform.position = spawnPosition;
    }

    private bool CheckCollision(Vector3 position)
    {
        foreach (var collider in tilemapColliders)
        {
            if (collider.OverlapPoint(position))
            {
                return true; // Collision found
            }
        }
        return false; // No collision found
    }
}
