using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardLookControls : MonoBehaviour
{
	[SerializeField] float sensivity = 100f;
	[SerializeField] Transform Player;

	float xRotation = 0f;

	private void Update()
	{
		float X = 0f;
		float Y = 0f;

		// Sa� ve Sol ok tu�lar� i�in yatay hareket
		if (Input.GetKey(KeyCode.RightArrow))
			X = sensivity * Time.deltaTime;
		else if (Input.GetKey(KeyCode.LeftArrow))
			X = -sensivity * Time.deltaTime;

		// Yukar� ve A�a�� ok tu�lar� i�in dikey hareket
		if (Input.GetKey(KeyCode.UpArrow))
			Y = sensivity * Time.deltaTime;
		else if (Input.GetKey(KeyCode.DownArrow))
			Y = -sensivity * Time.deltaTime;

		// Yukar�-a�a�� hareket s�n�rland�rmas� (xRotation)
		xRotation -= Y;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		// Kameray� yukar�-a�a�� d�nd�rme
		transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

		// Karakteri sa�a ve sola d�nd�rme
		Player.Rotate(Vector3.up * X);
	}
}
