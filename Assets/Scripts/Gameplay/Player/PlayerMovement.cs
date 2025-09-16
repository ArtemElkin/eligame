using UnityEngine;
using Core.Input;

namespace Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 5f;
        public float jumpForce = 5f;

        private Rigidbody _rb;
        private Camera _mainCamera;
        private InputModule _input; // ← ссылка на твой модуль ввода

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            
        }

        private void Start()
        {
            _mainCamera = Camera.main;
            _input = FindFirstObjectByType<InputModule>();
        }
        private void FixedUpdate()
        {
            HandleMovement();
            HandleJump();
            RotateTowardsMouse();
        }

        private void HandleMovement()
        {
            Vector2 moveInput = _input.Move;
            Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            Vector3 move = moveSpeed * Time.fixedDeltaTime * moveDir;

            _rb.MovePosition(_rb.position + move);
        }

        private void HandleJump()
        {
            if (IsGrounded() && _input.JumpPressed)
            {
                _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        private void RotateTowardsMouse()
        {
            Ray ray = _mainCamera.ScreenPointToRay(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Vector3 lookDir = hit.point - transform.position;
                lookDir.y = 0f;

                if (lookDir.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                    _rb.MoveRotation(targetRotation);
                }
            }
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, 1.1f);
        }
    }
}
