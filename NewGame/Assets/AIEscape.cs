using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEscape : MonoBehaviour
{
	public Transform player; // Oyuncunun Transform'u
	public float detectionRadius = 10f; // AI'nýn oyuncuyu algýlayabileceði mesafe
	public LayerMask obstacleLayerMask; // Engeller için katman

	private NavMeshAgent navMeshAgent;
	private bool isEscaping = false;

	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		SetRandomDestination(); // Baþlangýçta rastgele bir hedef belirle
	}

	private void Update()
	{
		if (CanSeePlayer())
		{
			// Oyuncuyu gördüðünde en uzak noktaya kaç
			EscapeToFurthestPoint();
			isEscaping = true;
		}
		else
		{
			// Oyuncu görünmüyorsa haritada rastgele dolaþ
			if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 0.5f)
			{
				SetRandomDestination();
				isEscaping = false;
			}
		}
	}

	private bool CanSeePlayer()
	{
		// Oyuncu ile AI arasýndaki mesafeyi kontrol et
		float distanceToPlayer = Vector3.Distance(transform.position, player.position);

		if (distanceToPlayer <= detectionRadius)
		{
			// AI'dan oyuncuya doðru bir ray (ýþýn) gönder
			Vector3 directionToPlayer = (player.position - transform.position).normalized;
			RaycastHit hit;

			// Raycast ile engel olup olmadýðýný kontrol et
			if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius, ~obstacleLayerMask))
			{
				// Eðer raycast sonucu doðrudan oyuncuya ulaþýyorsa
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
		Vector3 randomDirection = Random.insideUnitSphere * 20f; // Rastgele bir yön belirleyin
		randomDirection += transform.position;

		NavMeshHit hit;
		if (NavMesh.SamplePosition(randomDirection, out hit, 20f, NavMesh.AllAreas))
		{
			navMeshAgent.SetDestination(hit.position);
		}
	}
}

