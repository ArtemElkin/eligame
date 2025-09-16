using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public class InputModule : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _lookAction;
        private InputAction _attackAction;
        public Vector2 Move { get; private set; }
        public bool JumpPressed { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Attack { get; private set; }

        private void Awake()
        {
            var gameplayMap = inputActions.FindActionMap("Player");

            _moveAction = gameplayMap.FindAction("Move");
            _jumpAction = gameplayMap.FindAction("Jump");
            _lookAction = gameplayMap.FindAction("Look");
            _attackAction = gameplayMap.FindAction("Attack");
        }

        private void OnEnable()
        {
            _moveAction.Enable();
            _jumpAction.Enable();
            _lookAction.Enable();

            _moveAction.performed += OnMove;
            _moveAction.canceled  += OnMove;

            _jumpAction.started   += OnJump;
            _jumpAction.canceled  += OnJump;

            _lookAction.performed += OnLook;
            _lookAction.canceled  += OnLook;
            
            _attackAction.started  += OnAttack;
            _attackAction.canceled += OnAttack;
        }

        private void OnDisable()
        {
            _moveAction.performed -= OnMove;
            _moveAction.canceled  -= OnMove;

            _jumpAction.started   -= OnJump;
            _jumpAction.canceled  -= OnJump;

            _lookAction.performed -= OnLook;
            _lookAction.canceled  -= OnLook;

            _moveAction.Disable();
            _jumpAction.Disable();
            _lookAction.Disable();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            JumpPressed = context.ReadValueAsButton();
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            Attack =  context.ReadValueAsButton();
        }
    }
}
