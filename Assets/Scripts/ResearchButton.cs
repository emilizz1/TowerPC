using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchButton : MonoSingleton<ResearchButton>
{
    [SerializeField] Image fill;
    [SerializeField] GameObject researchWindow;
    [SerializeField] TweenAnimator animator;

    bool shouldPlayAnimation;

    private void Start()
    {
        NoResearchSelected();
    }

    public void NoResearchSelected()
    {
        shouldPlayAnimation = true;
        PlayFirstAnimation();
    }

    public void ResearchSelected()
    {
        shouldPlayAnimation = false;
    }

    public void UpdateFill(float prevValue, float newValue, float time)
    {
        LeanTween.value(gameObject, prevValue, newValue, time).setOnUpdate((value) => fill.fillAmount = value);
    }

    public void PlayFirstAnimation()
    {
        if (shouldPlayAnimation)
        {
            animator.PerformTween(0);
        }
    }

    public void PlaySecondAnimation()
    {
        animator.PerformTween(1);
    }
}
