using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEscape : MonoBehaviour
{
	public Transform player; 
	public float detectionRadius = 20f; 
	public LayerMask obstacleLayerMask; 

	private NavMeshAgent navMeshAgent;
	private bool isEscaping = false;

	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		SetRandomDestination(); 
	}

	private void Update()
	{
		if (CanSeePlayer())
		{
			EscapeToFurthestPoint();
			isEscaping = true;
		}
		else
		{
			if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 0.5f)
			{
				SetRandomDestination();
				isEscaping = false;
			}
		}
	}

	private bool CanSeePlayer()
	{
		float distanceToPlayer = Vector3.Distance(transform.position, player.position);

		if (distanceToPlayer <= detectionRadius)
		{
			Vector3 directionToPlayer = (player.position - transform.position).normalized;
			RaycastHit hit;

			if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius, ~obstacleLayerMask))
			{
				return hit.transform == player;
			}
		}
		return false;
	}

	private void EscapeToFurthestPoint()
	{
		NavMeshHit furthestPoint;
		if (NavMesh.SamplePosition(-player.position * 100, out furthestPoint, Mathf.Infinity, NavMesh.AllAreas))
		{
			navMeshAgent.SetDestination(furthestPoint.position);
		}
	}

	private void SetRandomDestination()
	{
		Vector3 randomDirection = Random.insideUnitSphere * 20f; 
		randomDirection += transform.position;

		NavMeshHit hit;
		if (NavMesh.SamplePosition(randomDirection, out hit, 20f, NavMesh.AllAreas))
		{
			navMeshAgent.SetDestination(hit.position);
		}
	}
}

