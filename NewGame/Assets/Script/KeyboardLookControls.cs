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

		// Sað ve Sol ok tuþlarý için yatay hareket
		if (Input.GetKey(KeyCode.RightArrow))
			X = sensivity * Time.deltaTime;
		else if (Input.GetKey(KeyCode.LeftArrow))
			X = -sensivity * Time.deltaTime;

		// Yukarý ve Aþaðý ok tuþlarý için dikey hareket
		if (Input.GetKey(KeyCode.UpArrow))
			Y = sensivity * Time.deltaTime;
		else if (Input.GetKey(KeyCode.DownArrow))
			Y = -sensivity * Time.deltaTime;

		// Yukarý-aþaðý hareket sýnýrlandýrmasý (xRotation)
		xRotation -= Y;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		// Kamerayý yukarý-aþaðý döndürme
		transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

		// Karakteri saða ve sola döndürme
		Player.Rotate(Vector3.up * X);
	}
}
