using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using System;

namespace Prototype.Controllers
{
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(NavMeshModifier))]
    public sealed class PlayerController : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            InitializeComponents();
        }


        public void MoveToPosition(Vector3 targetPosition, Action onComplete)
        {
            _navMeshAgent.SetDestination(targetPosition);
            StartCoroutine(CheckArrival(onComplete));
        }


        private void InitializeComponents()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
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
}