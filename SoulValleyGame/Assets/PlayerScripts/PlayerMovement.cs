using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float currentSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float gravity = -9.81f;
    public float groundDrag;


    public float jumpForce;
    public float jumpColdown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Animation")]
    private Animator animator;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    Vector3 cameraForward;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        air
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ResetJump();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        MyInput();
        StateHandler();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Jump trigger
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            

            Jump();

            Invoke(nameof(ResetJump), jumpColdown);
        }
    }

    private void StateHandler()
    {
        if(grounded && Input.GetButton("Sprint"))
        {
            state = MovementState.sprinting;
            currentSpeed = sprintSpeed;
            animator.SetBool("Running", true);
        }
        else if (grounded)
        {

            state = MovementState.walking;
            currentSpeed = walkSpeed;
            animator.SetBool("Running", false);
            animator.SetBool("isJumping", false);

        }
        else
        {
            animator.SetBool("isJumping", true);
            state = MovementState.air;
            
        }
    }

    private void MovePlayer()
    {
        
        cameraForward = Vector3.Scale(orientation.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = (verticalInput * cameraForward + horizontalInput * orientation.right).normalized;
        movement *= currentSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
               
        animator.SetFloat("Speed", movement.magnitude);

        Debug.Log("Movement: " + movement.magnitude);
    }

   

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        
        rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
    }
    
    private void ResetJump()
    {
        readyToJump = true;
    }

}
