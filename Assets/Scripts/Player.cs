using System.Data;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;
    private HealthManager healthManager;

    public float normalJumpForce = 9f;
    public float highJumpForce = 15f;
    public float jumpThreshold = 20f;
    public float highJumpThreshold = 40f;
    public float gravity = 9.81f * 2f;
    public static Player instance;
    public bool isJump = true;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
        instance = this;

        // Get HealthManager and add error checking
        healthManager = GetComponent<HealthManager>();
        if (healthManager == null)
        {
            Debug.LogError("HealthManager component not found on Player!");
            healthManager = gameObject.AddComponent<HealthManager>();
        }
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
    }

    private void Update()
    {
        JumpFunction();
    }

    public void JumpFunction()
    {
        // Ground check
        if (character.isGrounded)
        {
            isJump = true;
            direction = Vector3.zero;
        }

        // Jump check
        if (isJump && Read_Arduino.instance.sendValueFlow >= jumpThreshold)
        {
            // normalJumpForce 
            direction = Vector3.up * normalJumpForce;
            // gravity 
            gravity = Read_Arduino.instance.sendValueFlow >= 30 ? 14f : 19.62f;
            isJump = false;
            Debug.Log($"Jump Force: {normalJumpForce}, Gravity: {gravity}");
        }

        // Apply gravity and movement
        direction += gravity * Time.deltaTime * Vector3.down;
        character.Move(direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collision with: {other.gameObject.name}, Tag: {other.tag}");
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle hit detected");
            if (healthManager != null)
            {
                healthManager.TakeDamage(33.3f);
                Debug.Log($"Damage dealt. Hits: {healthManager.currentHits}, Health: {healthManager.healthAmount}");
            }
            else
            {
                Debug.LogError("HealthManager is null!");
            }
        }
    }
}