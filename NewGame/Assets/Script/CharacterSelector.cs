using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
	public Image characterImage;
	public Text characterNameText;
	public Button selectButton, nextButton, previousButton;

	public Sprite[] characterSprites;
	public string[] characterNames;
	public string sceneNames;

	private int currentIndex = 0;
	private int[] ownedCharacterIndices; // Sadece sahip olunan karakterlerin indeksleri
	private string ownedCharactersKey = "OwnedCharacters";

	private void Start()
	{
		// Sahip olunan karakterlerin indekslerini belirle
		ownedCharacterIndices = GetOwnedCharacterIndices();

		if (ownedCharacterIndices.Length == 0)
		{
			Debug.LogError("Kullanýcýnýn sahip olduðu hiçbir karakter yok!");
			return; // Eðer kullanýcýya ait karakter yoksa devam etmeyiz.
		}

		UpdateCharacterDisplay();

		nextButton.onClick.AddListener(NextCharacter);
		previousButton.onClick.AddListener(PrevCharacter);
		selectButton.onClick.AddListener(SelectCharacter);
	}

	private int[] GetOwnedCharacterIndices()
	{
		// Sahip olunan karakterlerin indekslerini bir listeye ekle
		var ownedIndices = new System.Collections.Generic.List<int>();
		for (int i = 0; i < characterSprites.Length; i++)
		{
			if (PlayerPrefs.GetInt(ownedCharactersKey + i, 0) == 1)
			{
				ownedIndices.Add(i);
			}
		}
		return ownedIndices.ToArray();
	}

	public void UpdateCharacterDisplay()
	{
		int ownedIndex = ownedCharacterIndices[currentIndex]; // Sadece sahip olunan karakterlerin indekslerini kullan
		characterImage.sprite = characterSprites[ownedIndex];
		characterNameText.text = characterNames[ownedIndex];
	}

	public void NextCharacter()
	{
		currentIndex = (currentIndex + 1) % ownedCharacterIndices.Length;
		UpdateCharacterDisplay();
	}

	public void PrevCharacter()
	{
		currentIndex = (currentIndex - 1 + ownedCharacterIndices.Length) % ownedCharacterIndices.Length;
		UpdateCharacterDisplay();
	}

	public void SelectCharacter()
	{
		int ownedIndex = ownedCharacterIndices[currentIndex];
		string selectedCharacterName = characterNames[ownedIndex];
		PlayerPrefs.SetString("SelectedCharacter", selectedCharacterName); // Seçili karakteri kaydet
		Debug.Log("Selected Character: " + selectedCharacterName);
		SelectScene();
	}

	public void SelectScene()
	{
		string selectedSceneName = sceneNames;
		SceneManager.LoadScene(selectedSceneName);
	}
}