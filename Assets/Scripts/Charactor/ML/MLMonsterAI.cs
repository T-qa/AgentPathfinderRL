using System.Collections;
using UnityEngine;

public class MLMonsterAI : MonoBehaviour
{
    [Header("Monster AI Settings")]
    public float moveRange = 5f;
    public float detectRange = 10f;
    public float updateInterval = 0.5f;

    [Header("Hyperparameters")]
    [Range(0.05f, 100f)]
    public float learningRate = 1f;
    public float discountFactor = 0.9f;
    public float explorationRate = 1.0f;
    public float explorationDecay = 0.99f;

    public QLearner qLearner;
    private Vector3 startPosition;
    private GameObject player;

    private Vector3 lastMonsterPosition;
    private Vector3 lastPlayerPosition;

    private StatisticsDisplay statsDisplay;

    void Start()
    {
        qLearner = new QLearner(learningRate, discountFactor, explorationRate, explorationDecay);
        startPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        statsDisplay = FindObjectOfType<StatisticsDisplay>(); // Assuming it's in the scene

        if (player == null)
        {
            Debug.LogError(
                "Player GameObject not found. Make sure the player GameObject has the tag 'Player'."
            );
            return;
        }

        StartCoroutine(AICoroutine());
    }

    private IEnumerator AICoroutine()
    {
        while (true)
        {
            string state = GetCurrentState();
            int action = qLearner.GetNextAction(state);
            Debug.Log($"Current State: {state}, Selected Action: {action}");
            PerformAction(action);
            yield return new WaitForSeconds(updateInterval);

            string nextState = GetCurrentState();
            float reward = CalculateReward();
            qLearner.UpdateQTable(state, action, reward, nextState);
            qLearner.DecayExplorationRate();

            lastMonsterPosition = transform.position;
            lastPlayerPosition = player.transform.position;
        }
    }

    private string GetCurrentState()
    {
        Vector3 monsterPosition = transform.position;
        Vector3 playerPosition = player.transform.position;
        return qLearner.GetState(monsterPosition, playerPosition);
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

    private void MoveTo(Vector3 destination)
    {
        StartCoroutine(MoveOverTime(destination));
    }

    private IEnumerator MoveOverTime(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                destination,
                Time.deltaTime * 2
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
            reward += 1; // Reward for moving closer
        }
        else if (currentDistance > previousDistance)
        {
            reward -= 1; // Penalty for moving away
        }

        if (transform.position == lastMonsterPosition)
        {
            reward -= 0.1f; // Penalty for waiting
        }

        return reward;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(startPosition, detectRange);
    }
}
