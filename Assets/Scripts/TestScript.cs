using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    public float jumpForce = 8f;
    public float gravity = 9.81f * 2f;

    private bool isJumping = false;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
    }

    private void Update()
    {
        if (character.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                direction = Vector3.up * jumpForce;
            }
        }
        else
        {
            if (Input.GetButtonUp("Jump"))
            {
                isJumping = false;
            }
        }

        if (!isJumping)
        {
            direction += gravity * Time.deltaTime * Vector3.down;
        }

        character.Move(direction * Time.deltaTime);

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
