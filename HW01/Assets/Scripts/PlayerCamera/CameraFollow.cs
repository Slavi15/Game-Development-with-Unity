using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;

    [Range(1, 10)]
    [SerializeField]
    private float smoothFactor = 1.5f;

    [SerializeField] private Vector3 minValues;
    [SerializeField] private Vector3 maxValues;

    void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 targetPosition = player.position + offset;

        Vector3 boundPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minValues.x, maxValues.x),
            Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y),
            Mathf.Clamp(targetPosition.z, minValues.z, maxValues.z)
        );

        Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
