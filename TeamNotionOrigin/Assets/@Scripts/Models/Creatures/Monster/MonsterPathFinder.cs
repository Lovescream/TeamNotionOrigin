using UnityEngine;
using UnityEngine.AI;

public class MonsterPathFinder : MonoBehaviour
{
    private Vector2 _target;
    private NavMeshAgent _agent;
    private Monster parent;

    public bool IsStopped => _agent.isStopped;
    public float RemainingDistance => _agent.remainingDistance;
    public NavMeshAgent Agent => _agent;

    private void Start()
    {
        _agent = gameObject.GetOrAddComponent<NavMeshAgent>();
        parent = GetComponent<Monster>();
        SetInfo();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    public void SetDestination(Transform target)
    {
        _target = target.position;
        _agent.SetDestination(_target);
    }

    public void SetDestination(Vector2 target)
    {
        _target = target;
        _agent.SetDestination(_target);
    }

    public void ResetPath()
    {
        _agent.ResetPath();
    }

    public void SetInfo()
    {
        // TODO: status 받아와서 세팅..
        _agent.speed = parent.Status[StatType.Speed].Value;
        //_agent.speed = 5f;
        _agent.stoppingDistance = parent.AttackRange; // 얘는 AttackRange로 세팅하면 될듯

        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        _agent.angularSpeed = 360f;
        _agent.acceleration = float.MaxValue;
    }
}