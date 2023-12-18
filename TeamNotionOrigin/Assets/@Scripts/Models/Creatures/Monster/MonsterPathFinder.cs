using UnityEngine;
using UnityEngine.AI;

public class MonsterPathFinder : MonoBehaviour
{
    private Vector2 _target;
    private NavMeshAgent _agent;
    private Status status;

    public bool IsStopped => _agent.isStopped;
    public float RemainingDistance => _agent.remainingDistance;

    private void Start()
    {
        _agent = gameObject.GetOrAddComponent<NavMeshAgent>();
        status = GetComponent<Monster>().Status;
        SetInfo(status);
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

    public void SetInfo(Status data)
    {
        _agent.speed = data[StatType.Speed].Value;
        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        _agent.angularSpeed = 360f;
        _agent.acceleration = float.MaxValue;
    }
}