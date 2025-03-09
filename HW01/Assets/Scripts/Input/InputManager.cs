using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static PlayerInput PlayerInput { get; private set; }

    public static Vector2 Movement { get; private set; }
    public static bool JumpWasPressed { get; private set; }
    public static bool JumpIsHeld { get; private set; }
    public static bool JumpWasReleased { get; private set; }

    [SerializeField]
    private InputAction _moveAction;

    [SerializeField]
    private InputAction _jumpAction;

    void Start()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Move"];
        _jumpAction = PlayerInput.actions["Jump"];
    }

    void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();

        JumpWasPressed = _jumpAction.WasPressedThisFrame();
        JumpIsHeld = _jumpAction.IsPressed();
        JumpWasReleased = _jumpAction.WasReleasedThisFrame();
    }
}
