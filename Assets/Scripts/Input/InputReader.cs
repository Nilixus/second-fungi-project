using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _actions;
        
        private InputAction _moveAction;
        private InputAction _fireAction;
        private InputAction _jumpAction;
        
        public static Vector2 MoveDirection { get; private set; }

        public static event Action OnFire;
        public static event Action OnJump;
        
        private void Awake()
        {
            _moveAction = _actions.FindActionMap("Player")["Move"];
            _fireAction = _actions.FindActionMap("Player")["Fire"];
            _jumpAction = _actions.FindActionMap("Player")["Jump"];
        }

        private void OnEnable()
        {
            _moveAction.performed += OnMovePerformed;
            _moveAction.canceled  += OnMoveCanceled;
            _fireAction.performed += _ => OnFire?.Invoke();
            _jumpAction.performed += _ => OnJump?.Invoke();
        }
        
        private void OnMovePerformed(InputAction.CallbackContext ctx) => MoveDirection = ctx.ReadValue<Vector2>();
        private void OnMoveCanceled(InputAction.CallbackContext ctx) => MoveDirection = Vector2.zero;
        
        
        private void OnDisable()
        {
            _moveAction.performed -= OnMovePerformed;
            _moveAction.canceled  -= OnMoveCanceled;
        }
    }
}