using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsDisplay : MonoBehaviour
{
    private QLearner _qLearner;

    [Header("Settings")]
    public int SnapShotSize = 10;

    [Header("Output")]
    [SerializeField]
    private int stepCount;

    [SerializeField]
    private int episodeCount;

    [SerializeField]
    private float avgStepsPerEpisode;

    [SerializeField]
    private float avgStepsPerEpisodeSnapshot;

    [SerializeField]
    private float avgRewardPerEpisode;

    [SerializeField]
    private float successRate;

    private int stepCountLastEpisode;
    private List<float> episodeStepsHistory = new List<float>();
    private List<float> episodeRewardsHistory = new List<float>();
    private List<float> qValueMaxDifferences = new List<float>();

    void Start()
    {
        _qLearner = FindObjectOfType<MLMonsterAI>().qLearner; // Assuming QLearner is attached to MLMonsterAI
        _qLearner.OnStep += Step;
        _qLearner.OnEpisodeComplete += EpisodeComplete;
    }

    public void Step()
    {
        stepCount++;
    }

    public void EpisodeComplete()
    {
        episodeCount++;

        // Calculate average steps per episode
        int episodeSteps = stepCount - stepCountLastEpisode;
        episodeStepsHistory.Add(episodeSteps);
        avgStepsPerEpisode = CalculateAverage(episodeStepsHistory);

        // Calculate average steps per episode snapshot
        avgStepsPerEpisodeSnapshot = CalculateAverageSnapshot(episodeStepsHistory, SnapShotSize);

        // Calculate average reward per episode
        float episodeReward = CalculateEpisodeReward();
        episodeRewardsHistory.Add(episodeReward);
        avgRewardPerEpisode = CalculateAverage(episodeRewardsHistory);

        // Calculate success rate
        successRate = CalculateSuccessRate();

        // Reset step count for next episode
        stepCountLastEpisode = stepCount;
    }

    private float CalculateAverage(List<float> values)
    {
        if (values.Count == 0)
            return 0f;

        float sum = 0f;
        foreach (var value in values)
        {
            sum += value;
        }
        return sum / values.Count;
    }

    private float CalculateAverageSnapshot(List<float> history, int size)
    {
        if (history.Count == 0)
            return 0f;

        int startIndex = Mathf.Max(0, history.Count - size);
        float sum = 0f;
        for (int i = startIndex; i < history.Count; i++)
        {
            sum += history[i];
        }
        return sum / Mathf.Min(size, history.Count);
    }

    private float CalculateEpisodeReward()
    {
        if (episodeRewardsHistory.Count == 0)
            return 0f;

        float sum = 0f;
        foreach (var reward in episodeRewardsHistory)
        {
            sum += reward;
        }
        return sum / episodeRewardsHistory.Count;
    }

    private float CalculateSuccessRate()
    {
        // For simplicity, assume success rate based on average reward
        return avgRewardPerEpisode > 0f ? 1f : 0f;
    }
}
