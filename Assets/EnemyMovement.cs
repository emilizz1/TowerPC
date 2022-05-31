using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    internal float movementSpeed;
    internal List<Vector3> movementPath;

    internal Enemy enemy;

    int moveIndex = 0;

    void Update()
    {
        if (moveIndex != movementPath.Count)
        {
            float moveDistance = movementSpeed * Time.deltaTime;
            while (moveDistance > 0.0001f)
            {
                float distanceToNextPath = Vector3.Distance(transform.position, movementPath[moveIndex]);
                transform.position = Vector3.MoveTowards(transform.position, movementPath[moveIndex], moveDistance);
                if (distanceToNextPath < moveDistance)
                {
                    moveIndex++;
                    if (moveIndex == movementPath.Count)
                    {
                        enemy.ReachedEnd();
                        return;
                    }
                    moveDistance -= distanceToNextPath;
                }
                else
                {
                    moveDistance = 0;
                }
            }
        }
    }
}
