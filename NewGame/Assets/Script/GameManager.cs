using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public TextMeshProUGUI countdownText;
	public GameObject player;
	public GameObject aiCharacter;
	public GameObject resultPanel; // Panel
	public TextMeshProUGUI resultText; // Paneldeki sonu� yaz�s�
	public TextMeshProUGUI scoreText; // Puan yaz�s�
	public GameObject okButton; // Tamam butonu

	private int collectedCubes = 0;
	private float countdownTime = 120f;
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
		if (collectedCubes >= 1 && !countdownStarted) // 5 k�p topland���nda saya� ba�las�n
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

			if (distanceToAI < 2f) // AI'ya yak�nl�k kontrol�
			{
				WinGame();
			}
		}
	}

	private void WinGame()
	{
		gameEnded = true;
		countdownText.gameObject.SetActive(false);
		StopAllCoroutines();

		int totalScore = PlayerPrefs.GetInt("TotalScore", 0); // Mevcut toplam skoru al
		int addedScore = CalculateScore(countdownTime); // Kalan s�reye g�re puan hesapla
		totalScore += addedScore; // Yeni puan� ekle

		PlayerPrefs.SetInt("TotalScore", totalScore); // G�ncellenen skoru kaydet
		PlayerPrefs.Save();

		ShowResultPanel("You Win!", Color.green, addedScore);
	}

	private void LoseGame()
	{
		gameEnded = true;
		countdownText.gameObject.SetActive(false);
		StopAllCoroutines();

		ShowResultPanel("Time's up! You Lose!", Color.red, 0); // Kaybetti�i i�in puan yok
	}

	private int CalculateScore(float remainingTime)
	{
		if (remainingTime > 60f)
			return 3;
		else if (remainingTime > 30f)
			return 2;
		else
			return 1;
	}

	private void ShowResultPanel(string resultMessage, Color textColor, int score)
	{
		resultPanel.SetActive(true); // Paneli g�ster
		resultText.text = resultMessage; // Sonu� mesaj�n� ayarla
		resultText.color = textColor;

		if (score > 0)
		{
			scoreText.text = "Score Earned: " + score.ToString();
		}
		else
		{
			scoreText.text = "No Score Earned!";
		}

		okButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
		{
			Time.timeScale = 1; // Zaman� s�f�rdan ��kar
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // Bir �nceki sahneye git
		});
	}
}
