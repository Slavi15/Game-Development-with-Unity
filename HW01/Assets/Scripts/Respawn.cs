using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.position = respawnPoint.position;
        }
    }
}
