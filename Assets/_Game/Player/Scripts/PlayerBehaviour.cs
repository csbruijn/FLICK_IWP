using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    [SerializeField] private float sprintBonus = 3f;
    [SerializeField] private float smoothtime = 0.1f;
    [SerializeField] private float currentVelocity;

    private CharacterController CC;
    private Vector2 moveVector;
    private float sprintSpeed = 1f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;
    private float verticalVelocity;
    private bool jumpInput;

    [Header("Platform Interaction")]
    [SerializeField] private float launchCooldown = 0.2f;
    private float lastLaunchTime;
    private KeyBoardPlatform currentPlatform;

    private void Awake()
    {
        CC = GetComponent<CharacterController>();
        var playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Player");
    }

    private void Update()
    {
        // sprint decay
        sprintSpeed = Mathf.MoveTowards(sprintSpeed, 1f, Time.deltaTime * sprintBonus);

        HandleGravityAndJump();
        MovePlayer();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && sprintSpeed <= 1f)
            sprintSpeed = sprintBonus;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded())
            jumpInput = true;
    }

    private void HandleGravityAndJump()
    {
        if (IsGrounded())
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -2f;

            if (jumpInput)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpInput = false;
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    public void LaunchPlayer(Component sender, object data)
    {
        // Prevent spammy launches
        if (Time.time - lastLaunchTime < launchCooldown) return;

        if (!(sender is KeyBoardPlatform senderPlatform))
        {
            Debug.LogWarning("Launch ignored: sender is not a KeyBoardPlatform.");
            return;
        }

        // Allow slight tolerance: either standing on or just above platform
        bool validSource = currentPlatform == senderPlatform ||
                           Vector3.Distance(transform.position, senderPlatform.transform.position) < 2f;

        if (!validSource)
        {
            Debug.Log($"Launch ignored: not on correct platform ({senderPlatform.name})");
            return;
        }

        float launchForce = (float)data;
        if (launchForce <= 0f) return;

        // Apply vertical velocity directly
        verticalVelocity = Mathf.Max(verticalVelocity, 0f); // prevent downward inertia from muting launch
        verticalVelocity += launchForce;

        lastLaunchTime = Time.time;
        Debug.Log($"Player launched with force {launchForce} from {senderPlatform.name}");
    }

    private void MovePlayer()
    {
        Vector3 move = new Vector3(moveVector.x, 0, moveVector.y);
        move = transform.TransformDirection(move);

        Vector3 velocity = move * moveSpeed * sprintSpeed;
        velocity.y = verticalVelocity;

        CC.Move(velocity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        bool foundGround = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, CC.height / 2f + 0.2f);

        if (foundGround)
            currentPlatform = hit.collider.GetComponent<KeyBoardPlatform>();
        else
            currentPlatform = null;

        return foundGround;
    }
}
