using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
	[SerializeField] private Transform[] spawnPoints; // 50 farklý nokta
	[SerializeField] private GameObject cubePrefab;   // Küp prefab'ý
	[SerializeField] private int cubeCount = 5;       // Her oyunda belirecek küp sayýsý

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
		// Önceki oyunlardan kalan küpleri temizle
		foreach (var cube in spawnedCubes)
		{
			Destroy(cube);
		}
		spawnedCubes.Clear();

		// 50 noktadan rastgele 5 tanesini seç
		HashSet<int> selectedIndices = new HashSet<int>();
		while (selectedIndices.Count < cubeCount)
		{
			int randomIndex = Random.Range(0, spawnPoints.Length);
			selectedIndices.Add(randomIndex);
		}

		// Seçilen noktalara küpleri yerleþtir
		foreach (int index in selectedIndices)
		{
			GameObject cube = Instantiate(cubePrefab, spawnPoints[index].position, Quaternion.identity);
			spawnedCubes.Add(cube);
		}
	}
}
