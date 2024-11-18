using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
	[SerializeField] float waittime = 4f;

	private void Start()
	{
		StartCoroutine(WaitAndLoadMainMenu());
	}

	private IEnumerator WaitAndLoadMainMenu()
	{
		yield return new WaitForSeconds(waittime);
		SceneManager.LoadScene("StartMenu");
	}
}
