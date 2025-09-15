using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody _rb;
    private InputAction _moveAction;
    private InputAction _jumpAction;

    private bool _jumpPressed;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        var playerInput = GetComponent<PlayerInput>();
        _moveAction = playerInput.actions["Move"];
        _jumpAction = playerInput.actions["Jump"];
    }

    private void OnEnable()
    {
        _jumpAction.performed += OnJump;
    }

    private void OnDisable()
    {
        _jumpAction.performed -= OnJump;
    }

    private void FixedUpdate()
    {
        // ===== Ходьба =====
        Vector2 input = _moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(input.x, 0f, input.y).normalized;

        Vector3 move = moveSpeed * Time.fixedDeltaTime * moveDir;
        _rb.MovePosition(_rb.position + move);

        // ===== Прыжок =====
        if (IsGrounded() && _jumpPressed)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _jumpPressed = false;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _jumpPressed = true;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}