using UnityEngine;

public class Jump : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private PlayerMovementStats MoveStats;
    [SerializeField] private Collider2D _feetCollider;
    [SerializeField] private Collider2D _bodyCollider;

    private Rigidbody2D _rb;

    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _bumpHead;

    public float VerticalVelocity { get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _usedJumps;

    private float _apexPoint;
    private float _timeAfterThreshold;
    private bool _isAfterThreshold;

    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;
    private float _coyoteTimer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CountTimers();
        JumpChecks();
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        JumpLogic();
    }

    private void JumpLogic()
    {
        if (_isJumping)
        {
            HandleJumpPhysics();
        }

        if (_isFastFalling)
        {
            HandleFastFall();
        }

        if (!_isGrounded && !_isJumping)
        {
            _isFalling = true;
            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
        }

        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 50f);
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, VerticalVelocity);
    }

    private void HandleJumpPhysics()
    {
        if (_bumpHead)
        {
            _isFastFalling = true;
        }

        if (VerticalVelocity >= 0f)
        {
            _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

            if (_apexPoint > MoveStats.ApexThreshold)
            {
                HandleApexPoint();
            }
            else
            {
                VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
            }
        }
        else if (!_isFastFalling)
        {
            VerticalVelocity += MoveStats.Gravity * MoveStats.GravityMult * Time.fixedDeltaTime;
        }
    }

    private void HandleApexPoint()
    {
        if (!_isAfterThreshold)
        {
            _isAfterThreshold = true;
            _timeAfterThreshold = 0f;
        }

        _timeAfterThreshold += Time.fixedDeltaTime;
        VerticalVelocity = _timeAfterThreshold < MoveStats.ApexHangTime ? 0f : -0.01f;
    }

    private void HandleFastFall()
    {
        if (_fastFallTime >= MoveStats.TimeUpwardCancel)
        {
            VerticalVelocity += MoveStats.Gravity * MoveStats.GravityMult * Time.fixedDeltaTime;
        }
        else
        {
            VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, _fastFallTime / MoveStats.TimeUpwardCancel);
        }

        _fastFallTime += Time.fixedDeltaTime;
    }

    private void CollisionChecks()
    {
        IsGrounded();
    }

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetCollider.bounds.center.x, _feetCollider.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetCollider.bounds.size.x, MoveStats.BottomGroundDetectionLength);

        _groundHit = Physics2D.BoxCast(
            boxCastOrigin, 
            boxCastSize, 
            0f, 
            Vector2.down, 
            MoveStats.BottomGroundDetectionLength, 
            MoveStats.GroundLayer
            );

        _isGrounded = _groundHit.collider != null;
    }

    private void JumpChecks()
    {
        if (InputManager.JumpWasPressed)
        {
            _jumpBufferTimer = MoveStats.JumpBufferTime;
            _jumpReleasedDuringBuffer = false;
        }

        if (InputManager.JumpWasReleased)
        {
            HandleJumpRelease();
        }

        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f || _usedJumps < MoveStats.JumpsAllowed))
        {
            InitiateJump(1);

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed  = VerticalVelocity;
            }
        }
        else if (_jumpBufferTimer > 0f && _isJumping && _usedJumps < MoveStats.JumpsAllowed)
        {
            _isFastFalling = false;
            InitiateJump(1);
        }
        else if (_jumpBufferTimer > 0f && _isFalling && _usedJumps < MoveStats.JumpsAllowed - 1)
        {
            InitiateJump(2);
            _isFastFalling = false;
        }

        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
        {
            ResetJumpFlags();
        }
    }

    private void HandleJumpRelease()
    {
        if (_jumpBufferTimer > 0f)
        {
            _jumpReleasedDuringBuffer = true;
        }

        if (_isJumping && VerticalVelocity > 0f)
        {
            _isFastFalling = true;
            _fastFallReleaseSpeed = VerticalVelocity;
        }
    }

    private void InitiateJump(int numberOfJumps)
    {
        if (_usedJumps >= MoveStats.JumpsAllowed) return;

        if (!_isJumping)
        {
            _isJumping = true;
        }

        _jumpBufferTimer = 0f;
        _usedJumps += numberOfJumps;
        _isGrounded = false;
        VerticalVelocity = MoveStats.InitialJumpVelocity;
    }

    private void CountTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;
        _coyoteTimer = _isGrounded ? _coyoteTimer - Time.deltaTime : MoveStats.JumpCoyoteTime;
    }

    private void ResetJumpFlags()
    {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isAfterThreshold = false;
            _usedJumps = 0;
            VerticalVelocity = Physics2D.gravity.y;
    }
}
