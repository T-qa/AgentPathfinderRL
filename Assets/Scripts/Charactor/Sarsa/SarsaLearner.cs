using System;
using System.Collections.Generic;
using UnityEngine;

public class SarsaLearner
{
    public event Action OnStep;
    public Dictionary<string, float[]> qTable;
    private float learningRate;
    private float discountFactor;
    private float explorationRate;
    private float explorationDecay;
    private float minExplorationRate = 0.1f;

    // Tracking variables
    public int TotalSteps { get; private set; }
    public int TotalEpisodes { get; private set; }
    public float TotalReward { get; private set; }
    public float HighestReward { get; private set; } = float.MinValue;
    public int HighestRewardSteps { get; private set; }
    public int SuccessCount { get; private set; }
    public float ConvergenceQValue { get; private set; }
    public float ExplorationRate => explorationRate; // Expose exploration rate publicly

    private float episodeReward;
    private int episodeSteps;

    public SarsaLearner(
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
        int monsterX = Mathf.RoundToInt(monsterPosition.x);
        int monsterY = Mathf.RoundToInt(monsterPosition.y);
        int playerX = Mathf.RoundToInt(playerPosition.x);
        int playerY = Mathf.RoundToInt(playerPosition.y);
        string state = $"{monsterX},{monsterY},{playerX},{playerY}";
        return state;
    }

    public int GetNextAction(string state)
    {
        if (UnityEngine.Random.value < explorationRate)
        {
            // Explore: choose a random action
            return UnityEngine.Random.Range(0, 5);
        }
        else
        {
            // Exploit: choose the best action from Q-table
            if (!qTable.ContainsKey(state))
            {
                qTable[state] = new float[5];
            }
            float[] qValues = qTable[state];
            int bestAction = Array.IndexOf(qValues, Mathf.Max(qValues));
            return bestAction;
        }
    }

    public void UpdateQTable(
        string state,
        int action,
        float reward,
        string nextState,
        int nextAction
    )
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
        float nextQValue = nextQValues[nextAction];

        // Update Q-value using the SARSA formula
        qValues[action] += learningRate * (reward + discountFactor * nextQValue - qValues[action]);

        // Update statistics
        episodeReward += reward;
        episodeSteps++;
        TotalSteps++;

        OnStep?.Invoke();
    }

    public void DecayExplorationRate()
    {
        explorationRate = Mathf.Max(explorationRate * explorationDecay, minExplorationRate); // Apply exponential decay with a floor
    }

    public void EndEpisode()
    {
        TotalEpisodes++;
        TotalReward += episodeReward;
        ConvergenceQValue = CalculateConvergence();
        if (episodeReward > HighestReward)
        {
            HighestReward = episodeReward;
            HighestRewardSteps = episodeSteps;
            SuccessCount++;
        }
        episodeReward = 0;
        episodeSteps = 0;
    }

    private float CalculateConvergence()
    {
        float sumQValues = 0;
        int countQValues = 0;

        foreach (var qValues in qTable.Values)
        {
            foreach (var qValue in qValues)
            {
                sumQValues += Mathf.Abs(qValue);
                countQValues++;
            }
        }

        return countQValues > 0 ? sumQValues / countQValues : 0;
    }
}
