using UnityEngine;
using UnityEngine.AI;

public class MonsterPathFinder : MonoBehaviour
{
    private Vector2 _target;
    private NavMeshAgent _agent;
    private Monster _parent;

    public bool IsStopped => _agent.isStopped;
    public float RemainingDistance => _agent.remainingDistance;
    public NavMeshAgent Agent => _agent;

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

    public void SetInfo(Monster parent)
    {
        // TODO: status 받아와서 세팅..
        _parent = parent;
        _agent = gameObject.GetOrAddComponent<NavMeshAgent>();
        Debug.Log(_agent);
        Debug.Log(_parent);
        Debug.Log(_parent.Status);
        _agent.speed = _parent.Status[StatType.Speed].Value;
        //_agent.speed = 5f;
        _agent.stoppingDistance = _parent.AttackRange; // 얘는 AttackRange로 세팅하면 될듯
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        _agent.angularSpeed = 360f;
        _agent.acceleration = float.MaxValue;
    }
}