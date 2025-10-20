using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float sprintSpeed = 9.0f;
    [SerializeField] float gravity = -30f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    [Header("Jumping")]
    public float jumpHeight = 6f;

    [Header("Stamina Settings")]
    [SerializeField] float maxStamina = 2f; // sprint time in seconds
    [SerializeField] float staminaRegenRate = 1f; // seconds regained per second
    [SerializeField] float staminaDrainRate = 1f; // seconds drained per second

    float currentStamina;
    float velocityY;
    bool isGrounded;
    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        UpdateMouse();
        UpdateMove();
    }

    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraCap -= currentMouseDelta.y * mouseSensitivity;
        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraCap;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, mouseSmoothTime);

        bool isMoving = currentDir.magnitude > 0.1f;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && isMoving;

        // Handle sprinting and stamina
        float currentSpeed = walkSpeed;
        if (isSprinting)
        {
            currentSpeed = sprintSpeed;
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina < 0) currentStamina = 0;
        }
        else
        {
            if (currentStamina < maxStamina)
                currentStamina += staminaRegenRate * Time.deltaTime;
        }

        // Gravity and movement
        velocityY += gravity * 2f * Time.deltaTime;
        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * currentSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);

        // Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (isGrounded && controller.velocity.y < -1f)
        {
            velocityY = -8f;
        }
    }

    // Optional: for debugging stamina
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), "Stamina: " + Mathf.Round(currentStamina * 100f) / 100f);
    }
}
