using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyboardControls : MonoBehaviour
{
	private CharacterController characterController;
	[SerializeField] private float _speed = 4f, _XSpeed = 6f, _jump = 1f, _gravity = -9.8f;
	[SerializeField] Transform groundCheck;
	[SerializeField] float groundDistance = 0.3f;
	[SerializeField] LayerMask groundLayerMask;
	Vector3 _velocity;
	bool isGrounded;

	[SerializeField] private Animator animator;

	private void Start()
	{
		characterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayerMask);

		if (isGrounded && _velocity.y < 0)
		{
			_velocity.y = -2;
			animator.SetBool("isJumping", false);
		}

		float x = Input.GetAxis("Horizontal"); // A ve D veya sol-sað ok tuþlarý
		float z = Input.GetAxis("Vertical"); // W ve S veya yukarý-aþaðý ok tuþlarý

		Vector3 move = transform.right * x + transform.forward * z;

		if (z > 0.8f)
		{
			characterController.Move(move * _XSpeed * Time.deltaTime);
			animator.SetBool("isRunning", true);
			animator.SetBool("isWalking", false);
		}
		else if (move.magnitude > 0)
		{
			characterController.Move(move * _speed * Time.deltaTime);
			animator.SetBool("isRunning", false);
			animator.SetBool("isWalking", true);
		}
		else
		{
			animator.SetBool("isRunning", false);
			animator.SetBool("isWalking", false);
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}

		_velocity.y += _gravity * Time.deltaTime;
		characterController.Move(_velocity * Time.deltaTime);
	}

	public void Jump()
	{
		if (isGrounded)
		{
			_velocity.y = Mathf.Sqrt(_jump * -2f * _gravity);
			animator.SetBool("isJumping", true);
		}
	}
}