using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class SageAnimationController : MonoBehaviour
{
    private Animator anim;
    private CharacterController controller;

    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float crouchSpeed = 2f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Input Keys")]
    public Key crouchInput = Key.LeftCtrl;
    public Key jumpInput = Key.Space;
    public Key grabInput = Key.E;
    public Key openDoorInput = Key.O;

    [Header("Action Timing")]
    [Tooltip("How long Grab and Open Door animations stay active after pressing the key")]
    public float actionDuration = 1.0f;

    private Vector3 velocity;
    private bool isGrounded = true;
    private float ungroundedTimer = 0f;
    private float grabTimer = 0f;
    private float openTimer = 0f;

    // Animator Parameter Hashes
    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int isWalkingHash = Animator.StringToHash("IsWalking");
    private readonly int isCrouchingHash = Animator.StringToHash("IsCrouching");
    private readonly int isGrabbingHash = Animator.StringToHash("IsGrabbing");
    private readonly int isJumpingHash = Animator.StringToHash("IsJumping");
    private readonly int isOpeningHash = Animator.StringToHash("IsOpening");
    private readonly int isIdleHash = Animator.StringToHash("IsIdle");

    void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        // Snap character to ground on scene start so jump does not trigger
        velocity.y = -2f;
        controller.Move(Vector3.down * 0.1f);
        isGrounded = true;
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

     
        if (controller.isGrounded)
        {
            isGrounded = true;
            ungroundedTimer = 0f;
            if (velocity.y < 0f) velocity.y = -2f;
        }
        else
        {
            ungroundedTimer += Time.deltaTime;
            if (ungroundedTimer > 0.15f)
            {
                isGrounded = false;
            }
        }

       
        float horizontal = 0f;
        float vertical = 0f;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) horizontal -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) horizontal += 1f;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) vertical -= 1f;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) vertical += 1f;

        Vector3 moveDirection = (transform.right * horizontal + transform.forward * vertical).normalized;
        float currentSpeed = moveDirection.magnitude;

     
        bool isCrouching = IsKeyPressed(keyboard, crouchInput);
        bool jumpPressed = WasKeyPressedThisFrame(keyboard, jumpInput);

        if (WasKeyPressedThisFrame(keyboard, grabInput))
        {
            grabTimer = actionDuration;
        }

        if (WasKeyPressedThisFrame(keyboard, openDoorInput))
        {
            openTimer = actionDuration;
        }

       
        if (grabTimer > 0f) grabTimer -= Time.deltaTime;
        if (openTimer > 0f) openTimer -= Time.deltaTime;

        bool isGrabbing = grabTimer > 0f || IsKeyPressed(keyboard, grabInput);
        bool isOpening = openTimer > 0f || IsKeyPressed(keyboard, openDoorInput);

        
        if (!isGrabbing && !isOpening)
        {
            float activeMoveSpeed = isCrouching ? crouchSpeed : walkSpeed;
            controller.Move(moveDirection * activeMoveSpeed * Time.deltaTime);
        }

        
        if (jumpPressed && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isGrounded = false;
            ungroundedTimer = 0.2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

       
        bool isJumping = !isGrounded;
        bool isWalking = currentSpeed > 0.01f && !isCrouching && !isJumping && !isGrabbing && !isOpening;
        bool isIdle = !isWalking && !isCrouching && !isGrabbing && !isJumping && !isOpening;

        
        anim.SetFloat(speedHash, isWalking ? currentSpeed : 0f);
        anim.SetBool(isWalkingHash, isWalking);
        anim.SetBool(isCrouchingHash, isCrouching);
        anim.SetBool(isGrabbingHash, isGrabbing);
        anim.SetBool(isJumpingHash, isJumping);
        anim.SetBool(isOpeningHash, isOpening);
        anim.SetBool(isIdleHash, isIdle);
    }

    private bool IsKeyPressed(Keyboard keyboard, Key key)
    {
        if (key <= Key.None || (int)key >= 115) return false;
        return keyboard[key].isPressed;
    }

    private bool WasKeyPressedThisFrame(Keyboard keyboard, Key key)
    {
        if (key <= Key.None || (int)key >= 115) return false;
        return keyboard[key].wasPressedThisFrame;
    }
}
