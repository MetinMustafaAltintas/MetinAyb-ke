using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEscape : MonoBehaviour
{
	public Transform player; // Oyuncunun Transform'u
	public float detectionRadius = 10f; // AI'n�n oyuncuyu alg�layabilece�i mesafe
	public LayerMask obstacleLayerMask; // Engeller i�in katman

	private NavMeshAgent navMeshAgent;
	private bool isEscaping = false;

	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		SetRandomDestination(); // Ba�lang��ta rastgele bir hedef belirle
	}

	private void Update()
	{
		if (CanSeePlayer())
		{
			// Oyuncuyu g�rd���nde en uzak noktaya ka�
			EscapeToFurthestPoint();
			isEscaping = true;
		}
		else
		{
			// Oyuncu g�r�nm�yorsa haritada rastgele dola�
			if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 0.5f)
			{
				SetRandomDestination();
				isEscaping = false;
			}
		}
	}

	private bool CanSeePlayer()
	{
		// Oyuncu ile AI aras�ndaki mesafeyi kontrol et
		float distanceToPlayer = Vector3.Distance(transform.position, player.position);

		if (distanceToPlayer <= detectionRadius)
		{
			// AI'dan oyuncuya do�ru bir ray (���n) g�nder
			Vector3 directionToPlayer = (player.position - transform.position).normalized;
			RaycastHit hit;

			// Raycast ile engel olup olmad���n� kontrol et
			if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius, ~obstacleLayerMask))
			{
				// E�er raycast sonucu do�rudan oyuncuya ula��yorsa
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
		Vector3 randomDirection = Random.insideUnitSphere * 20f; // Rastgele bir y�n belirleyin
		randomDirection += transform.position;

		NavMeshHit hit;
		if (NavMesh.SamplePosition(randomDirection, out hit, 20f, NavMesh.AllAreas))
		{
			navMeshAgent.SetDestination(hit.position);
		}
	}
}

