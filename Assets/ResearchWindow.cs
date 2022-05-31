using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchWindow : MonoSingleton<ResearchWindow>
{
    [SerializeField] TweenAnimator tweenAnimator;
    [SerializeField] Image currentlyResearchingImage;

    ResearchNode currentlyResearching;

    public void AdvanceResearch()
    {
        if(currentlyResearching != null)
        {
            currentlyResearching.Advanced();
        }
    }

    public void NewResearchSelected(ResearchNode research)
    {
        currentlyResearching = research;
        currentlyResearchingImage.sprite = research.research.sprite;
    }

    public void Open()
    {
        tweenAnimator.PerformTween(1);
    }

    public void Close()
    {
        tweenAnimator.PerformTween(0);
    }
}
