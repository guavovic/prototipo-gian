using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(NavMeshModifier))]
public sealed class PlayerController : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void MoveToPosition(Vector3 targetPosition, Action onComplete)
    {
        _navMeshAgent.SetDestination(targetPosition);
        StartCoroutine(CheckArrival(onComplete));
    }

    private System.Collections.IEnumerator CheckArrival(Action onComplete)
    {
        while (_navMeshAgent.pathPending || _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            yield return null;
        }

        onComplete?.Invoke();
    }
}