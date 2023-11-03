using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    private NavMeshAgent agent;

    public Transform target; // Set this in the Inspector to specify the destination.

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}