using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollect : MonoBehaviour
{
	public delegate void CubeCollectedHandler();
	public static event CubeCollectedHandler OnCubeCollected;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			OnCubeCollected?.Invoke();
			Destroy(gameObject);
		}
	}
}
