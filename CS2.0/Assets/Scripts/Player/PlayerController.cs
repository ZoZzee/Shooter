using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canMove;

    [Header("Moving")]
    public float walkingSpeed;
    public float runningSpeed;
    private float velocityDiagonalLimiter = 0.7072f;

    public float jumpForce;
    public float gravity;

    [Header("Rotation")]
    public Camera mainCamera;

    public float lookSpeed;
    public float lookXLimit;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX;

    [SerializeField] public Animator handAnimator;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {
        Movement();
        Rotation();
    }

    private void LateUpdate()
    {
        handAnimator.SetFloat("Velocity", characterController.velocity.magnitude);
    }

    private void Movement()
    {
        Vector3 forwardDirection = transform.TransformDirection(Vector3.forward);
        Vector3 rightDirection = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        //float curSpeedZ = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;

        float curSpeedZ = 0;
        float curSpeedX = 0;

        if (canMove)
        {
            if (isRunning)
            {
                curSpeedZ = runningSpeed;
                curSpeedX = runningSpeed;
            }
            else
            {
                curSpeedZ = walkingSpeed;
                curSpeedX = walkingSpeed;
            }

            curSpeedZ *= Input.GetAxis("Vertical");
            curSpeedX *= Input.GetAxis("Horizontal");
        }


        float moveDirectionY = moveDirection.y;

        moveDirection = (forwardDirection * curSpeedZ) + (rightDirection * curSpeedX);

        if (Input.GetKey(KeyCode.Space) && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpForce;
        }
        else
        {
            moveDirection.y = moveDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        Vector3 limitedDirection = moveDirection;
        if ((Input.GetAxis("Horizontal")== -1 || Input.GetAxis("Horizontal") == 1)&& (Input.GetAxis("Vertical") == -1 || Input.GetAxis("Vertical") == 1))
        {
            limitedDirection.x *= velocityDiagonalLimiter;
            limitedDirection.z *= velocityDiagonalLimiter;
        }

        moveDirection = limitedDirection;

        characterController.Move(moveDirection * Time.deltaTime);

    }

    

    private void Rotation()
    {
        rotationX += Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        Camera.main.transform.localRotation = Quaternion.Euler(-rotationX, 0, 0);

        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse" +
            " X") * lookSpeed, 0);
    }
}
