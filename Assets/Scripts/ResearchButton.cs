using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchButton : MonoSingleton<ResearchButton>
{
    [SerializeField] Image fill;
    [SerializeField] GameObject researchWindow;
    [SerializeField] TweenAnimator animator;

    public void UpdateFill(float prevValue, float newValue, float time)
    {
        LeanTween.value(gameObject, prevValue, newValue, time).setOnUpdate((value) => fill.fillAmount = value);
    }
}
