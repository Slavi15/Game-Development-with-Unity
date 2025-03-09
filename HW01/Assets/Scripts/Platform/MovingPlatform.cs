using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    
    [SerializeField] private List<Transform> points;
    [SerializeField] private Transform platform;
    [SerializeField] private int goalPoint = 0;
    [SerializeField] private float speed = 1.5f;

    void Update()
    {
        MoveToNextPoint();
    }

    private void MoveToNextPoint()
    {
        platform.position = Vector2.MoveTowards(platform.position, points[goalPoint].position, Time.fixedDeltaTime * speed);

        if (Vector2.Distance(platform.position, points[goalPoint].position) < 0.1f)
        {
            goalPoint = ++goalPoint % points.Count;
        }
    }
}
