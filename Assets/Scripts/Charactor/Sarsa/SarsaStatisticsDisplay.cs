using TMPro;
using UnityEngine;

public class SarsaStatisticsDisplay : MonoBehaviour
{
    public TextMeshProUGUI totalStepsText;
    public TextMeshProUGUI totalEpisodesText;
    public TextMeshProUGUI convergenceQValueText;
    public TextMeshProUGUI averageStepsPerEpisodeText;
    public TextMeshProUGUI averageRewardPerEpisodeText;
    public TextMeshProUGUI highestRewardText;
    public TextMeshProUGUI averageStepsToHighestRewardText;

    public void UpdateStatistics(SarsaLearner sarsaLearner)
    {
        int totalSteps = sarsaLearner.TotalSteps;
        int totalEpisodes = sarsaLearner.TotalEpisodes;
        float convergenceQValue = sarsaLearner.ConvergenceQValue;
        float totalReward = sarsaLearner.TotalReward;
        float highestReward = sarsaLearner.HighestReward;
        int successCount = sarsaLearner.SuccessCount;
        int highestRewardSteps = sarsaLearner.HighestRewardSteps;

        float averageStepsPerEpisode = (totalEpisodes > 0) ? (float)totalSteps / totalEpisodes : 0;
        float averageRewardPerEpisode = (totalEpisodes > 0) ? totalReward / totalEpisodes : 0;
        float successRate = (totalEpisodes > 0) ? (float)successCount / totalEpisodes : 0;
        float averageStepsToHighestReward =
            (successCount > 0) ? (float)highestRewardSteps / successCount : 0;

        float explorationRate = sarsaLearner.ExplorationRate;

        totalStepsText.text = "Total Steps: " + totalSteps;
        totalEpisodesText.text = "Total Episodes: " + totalEpisodes;
        convergenceQValueText.text = "Convergence of Q-Value: " + convergenceQValue.ToString("F2");
        averageStepsPerEpisodeText.text = "Average Steps per Episode: " + averageStepsPerEpisode;
        averageRewardPerEpisodeText.text = "Average Reward per Episode: " + averageRewardPerEpisode;
        highestRewardText.text = "Highest Reward: " + highestReward;
        averageStepsToHighestRewardText.text =
            "Avg Steps to Highest Reward: " + averageStepsToHighestReward;
    }
}
