using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
	public GameObject mainCamera;   
	public GameObject topCamera;    

	private bool isMainCameraActive = true;

	public void SwitchCamera()
	{
		if (isMainCameraActive)
		{
			mainCamera.SetActive(false);
			topCamera.SetActive(true);
		}
		else
		{
			mainCamera.SetActive(true);
			topCamera.SetActive(false);
		}
		isMainCameraActive = !isMainCameraActive;
	}
}