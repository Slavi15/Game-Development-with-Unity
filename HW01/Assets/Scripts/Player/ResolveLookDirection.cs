using UnityEngine;

public class ResolveLookDirection : MonoBehaviour
{

    private bool _isFacingRight;

    void Awake()
    {
        _isFacingRight = true;
    }

    void FixedUpdate()
    {
        Vector2 moveInput = InputManager.Movement;

        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x < 0) Turn(false);
        else if (!_isFacingRight && moveInput.x > 0) Turn(true);
    }

    private void Turn(bool turnRight)
    {
        _isFacingRight = turnRight;
        transform.Rotate(0f, turnRight ? 180f : -180f, 0f);
    }
}
