using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QLearner
{
    public event Action OnStep;
    public event Action OnEpisodeComplete;

    public Dictionary<string, float[]> qTable;
    private float learningRate;
    private float discountFactor;
    private float explorationRate;
    private float explorationDecay;

    public QLearner(
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

    public string GetState(Vector3 monsterPosition, Vector3 playerPosition)
    {
        string state =
            $"{Mathf.RoundToInt(monsterPosition.x)},{Mathf.RoundToInt(monsterPosition.y)},{Mathf.RoundToInt(playerPosition.x)},{Mathf.RoundToInt(playerPosition.y)}";
        return state;
    }

    public int GetNextAction(string state)
    {
        if (UnityEngine.Random.value < explorationRate)
        {
            // Explore: choose a random action
            return UnityEngine.Random.Range(0, 5); // 5 actions: 4 moves + wait
        }
        else
        {
            // Exploit: choose the best action from Q-table
            if (!qTable.ContainsKey(state))
            {
                qTable[state] = new float[5]; // Initialize if state not present
            }
            float[] qValues = qTable[state];
            int bestAction = System.Array.IndexOf(qValues, Mathf.Max(qValues));
            return bestAction;
        }
    }

    public void UpdateQTable(string state, int action, float reward, string nextState)
    {
        if (!qTable.ContainsKey(state))
        {
            qTable[state] = new float[5]; // Initialize Q-values for actions
        }
        if (!qTable.ContainsKey(nextState))
        {
            qTable[nextState] = new float[5]; // Initialize Q-values for actions
        }

        float[] qValues = qTable[state];
        float[] nextQValues = qTable[nextState];
        float bestNextQValue = Mathf.Max(nextQValues);

        // Update Q-value using the Q-learning formula
        qValues[action] +=
            learningRate * (reward + discountFactor * bestNextQValue - qValues[action]);

        OnStep?.Invoke(); // Notify step completion
    }

    public void DecayExplorationRate()
    {
        // Decrease exploration rate over time
        explorationRate *= explorationDecay;
    }

    public void NotifyEpisodeComplete()
    {
        OnEpisodeComplete?.Invoke(); // Notify episode completion
    }
}
