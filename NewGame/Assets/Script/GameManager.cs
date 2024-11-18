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
	public TextMeshProUGUI resultText; // Paneldeki sonuç yazýsý
	public TextMeshProUGUI scoreText; // Puan yazýsý
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
		if (collectedCubes >= 1 && !countdownStarted) // 5 küp toplandýðýnda sayaç baþlasýn
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

			if (distanceToAI < 2f) // AI'ya yakýnlýk kontrolü
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
		int addedScore = CalculateScore(countdownTime); // Kalan süreye göre puan hesapla
		totalScore += addedScore; // Yeni puaný ekle

		PlayerPrefs.SetInt("TotalScore", totalScore); // Güncellenen skoru kaydet
		PlayerPrefs.Save();

		ShowResultPanel("You Win!", Color.green, addedScore);
	}

	private void LoseGame()
	{
		gameEnded = true;
		countdownText.gameObject.SetActive(false);
		StopAllCoroutines();

		ShowResultPanel("Time's up! You Lose!", Color.red, 0); // Kaybettiði için puan yok
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
		resultPanel.SetActive(true); // Paneli göster
		resultText.text = resultMessage; // Sonuç mesajýný ayarla
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
			Time.timeScale = 1; // Zamaný sýfýrdan çýkar
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // Bir önceki sahneye git
		});
	}
}
