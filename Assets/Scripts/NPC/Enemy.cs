using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace MomoCoop.NPC
{
    public class Enemy : NetworkBehaviour
    {
        [SerializeField]
        private NavMeshAgent _agent;

        private Transform _target;
        private Transform _thisTransform;
        private Waypoint[] _waypoints;

        public void Initialize()
        {
            _thisTransform = transform;
            _waypoints = FindObjectsOfType<Waypoint>();
            SetTarget(GetRandomTarget());
            _thisTransform.position = _target.position;
            StartCoroutine(WaitUntilWaypointReached());
        }

        private Transform GetRandomTarget() => _waypoints[Random.Range(0, _waypoints.Length)].transform;

        private void SetTarget(Transform target)
        {
            _target = target;
            _agent.destination = _target.position;
        }

        private IEnumerator WaitUntilWaypointReached()
        {
            while (true)
            {
                yield return new WaitUntil(() => (_thisTransform.position - _target.position).magnitude <= _agent.stoppingDistance * 1.1f);
                SetTarget(GetRandomTarget());
            }
        }
    }
}