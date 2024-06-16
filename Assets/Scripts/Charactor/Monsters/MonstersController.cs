/*using System;
using System.Collections.Generic;
using Tqa.DungeonQuest.ObjectPooling;
using Unity.VisualScripting;
using UnityEngine;

public class MonstersController : BaseCombatCharactorController, IPoolObject
{
    public event Action OnDoneSetup;
    public event Action<Fighter> StageSupportDeathEvent;

    [field: SerializeField]
    public string MonsterName { get; private set; }
    public int Level { get; private set; }
    public QLearner qLearner;

    private FollowMonsterInfo _monsterInfo;

    private void Start()
    {
        // Initialize QLearner with appropriate parameters
        qLearner = new QLearner(
            learningRate: 0.1f,
            discountFactor: 0.9f,
            explorationRate: 1.0f,
            explorationDecay: 0.99f
        );

        // Subscribe to necessary events
        OnDoneSetup += StartQLearning;
        StageSupportDeathEvent += StopQLearning;
    }

    protected override void OnDestroy()
    {
        // Unsubscribe from events
        OnDoneSetup -= StartQLearning;
        StageSupportDeathEvent -= StopQLearning;
    }

    private void StartQLearning()
    {
        // Start Q-learning process, e.g., decision-making in Update method
        // Example: Invoke QLearner logic in Update or FixedUpdate
        InvokeRepeating(nameof(ProcessQLearning), 1f, 1f); // Invoke every 1 second
    }

    private void StopQLearning(Fighter fighter)
    {
        // Stop Q-learning process when the monster dies
        CancelInvoke(nameof(ProcessQLearning));
    }

    private void ProcessQLearning()
    {
        // Example: Fetch necessary information (player position, obstacles)
        Vector3 monsterPosition = transform.position;
        // Vector3 playerPosition = // Get player position

        // Construct state representation
        string currentState = qLearner.GetState(monsterPosition, playerPosition);

        // Determine action using QLearner
        int action = qLearner.GetNextAction(currentState);

        // Perform action based on the decision (e.g., move the monster)
        Vector3 nextMonsterPosition = PerformAction(action);

        // Update Q-table based on action and resulting state
        float reward = CalculateReward(monsterPosition, nextMonsterPosition, playerPosition);
        string nextState = qLearner.GetState(nextMonsterPosition, playerPosition);
        qLearner.UpdateQTable(currentState, action, reward, nextState);

        // Decay exploration rate over time
        qLearner.DecayExplorationRate();
    }

    private Vector3 PerformAction(int action)
    {
        // Example: Move the monster based on the action
        Vector3 newPosition = transform.position;

        switch (action)
        {
            case 0: // Move Up
                newPosition += Vector3.up;
                break;
            case 1: // Move Down
                newPosition += Vector3.down;
                break;
            case 2: // Move Left
                newPosition += Vector3.left;
                break;
            case 3: // Move Right
                newPosition += Vector3.right;
                break;
            case 4: // Attack (if applicable)
                // Implement attack logic here
                break;
            case 5: // Wait
                // Implement waiting logic here
                break;
            default:
                break;
        }

        // Example: Move the monster to the new position
        transform.position = newPosition;

        return newPosition;
    }

    private float CalculateReward(
        Vector3 currentPosition,
        Vector3 nextPosition,
        Vector3 playerPosition
    )
    {
        // Example: Calculate reward based on distance to player
        float currentDistance = Vector3.Distance(currentPosition, playerPosition);
        float nextDistance = Vector3.Distance(nextPosition, playerPosition);

        // Reward for moving closer to the player
        float reward = (currentDistance - nextDistance) * 0.1f;

        // Add other reward rules based on your game logic (avoiding obstacles, survival, etc.)

        return reward;
    }

    private void OnDisable()
    {
        if (_monsterInfo != null)
        {
            _monsterInfo.ReturnToPool();
        }
    }

    public void Initialize(BaseStatData statData, int level = 1)
    {
        Combat.RemoveAllEffect();
        Combat.Stats.ClearAllBonus();
        Combat.InstanciateFromStatsData(statData);
        SetLevel(statData, level);
        Combat.Health.Fill();
        Combat.Mana.Fill();
        Movement.ClearState();
        Animator.ClearState();
        //  Movement.MoveDirect = MovementInput.InputVector;
        _monsterInfo = MonsterWorldSpaceUIManager.Instance.GetMonsterInfo(this);
        OnDoneSetup?.Invoke();
    }

    private void SetLevel(BaseStatData statData, int level)
    {
        Level = level;
        var statModifiers = statData.growStat.GetGrowingStat(level);
        foreach (var modifier in statModifiers)
        {
            Combat.Stats.ApplyModifier(modifier.Key, modifier.Value);
        }
    }

    protected override void OnDead(Fighter fighter)
    {
        base.OnDead(fighter);
        StageSupportDeathEvent?.Invoke(fighter);
        StageSupportDeathEvent = null;
    }

    #region Pooling
    private Action<IPoolObject> _returnAction;

    public void Init(Action<IPoolObject> returnAction)
    {
        _returnAction = returnAction;
    }

    public void ReturnToPool()
    {
        if (_returnAction != null)
        {
            _returnAction.Invoke(this);
            _returnAction = null;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
*/
