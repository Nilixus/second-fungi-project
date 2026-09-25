using Combat;
using Input;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("PlayerMovement")]
    [SerializeField] private float _speed = 5f;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private int _jumpCount = 2;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundMask;

    private Rigidbody2D _rb;
    private Weapon _weapon;
    private Vector2 _movement;



    private bool _jumpRequested;
    private int _jumpRemaining;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _weapon = GetComponent<Weapon>();
        _jumpRemaining = _jumpCount;
    }

    private void OnEnable()
    {
        InputReader.OnFire += HandleFire;
        InputReader.OnJump += Jump;
    }

    private void OnDisable()
    {
        InputReader.OnFire -= HandleFire;
        InputReader.OnJump -= Jump;
    }
    
    private void FixedUpdate()
    {
        Move();
    }
    
    private bool IsGrounded()
    {
        /*return Physics2D.OverlapCircle(
            _groundCheck.position,
            _groundCheckRadius,
            _groundMask
        );*/

        RaycastHit2D hit = Physics2D.Raycast(
            origin: _groundCheck.position,
            direction: Vector2.down,
            _groundCheckRadius,
            _groundMask
        );
        return hit.collider;
    }
    
    private void Move()
    {
        Vector2 direction = InputReader.MoveDirection;
        if(direction.sqrMagnitude > 1)
            direction.Normalize();

        _rb.linearVelocity = new Vector2(
            direction.x * _speed,
            _rb.linearVelocity.y
        );
    }
    
    
    
    private void Jump()
    {
        if (IsGrounded())
            _jumpRemaining = _jumpCount;

        if(_jumpRemaining <= 0)
            return;
        
        Vector2 velocity = _rb.linearVelocity;
        velocity.y = _jumpForce;
        _rb.linearVelocity = velocity;
        _jumpRemaining--;
    }
    
    private void HandleFire()
    { 
        _weapon.TryFire();
    }
    
}