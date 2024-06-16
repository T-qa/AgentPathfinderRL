/*using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StageTrigger : MonoBehaviour
{
    [SerializeField]
    private LevelManager levelManager;

    [SerializeField]
    private PolygonCollider2D boundary;

    [SerializeField]
    private MonsterStageSpawner[] stageSpawners;

    public UnityEvent OnStageTrigger;
    public UnityEvent OnStageEnded;
    public UnityEvent OnAllMonstersDefeated; // Event to trigger scene restart

    private bool _isTrigged = false;
    private bool _isActive = false;
    private int _workingSpawner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isTrigged && collision.CompareTag("Player"))
        {
            TriggerStageEvent();
        }
    }

    public void TriggerStageEvent()
    {
        _isTrigged = true;
        _isActive = true;
        _workingSpawner = stageSpawners.Length;
        levelManager.SetConfinerCollider(boundary);
        OnStageTrigger.Invoke();

        foreach (var stageSpawner in stageSpawners)
        {
            stageSpawner.StartSpawning(OnSpawnerEnded);
        }
    }

    private void OnSpawnerEnded()
    {
        _workingSpawner--;
        if (_workingSpawner > 0 || !_isActive)
            return;

        _isActive = false;
        levelManager.SetToDefaultLevelBound();
        OnStageEnded.Invoke();

        if (AreAllMonstersDefeated())
        {
            // Trigger scene restart
            OnAllMonstersDefeated.Invoke();
            RespawnMonsters();
        }
    }

    private bool AreAllMonstersDefeated()
    {
        _isActive = true;
        _workingSpawner = stageSpawners.Length;
        foreach (var stageSpawner in stageSpawners)
        {
            if (stageSpawner.HasActiveMonsters())
                return false;
        }
        return true;
    }

    private void RespawnMonsters()
    {
        foreach (var stageSpawner in stageSpawners)
        {
            stageSpawner.ResetSpawner();
            stageSpawner.StartSpawning(OnSpawnerEnded);
        }
    }
}
*/