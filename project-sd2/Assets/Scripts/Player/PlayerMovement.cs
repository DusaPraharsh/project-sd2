using Unity.Mathematics.Geometry;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform cam;
    Animator anim;

    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;

    public float turnSmoothTime;
    float turnSmoothVelocity;

    private void Start()
    {
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * walkSpeed * Time.deltaTime);
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }

        if (direction.magnitude >= 0.1f && Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * walkSpeed * Time.deltaTime);
            anim.SetBool("sprint", true);
        }
        else
        {
            anim.SetBool("sprint", false);
        }
    }
}
