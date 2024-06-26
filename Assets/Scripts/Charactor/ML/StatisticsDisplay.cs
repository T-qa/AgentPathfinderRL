using TMPro;
using UnityEngine;

public class StatisticsDisplay : MonoBehaviour
{
    public TextMeshProUGUI totalStepsText;
    public TextMeshProUGUI totalEpisodesText;
    public TextMeshProUGUI convergenceQValueText;
    public TextMeshProUGUI averageStepsPerEpisodeText;
    public TextMeshProUGUI averageRewardPerEpisodeText;
    public TextMeshProUGUI highestRewardText;
    public TextMeshProUGUI averageStepsToHighestRewardText;

    public void UpdateStatistics(QLearner qLearner)
    {
        int totalSteps = qLearner.TotalSteps;
        int totalEpisodes = qLearner.TotalEpisodes;
        float convergenceQValue = qLearner.ConvergenceQValue;
        float totalReward = qLearner.TotalReward;
        float highestReward = qLearner.HighestReward;
        int successCount = qLearner.SuccessCount;
        int highestRewardSteps = qLearner.HighestRewardSteps;

        float averageStepsPerEpisode = (totalEpisodes > 0) ? (float)totalSteps / totalEpisodes : 0;
        float averageRewardPerEpisode = (totalEpisodes > 0) ? totalReward / totalEpisodes : 0;
        float successRate = (totalEpisodes > 0) ? (float)successCount / totalEpisodes : 0;
        float averageStepsToHighestReward =
            (successCount > 0) ? (float)highestRewardSteps / successCount : 0;

        totalStepsText.text = "Total Steps: " + totalSteps;
        totalEpisodesText.text = "Total Episodes: " + totalEpisodes;
        convergenceQValueText.text = "Convergence of Q-Value: " + convergenceQValue;
        averageStepsPerEpisodeText.text = "Average Steps per Episode: " + averageStepsPerEpisode;
        averageRewardPerEpisodeText.text = "Average Reward per Episode: " + averageRewardPerEpisode;
        highestRewardText.text = "Highest Reward: " + highestReward;
        averageStepsToHighestRewardText.text =
            "Avg Steps to Highest Reward: " + averageStepsToHighestReward;
    }
}
