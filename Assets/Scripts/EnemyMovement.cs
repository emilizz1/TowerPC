using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    internal float movementSpeed;
    internal List<Vector3> movementPath;

    internal Enemy enemy;
    internal float additionalSpeed;

    int moveIndex = 0;
    float distanceToNextPath;

    void Update()
    {
        if (enemy.returning)
        {
            return;
        }

        if (moveIndex != movementPath.Count)
        {
            float moveDistance = (movementSpeed + additionalSpeed) * Time.deltaTime;
            while (moveDistance > 0.0001f)
            {
                distanceToNextPath = Vector3.Distance(transform.position, movementPath[moveIndex]);
                transform.position = Vector3.MoveTowards(transform.position, movementPath[moveIndex], moveDistance);
                if (distanceToNextPath < moveDistance)
                {
                    moveIndex++;
                    transform.LookAt(movementPath[moveIndex], Vector3.up);
                    if (moveIndex + 1 == movementPath.Count)
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

    public void ResetEnemy()
    {
        moveIndex = 0;
        movementPath = new List<Vector3>();
        additionalSpeed = 0f;
    }

    public float GetPathRemaining()
    {
        return distanceToNextPath + (movementPath.Count - moveIndex) * 100f;
    }
}
