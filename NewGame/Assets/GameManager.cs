using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public TextMeshProUGUI countdownText; // UI'de geri sayým göstergesi
	public GameObject player;
	public GameObject aiCharacter;
	private int collectedCubes = 0;
	private float countdownTime = 120f; // 2 dakika
	private bool countdownStarted = false;
	private bool gameEnded = false;

	private void OnEnable()
	{
		CubeCollect.OnCubeCollected += HandleCubeCollected;
	}

	private void OnDisable()
	{
		CubeCollect.OnCubeCollected -= HandleCubeCollected;
	}

	private void HandleCubeCollected()
	{
		collectedCubes++;
		if (collectedCubes >= 1 && !countdownStarted)
		{
			StartCoroutine(StartCountdown());
		}
	}

	private IEnumerator StartCountdown()
	{
		countdownStarted = true;
		countdownText.gameObject.SetActive(true);

		while (countdownTime > 0 && !gameEnded)
		{
			countdownText.text = "Time Left: " + Mathf.Ceil(countdownTime).ToString() + "s";
			countdownTime -= Time.deltaTime;
			yield return null;
		}

		if (!gameEnded)
		{
			LoseGame();
		}
	}

	private void Update()
	{
		if (countdownStarted && !gameEnded)
		{
			float distanceToAI = Vector3.Distance(player.transform.position, aiCharacter.transform.position);

			// Yapay zekayý yakalama koþulu
			if (distanceToAI < 2f) // Örneðin 2 birim mesafe içinde
			{
				WinGame();
			}
		}
	}

	private void WinGame()
	{
		gameEnded = true;
		countdownText.text = "You Win!";
		countdownText.color = Color.green;
		StopAllCoroutines();
	}

	private void LoseGame()
	{
		gameEnded = true;
		countdownText.text = "Time's up! You Lose!";
		countdownText.color = Color.red;
	}
}
