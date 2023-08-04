using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using FMOD.Studio;
using FMODUnity;

public class PlayerMovement : MonoBehaviour
{
    
    private PhotonView view;
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

    public KeyCode throwKey = KeyCode.Q;
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

    private bool invStatus;

    public MovementState state;

    private EventInstance playerFootSteps;
    private EventInstance playerFootStepsSprint;
    FMOD.ATTRIBUTES_3D attributes;
    bool isPlayingSprinting = false;
    public enum MovementState
    {
        idle,
        walking,
        sprinting,
        air
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        attributes = new FMOD.ATTRIBUTES_3D();
        attributes.position = RuntimeUtils.ToFMODVector(transform.position); // Set the position in 3D space
        attributes.velocity = RuntimeUtils.ToFMODVector(Vector3.zero); // Set the velocity (optional)
        attributes.forward = RuntimeUtils.ToFMODVector(orientation.forward); // Set the forward vector (optional)
        attributes.up = RuntimeUtils.ToFMODVector(orientation.up);
        playerFootSteps = AudioManager.instance.CreateInstance(FMODEvents.instance.footSteps);
        playerFootSteps.setVolume(0.5f);
        playerFootStepsSprint = AudioManager.instance.CreateInstance(FMODEvents.instance.footStepsSprint);
        playerFootStepsSprint.setVolume(0.5f);
        playerFootSteps.set3DAttributes(attributes);
        playerFootStepsSprint.set3DAttributes(attributes);
        rb.freezeRotation = true;
        ResetJump();
    }

    private void Update()
    {
        if(view.IsMine){
            invStatus = InventoryUIControler.status;
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
            UpdateSound();
            MyInput();           
            StateHandler();
            
            if (grounded)
                rb.drag = groundDrag;
            else
                rb.drag = 0;
        }
        
    }

    private void FixedUpdate()
    {
        if(view.IsMine){
            MovePlayer();
        }
    }

    private void MyInput()
    {
        
        if (invStatus)
        {
            horizontalInput = 0f;
            verticalInput = 0f;
        }
        else
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }
        if(horizontalInput == 0 && verticalInput == 0)
        {
            state = MovementState.idle;
        }
        else
        {
            state = MovementState.walking;
        }
        //Jump trigger
        if (Input.GetKey(jumpKey) && readyToJump && grounded && !invStatus)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpColdown);
        }
        if (Input.GetKey(throwKey) && grounded && !invStatus)
        {
            animator.SetTrigger("Throwing");
        }
    }

    private void StateHandler()
    {
        if(grounded && Input.GetButton("Sprint"))
        {
            isPlayingSprinting = true;
            state = MovementState.sprinting;
            currentSpeed = sprintSpeed;
            animator.SetBool("Running", true);
            animator.SetBool("isJumping", false);
        }
        else if (state == MovementState.idle)
        {
            currentSpeed = walkSpeed;
        }
        else if (grounded)
        {
            isPlayingSprinting = false;
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

    private void UpdateSound()
    {
        attributes.position = RuntimeUtils.ToFMODVector(transform.position); // Set the position in 3D space
        attributes.velocity = RuntimeUtils.ToFMODVector(Vector3.zero); // Set the velocity (optional)
        attributes.forward = RuntimeUtils.ToFMODVector(orientation.forward); // Set the forward vector (optional)
        attributes.up = RuntimeUtils.ToFMODVector(orientation.up);
        playerFootSteps.set3DAttributes(attributes);
        playerFootStepsSprint.set3DAttributes(attributes);
        if(isPlayingSprinting == true)
        {
            if (state == MovementState.sprinting && currentSpeed == sprintSpeed)
            {
                PLAYBACK_STATE playbackState;
                playerFootStepsSprint.getPlaybackState(out playbackState);
                if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
                {
                    playerFootStepsSprint.start();
                    playerFootSteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                }
            }
            else
            {
                
                playerFootStepsSprint.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
        else
        {
            if (state == MovementState.walking && currentSpeed == walkSpeed)
            {
                PLAYBACK_STATE playbackState;
                playerFootSteps.getPlaybackState(out playbackState);
                if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
                {
                    playerFootSteps.start();
                    playerFootStepsSprint.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                }
            }
            else
            {
                playerFootSteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
        
        
        
    }

}
