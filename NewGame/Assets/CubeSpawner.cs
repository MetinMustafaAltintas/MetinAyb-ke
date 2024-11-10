using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
	[SerializeField] private Transform[] spawnPoints; // 50 farkl� nokta
	[SerializeField] private GameObject cubePrefab;   // K�p prefab'�
	[SerializeField] private int cubeCount = 5;       // Her oyunda belirecek k�p say�s�

	private List<GameObject> spawnedCubes = new List<GameObject>();

	private void Start()
	{
		SpawnCubes();
	}

	private void Update()
	{
		gameObject.SetActive(false);
	}

	private void SpawnCubes()
	{
		// �nceki oyunlardan kalan k�pleri temizle
		foreach (var cube in spawnedCubes)
		{
			Destroy(cube);
		}
		spawnedCubes.Clear();

		// 50 noktadan rastgele 5 tanesini se�
		HashSet<int> selectedIndices = new HashSet<int>();
		while (selectedIndices.Count < cubeCount)
		{
			int randomIndex = Random.Range(0, spawnPoints.Length);
			selectedIndices.Add(randomIndex);
		}

		// Se�ilen noktalara k�pleri yerle�tir
		foreach (int index in selectedIndices)
		{
			GameObject cube = Instantiate(cubePrefab, spawnPoints[index].position, Quaternion.identity);
			spawnedCubes.Add(cube);
		}
	}
}
