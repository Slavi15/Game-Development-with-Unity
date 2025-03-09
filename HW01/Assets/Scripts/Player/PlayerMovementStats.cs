using UnityEngine;

[CreateAssetMenu(menuName = "Player Movement")]
public class PlayerMovementStats : ScriptableObject
{

    [Header("Walk")]
    [Range(1f, 100f)] 
    [SerializeField]
    private float _MaxWalkSpeed = 12.5f;
    public float MaxWalkSpeed => _MaxWalkSpeed;

    [Range(0.25f, 50f)]
    [SerializeField]
    private float _GroundAcceleration = 5f;
    public float GroundAcceleration => _GroundAcceleration;

    [Range(0.25f, 50f)]
    [SerializeField]
    private float _GroundDeceleration = 20f;
    public float GroundDeceleration => _GroundDeceleration;

    [Range(0.25f, 50f)]
    [SerializeField]
    private float _AirAcceleration = 5f;
    public float AirAcceleration => _AirAcceleration;

    [Range(0.25f, 50f)]
    [SerializeField]
    private float _AirDeceleration = 5f;
    public float AirDeceleration => _AirDeceleration;

    [Header("Collisions")]
    [SerializeField]
    private LayerMask _GroundLayer;
    public LayerMask GroundLayer => _GroundLayer;

    [SerializeField]
    private float _GroundDetectionLength = 0.02f;
    public float BottomGroundDetectionLength => _GroundDetectionLength;

    [SerializeField]
    private float _HeadDetectionLength = 0.02f;
    public float HeadDetectionLength => _HeadDetectionLength;

    [Range(0f, 1f)]
    [SerializeField]
    private float _HeadWidth = 0.75f;
    public float HeadWidth => _HeadWidth;

    [Header("Jump")]
    [SerializeField]
    private float _JumpHeight = 6.5f;
    public float JumpHeight => _JumpHeight;

    [Range(1f, 1.1f)]
    [SerializeField]
    private float _JumpFactor = 1.055f;
    public float JumpFactor => _JumpFactor;

    [SerializeField]
    private float _TimeTillApex = 0.35f;
    public float TimeTillApex => _TimeTillApex;

    [Range(0.01f, 5f)]
    [SerializeField]
    private float _GravityMult = 2f;
    public float GravityMult => _GravityMult;

    [SerializeField]
    private float _MaxFallSpeed = 26f;
    public float MaxFallSpeed => _MaxFallSpeed;

    [Range(1, 5)]
    [SerializeField]
    private int _JumpsAllowed = 2;
    public int JumpsAllowed => _JumpsAllowed;

    [Header("Jump Cut")]
    [Range(0.02f, 0.3f)]
    [SerializeField]
    private float _TimeUpwardCancel = 0.027f;
    public float TimeUpwardCancel => _TimeUpwardCancel;

    [Header("Jump Apex")]
    [Range(0.5f, 1f)]
    [SerializeField]
    private float _ApexThreshold = 0.97f;
    public float ApexThreshold => _ApexThreshold;

    [Range(0.01f, 1f)]
    [SerializeField]
    private float _ApexHangTime = 0.075f;
    public float ApexHangTime => _ApexHangTime;

    [Header("Jump Buffer")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _JumpBufferTime = 0.125f;
    public float JumpBufferTime => _JumpBufferTime;

    [Header("Jump Coyote Time")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _JumpCoyoteTime = 0.1f;
    public float JumpCoyoteTime => _JumpCoyoteTime;

    public float Gravity { get; private set; }
    public float InitialJumpVelocity { get; private set; }
    public float AdjustedJumpHeight { get; private set; }

    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }

    private void CalculateValues()
    {
        AdjustedJumpHeight = JumpHeight * JumpFactor;
        Gravity = - (2f * AdjustedJumpHeight) / Mathf.Pow(TimeTillApex, 2f);
        InitialJumpVelocity = Mathf.Abs(Gravity) * TimeTillApex;
    }
}
