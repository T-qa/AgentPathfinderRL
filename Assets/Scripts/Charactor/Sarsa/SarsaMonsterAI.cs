using System.Collections;
using UnityEngine;

public class SarsaMonsterAI : MonoBehaviour
{
    [Header("Monster AI Settings")]
    public float updateInterval = 0.5f; // Reduced interval for faster updates

    [Range(0.1f, 10f)]
    public float gameSpeed = 1.0f;

    [Header("Hyperparameters")]
    [Range(0.05f, 1f)]
    public float learningRate = 1f;
    public float discountFactor = 0.9f;
    public float explorationRate = 1.0f;
    public float explorationDecay = 0.99f;

    public SarsaLearner sarsaLearner;
    private Vector3 startPosition;
    private GameObject player;

    private Vector3 lastMonsterPosition;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        Time.timeScale = gameSpeed; // Set initial game speed
        sarsaLearner = new SarsaLearner(
            learningRate,
            discountFactor,
            explorationRate,
            explorationDecay
        );
        startPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError(
                "Player GameObject not found. Make sure the player GameObject has the tag 'Player'."
            );
            return;
        }

        StartCoroutine(AICoroutine());
    }

    void LateUpdate()
    {
        Time.timeScale = gameSpeed; // Dynamically adjust game speed
    }

    private IEnumerator AICoroutine()
    {
        int episodeCount = 0;
        while (true)
        {
            for (int i = 0; i < 10; i++) // Perform 10 updates per frame
            {
                string state = GetCurrentState();
                int action = sarsaLearner.GetNextAction(state);
                PerformAction(action);
                episodeCount++;
                yield return new WaitForSeconds(updateInterval);

                string nextState = GetCurrentState();
                float reward = CalculateReward();
                int nextAction = sarsaLearner.GetNextAction(nextState);

                sarsaLearner.UpdateQTable(state, action, reward, nextState, nextAction);
                sarsaLearner.DecayExplorationRate();

                lastMonsterPosition = transform.position;
                lastPlayerPosition = player.transform.position;
            }
            yield return null;
        }
    }

    private string GetCurrentState()
    {
        Vector3 monsterPosition = transform.position;
        Vector3 playerPosition = player.transform.position;
        return sarsaLearner.GetState(monsterPosition, playerPosition);
    }

    private void PerformAction(int action)
    {
        Vector3 moveDirection = Vector3.zero;

        switch (action)
        {
            case 0:
                moveDirection = Vector3.up;
                break; // Move up
            case 1:
                moveDirection = Vector3.down;
                break; // Move down
            case 2:
                moveDirection = Vector3.left;
                break; // Move left
            case 3:
                moveDirection = Vector3.right;
                break; // Move right
            case 4:
                moveDirection = Vector3.zero;
                break; // Wait
        }

        MoveTo(transform.position + moveDirection);
    }

    public void MoveTo(Vector3 destination)
    {
        StartCoroutine(MoveOverTime(destination, 1f));
    }

    private IEnumerator MoveOverTime(Vector3 destination, float speed)
    {
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                Time.deltaTime * speed
            );
            yield return null;
        }
    }

    private float CalculateReward()
    {
        float previousDistance = Vector3.Distance(lastMonsterPosition, lastPlayerPosition);
        float currentDistance = Vector3.Distance(transform.position, player.transform.position);

        float reward = 0;

        if (currentDistance < previousDistance)
        {
            reward += 3; // Reward for moving closer
        }
        else if (currentDistance > previousDistance)
        {
            reward -= 5; // Penalty for moving away
        }

        if (transform.position == lastMonsterPosition)
        {
            reward -= 0.5f; // Penalty for waiting
        }

        return reward;
    }
}
