using Unity.Mathematics.Geometry;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform cam;
    public Transform groundCheck;
    public LayerMask groundLayer;
    Animator anim;

    [Header("Movement")]
    private float currentSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    [Header("Jump, Gravity")]
    public float jumpHeight;
    public float gravity = -9.81f;
    private float gravityMultiplier = 3.0f;

    [Header("Crouch")]
    public float crouchSpeed;
    private bool isCrouching;

    [Header("Camera Smoothing")]
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("Grounded")]
    public float groundDistance = 0.4f;
    private bool isGrounded;
    Vector3 velocity;
    Vector3 direction;

    private bool isSprinting;

    private void Start()
    {
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        MoveDirection();
        MovePlayer();
        GravityAndJump();
        Crouch();
    }

    private void MoveDirection()
    {
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void MovePlayer()
    {
        direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (direction.magnitude >= 0.1f)
        {
            isSprinting = Input.GetKey(KeyCode.LeftShift);

            if (isCrouching)
            {
                currentSpeed = crouchSpeed;
                isSprinting = false;
            }
            else
            {
                isSprinting = Input.GetKey(KeyCode.LeftShift);
                currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
            }

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);

            anim.SetBool("walk", true);
            anim.SetBool("sprint", isSprinting);
        }
        else
        {
            isSprinting = false;

            anim.SetBool("walk", false);
            anim.SetBool("sprint", false);
        }
    }

    private void GravityAndJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetTrigger("jump");
        }

        velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            anim.SetBool("crouching", isCrouching);
        }
    }
}
