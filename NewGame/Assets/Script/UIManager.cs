using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void JoinGame()
    {
        SceneManager.LoadScene("Game");
    }
   
	public void QuitGame()
	{
		Debug.Log("Oyun kapatýlýyor...");
		Application.Quit();
	}

}