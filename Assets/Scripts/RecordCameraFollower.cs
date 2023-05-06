using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordCameraFollower : MonoSingleton<RecordCameraFollower>
{
    [SerializeField] TweenAnimator animator;

    int currentStep;

    public void TileAdded(Vector2 placedTo)
    {
       // int maxCoordinate = Mathf.RoundToInt (Mathf.Max(Mathf.Abs(placedTo.x), Mathf.Abs(placedTo.y)));
       // if(maxCoordinate > currentStep)
       // {
            animator.PerformTween(currentStep);
        currentStep++;
        // }
    }
}
