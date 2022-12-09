using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCardWindow : MonoSingleton<DestroyCardWindow>
{
    [SerializeField] TweenAnimator animator;

    

    public void Open()
    {
        StartCoroutine(OpenAfterTime(2f));
    }

    IEnumerator OpenAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Cover.cover = true;
        animator.PerformTween(1);
        DestroyCardManager.instance.DisplayNewChoices();

    }

    public IEnumerator CloseAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Cover.cover = false;
        DestroyCardManager.instance.CloseWindow();
        yield return new WaitForSeconds(time);
        animator.PerformTween(0);
        yield return new WaitForSeconds(time);
        TurnController.FinishedDestroying();
    }
}
