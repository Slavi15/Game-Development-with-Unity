using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private PlayerMovementStats MoveStats;

    private Rigidbody2D _rb;
    private Vector2 _moveVelocity;

    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float acceleration = _isGrounded ? MoveStats.GroundAcceleration : MoveStats.AirAcceleration;
        float deceleration = _isGrounded ? MoveStats.GroundDeceleration : MoveStats.AirDeceleration;

        Move(acceleration, deceleration, InputManager.Movement);
    }

    private void Move(float acc, float dcc, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            Vector2 targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed;
            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acc * Time.fixedDeltaTime);
        }
        else
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, dcc * Time.fixedDeltaTime);
        }

        _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
    }

}