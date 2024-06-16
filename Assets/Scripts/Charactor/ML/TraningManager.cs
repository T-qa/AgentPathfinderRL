using System.Collections;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    public int numEpisodes = 1000;
    public GameObject playerPrefab;
    public GameObject monsterPrefab;
    public Transform spawnPoint;

    private void Start()
    {
        StartCoroutine(TrainingCoroutine());
    }

    private IEnumerator TrainingCoroutine()
    {
        for (int i = 0; i < numEpisodes; i++)
        {
            // Reset environment
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
            GameObject monster = Instantiate(
                monsterPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

            MonstersAI monsterAI = monster.GetComponent<MonstersAI>();
            yield return new WaitUntil(() => !monsterAI.IsAlive());

            Destroy(player);
            Destroy(monster);

            yield return new WaitForSeconds(1.0f); // Small delay between episodes
        }
    }
}
