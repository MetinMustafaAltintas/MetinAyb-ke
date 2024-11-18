using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
	public Image characterImage;
	public Text priceText;
	public Text totalScoreText;

	public Button actionButton, nextButton, previousButton;

	public Sprite[] characterSprites;
	public int[] characterPrices;
	public string[] characterNames;

	private int currentIndex = 0;
	private int totalscore = 0;
	private bool[] ownedCharacter;

	private string ownedCharacterKey = "OwnedCharacters";

	private void Start()
	{
		totalscore = PlayerPrefs.GetInt("TotalScore", 0);
		totalScoreText.text = "Total Score:" + totalscore.ToString();

		ownedCharacter = new bool[characterSprites.Length];
		for (int i = 0; i < characterSprites.Length; i++)
		{
			ownedCharacter[i] = PlayerPrefs.GetInt(ownedCharacterKey + i, 0) == 1 ? true : false;
		}

		UpdatedCharacterDisplay();

		nextButton.onClick.AddListener(NextCharacter);
		previousButton.onClick.AddListener(PreviousCharacter);
		actionButton.onClick.AddListener(ActionButtonPressed);
	}

	public void UpdatedCharacterDisplay()
	{
		characterImage.sprite = characterSprites[currentIndex];

		if (ownedCharacter[currentIndex])
		{
			actionButton.GetComponentInChildren<Text>().text = characterNames[currentIndex];
			actionButton.onClick.RemoveAllListeners();
			actionButton.onClick.AddListener(SelectCharacter);
			priceText.text = "";
		}
		else
		{
			actionButton.GetComponentInChildren<Text>().text = "Satýn Al";
			actionButton.onClick.RemoveAllListeners();
			actionButton.onClick.AddListener(BuyCharacter);
			priceText.text = "Price: " + characterPrices[currentIndex].ToString();

			if (totalscore >= characterPrices[currentIndex])
			{
				actionButton.interactable = true;
			}
			else
			{
				actionButton.interactable = false;
			}
		}
	}

	public void NextCharacter()
	{
		currentIndex = (currentIndex + 1) % characterSprites.Length;
		UpdatedCharacterDisplay();
	}
	public void PreviousCharacter()
	{
		currentIndex = (currentIndex - 1 + characterSprites.Length) % characterSprites.Length;
		UpdatedCharacterDisplay();
	}

	public void ActionButtonPressed()
	{
		if (ownedCharacter[currentIndex])
		{
			SelectCharacter();
		}
		else
		{
			BuyCharacter();
		}
	}

	public void SelectCharacter()
	{
		if (ownedCharacter[currentIndex])
		{
			string selectedCarPrefabName = characterNames[currentIndex];
			PlayerPrefs.SetString("SelectedCarPrefab", selectedCarPrefabName);
		}
	}

	public void BuyCharacter()
	{
		if (totalscore >= characterPrices[currentIndex] && !ownedCharacter[currentIndex])
		{
			totalscore -= characterPrices[currentIndex];
			PlayerPrefs.SetInt("TotalScore", totalscore);
			totalScoreText.text = "Total Score : " + totalscore.ToString();

			ownedCharacter[currentIndex] = true;
			PlayerPrefs.SetInt(ownedCharacterKey + currentIndex, 1);

			UpdatedCharacterDisplay();
		}
	}
}

