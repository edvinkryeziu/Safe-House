using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Idle, Patrol, Chase, Flee }
    private EnemyState _currentState;
    public float enemyChaseSpeed;
    public float normalSpeed = 3.5f;

    public float detectionRange;
    public float fleeRange;
    public float fleeDistance;
    public float fleeAngularSpeed;
    public float fleeAcceleration;
    public float loseRange;
    public Transform[] patrolWaypoints;
    public float fieldOfViewAngle;
    public float enemySightDistance;
    public LayerMask playerLayer;
    private Transform _player;
    private NavMeshAgent _agent;
    private bool hasPatrolTarget;
    private RaycastHit rayHit;
    private bool isFleeing;
    private float startAcceleration;
    private float startAngularSpeed;

    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        startAcceleration = _agent.acceleration;
        startAngularSpeed = _agent.angularSpeed;

    }

    void Update()
    {
        
        float distanceToPlayer = Vector3.Distance(transform.position,_player.position);
        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, directionToPlayer);

        switch (_currentState)
        {
            case EnemyState.Idle:
                // Idle behaviour
                break;
            case EnemyState.Patrol:
                Patrol(patrolWaypoints);
                break;
            case EnemyState.Chase:
                Chase(_player);
                break;
            case EnemyState.Flee:
                Flee(_player);
                break;
        }

        if (distanceToPlayer <= fleeRange && isFleeing)
        {
            _currentState = EnemyState.Flee;
            return;
        }

        // Enemy field of view check
        if (dot > Mathf.Cos(fieldOfViewAngle * 0.5f * Mathf.Deg2Rad) && distanceToPlayer <= detectionRange)
        {
            if (Physics.Raycast(transform.position,directionToPlayer,out rayHit, enemySightDistance))
            {
                if (rayHit.collider.CompareTag("Player"))
                {
                    _currentState = EnemyState.Chase;
                }
                else
                {
                    _currentState = EnemyState.Patrol;
                }
            }
        }
        else if (distanceToPlayer >= detectionRange)
        {
            _currentState = EnemyState.Patrol;
        }
        else if (distanceToPlayer >= loseRange)
        {
            _currentState = EnemyState.Idle;
        }
    }

    private void Chase(Transform playerPos)
    {
        _agent.destination = playerPos.position;
        _agent.speed = enemyChaseSpeed;
        _agent.acceleration = startAcceleration;
        _agent.angularSpeed = startAngularSpeed;
    }

    private void Patrol(Transform[] patrolWaypoints)
    {
        _agent.angularSpeed = startAngularSpeed;
        _agent.acceleration = startAcceleration;
        _agent.speed = normalSpeed;
        if (hasPatrolTarget == false)
        {
            hasPatrolTarget = true;
            int patrolIndex = Random.Range(0,patrolWaypoints.Length);
            _agent.destination = patrolWaypoints[patrolIndex].position;
        }
        else if (hasPatrolTarget)
        {
            if (_agent.remainingDistance < 5 && !_agent.pathPending)
            {
                hasPatrolTarget = false;
            }
        }
        
    }

    private void Flee(Transform playerPos)
    {
        Vector3 fleeDirection = transform.position + (transform.position - playerPos.position).normalized * fleeDistance;
        _agent.baseOffset -= 0.1f;
        _agent.angularSpeed = fleeAngularSpeed;
        _agent.acceleration = fleeAcceleration;
        _agent.speed = enemyChaseSpeed;
        _agent.destination = fleeDirection;
        
    }

    private void OnFlareFlee(bool isFlareActive)
    {
        if (isFlareActive)
        {
            isFleeing = true;
        }

        if (!isFlareActive)
        {
            isFleeing = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfViewAngle * 0.5f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfViewAngle * 0.5f, 0) * transform.forward;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftBoundary * enemySightDistance);
        Gizmos.DrawRay(transform.position, rightBoundary * enemySightDistance);
        // Optional: draw the detection range circle
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    void OnEnable()
    {
        FlareItem.OnFlareActive += OnFlareFlee;
    }

    void OnDisable()
    {
        FlareItem.OnFlareActive -= OnFlareFlee;
    }
}
