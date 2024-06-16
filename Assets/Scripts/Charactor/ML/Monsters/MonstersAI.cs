using System.Collections;
using UnityEngine;

public class MonstersAI : BaseMovementInput
{
    [Header("Monster AI fields")]
    [SerializeField]
    private float moveRange;

    [SerializeField]
    private float directChangeInterval;

    [SerializeField]
    private float detectRange;

    [Min(0.1f)]
    [SerializeField]
    private float updateInterval;

    private BaseAICombatBehaviour _combatBehaviour;

    public Vector2 StartPosition { get; protected set; }

    [field: SerializeField]
    public MonstersController Controller { get; private set; }

    public Fighter Player { get; private set; }
    public Vector2 PlayerPosition => Player.HitBox.bounds.center;

    private bool _wasTakenHit;

    private QLearning qLearning;

    // Define the number of states based on your game's requirements
    private int stateCount = 100;

    // Move Up, Down, Left, Right, Attack
    private int actionCount = 5;

    private void Awake()
    {
        qLearning = new QLearning(0.1f, 0.9f, 0.1f, 0.99f);
        SetCombatBehaviour(GetComponent<BaseAICombatBehaviour>());
        Controller.OnDoneSetup += StartAICoroutine;
        Controller.Combat.OnTakeDamage += TakeHit;
    }

    private void OnDestroy()
    {
        if (Controller != null)
        {
            Controller.OnDoneSetup -= StartAICoroutine;
            if (Controller.Combat != null)
            {
                Controller.Combat.OnTakeDamage -= TakeHit;
            }
        }
    }

    public bool IsAlive() => !Controller.Combat.Health.IsEmpty;

    private void StartAICoroutine()
    {
        StartCoroutine(AICoroutine());
    }

    public void SetCombatBehaviour(BaseAICombatBehaviour combatBehaviour)
    {
        if (combatBehaviour == null)
        {
            throw new System.ArgumentNullException(nameof(combatBehaviour));
        }
        _combatBehaviour = combatBehaviour;
        _combatBehaviour.Prepare(this);
    }

    public void LookToward(Vector2 destination)
    {
        InputVector = (destination - (Vector2)transform.position).normalized;
    }

    private IEnumerator AICoroutine()
    {
        Player = PlayerController.Instance.Combat;
        StartPosition = transform.position;
        _wasTakenHit = false;
        yield return new WaitForSeconds(0.3f);
        while (IsAlive())
        {
            var distanceToPlayer = Vector2.Distance(transform.position, PlayerPosition);
            if (distanceToPlayer < detectRange || _wasTakenHit)
            {
                yield return _combatBehaviour.StartCombatState();
                _wasTakenHit = false;
                continue;
            }
            QLearningMove();
            yield return new WaitForSeconds(updateInterval);
        }
        StopMove();
        yield return 4f.Wait();
        Controller.ReturnToPool();
    }

    private void QLearningMove()
    {
        string currentState = qLearning.GetState(
            transform.position,
            PlayerPosition,
            new Vector2[0]
        );
        int action = qLearning.GetNextAction(currentState);
        ExecuteAction(action);
    }

    private void ExecuteAction(int action)
    {
        switch (action)
        {
            case 0:
                MoveTo(transform.position + Vector3.up);
                break;
            case 1:
                MoveTo(transform.position + Vector3.down);
                break;
            case 2:
                MoveTo(transform.position + Vector3.left);
                break;
            case 3:
                MoveTo(transform.position + Vector3.right);
                break;
            case 4:
                // Implement attack logic
                break;
        }
    }

    public void MoveTo(Vector2 destination)
    {
        InputVector = (destination - (Vector2)transform.position).normalized;
    }

    private void StopMove()
    {
        InputVector = Vector2.zero;
    }

    private void TakeHit(DamageBlock _)
    {
        _wasTakenHit = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        DrawGizmos();
    }

    private void DrawGizmos()
    {
        DrawCircle(detectRange, Color.yellow);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(StartPosition, moveRange);
    }

    private void DrawCircle(float radius, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
