using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody mRB;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Ground Check Settings")]
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    private bool isGrounded;
    private Vector2 moveInput;

    private void Start()
    {
        mRB = GetComponent<Rigidbody>();
        mRB.freezeRotation = true; // prevent tipping over
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        MovePlayer();
    }

    private void CheckGrounded()
    {
        isGrounded = IsGrounded();
        //Debug.Log($"player is grounded: {isGrounded}");
    }

    private bool IsGrounded()
    {
        float GroundedDistance = 2f;
        if (mRB.linearVelocity.y == 0)
        {
            return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, GroundedDistance);
        }
        return false; 
    }

        private void MovePlayer()
    {
        // Convert input (x,z)
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed;
        Vector3 velocity = mRB.linearVelocity;
        velocity.x = move.x;
        velocity.z = move.z;
        mRB.linearVelocity = velocity;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {

        if (!isGrounded) Debug.Log("Not on ground"); 
        if (context.started && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        // Reset Y velocity to avoid �stacked� jumps
        Vector3 velocity = mRB.linearVelocity;
        velocity.y = 0f;
        mRB.linearVelocity = velocity;

        // Add upward impulse
        mRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        Debug.Log("Jump!");
    }

    private void OnDrawGizmosSelected()
    {
        // visualize ground check ray
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}