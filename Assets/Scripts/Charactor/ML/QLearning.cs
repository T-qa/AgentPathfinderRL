using System;
using System.Collections.Generic;
using UnityEngine;

public class QLearning
{
    private Dictionary<string, float[]> qTable;
    private float learningRate;
    private float discountFactor;
    private float explorationRate;
    private float explorationDecay;
    private float previousDistance;

    public QLearning(
        float learningRate,
        float discountFactor,
        float explorationRate,
        float explorationDecay
    )
    {
        qTable = new Dictionary<string, float[]>();
        this.learningRate = learningRate;
        this.discountFactor = discountFactor;
        this.explorationRate = explorationRate;
        this.explorationDecay = explorationDecay;
    }

    public string GetState(Vector2 monsterPosition, Vector2 playerPosition, Vector2[] obstacles)
    {
        // Simplified example of state representation
        string state =
            $"{monsterPosition.x},{monsterPosition.y},{playerPosition.x},{playerPosition.y}";
        foreach (var obstacle in obstacles)
        {
            state += $",{obstacle.x},{obstacle.y}";
        }
        return state;
    }

    public int GetNextAction(string state)
    {
        if (UnityEngine.Random.value < explorationRate)
        {
            // Explore: choose a random action
            return UnityEngine.Random.Range(0, 5); // 4 move directions + 1 attack
        }
        else
        {
            // Exploit: choose the best action from Q-table
            if (!qTable.ContainsKey(state))
            {
                qTable[state] = new float[5]; // Initialize if state not present
            }
            float[] qValues = qTable[state];
            int bestAction = Array.IndexOf(qValues, Mathf.Max(qValues));
            return bestAction;
        }
    }

    public void UpdateQTable(string state, int action, float reward, string nextState)
    {
        if (!qTable.ContainsKey(state))
        {
            qTable[state] = new float[5];
        }
        if (!qTable.ContainsKey(nextState))
        {
            qTable[nextState] = new float[5];
        }

        float[] qValues = qTable[state];
        float[] nextQValues = qTable[nextState];
        float bestNextQValue = Mathf.Max(nextQValues);

        qValues[action] +=
            learningRate * (reward + discountFactor * bestNextQValue - qValues[action]);
    }

    public void DecayExplorationRate()
    {
        // Decrease exploration rate over time
        explorationRate *= explorationDecay;
    }

    public float CalculateReward(
        Vector2 monsterPosition,
        Vector2 playerPosition,
        bool attacked,
        bool hitByPlayer,
        bool inTrap,
        bool avoidedTrap
    )
    {
        float reward = 0.0f;

        if (Vector2.Distance(monsterPosition, playerPosition) < previousDistance)
        {
            reward += 1.0f; // Moving closer to the player
        }
        if (attacked)
        {
            reward += 10.0f; // Successful attack
        }
        if (hitByPlayer)
        {
            reward -= 5.0f; // Hit by the player
        }
        if (inTrap)
        {
            reward -= 10.0f; // Monster in a trap
        }
        if (avoidedTrap)
        {
            reward += 5.0f; // Avoided a trap
        }

        previousDistance = Vector2.Distance(monsterPosition, playerPosition);

        return reward;
    }
}
