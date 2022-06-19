using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchWindow : MonoSingleton<ResearchWindow>
{
    [SerializeField] TweenAnimator tweenAnimator;
    [SerializeField] Image currentlyResearchingImage;
    [SerializeField] List<ResearchTree> trees;

    ResearchNode currentlyResearching;

    public void AdvanceResearch()
    {
        if (currentlyResearching != null)
        {
            float prevProgress = (float)currentlyResearching.currentProgress / currentlyResearching.research.timeToResearch;
            currentlyResearching.Advanced();
            ResearchButton.instance.UpdateFill(prevProgress, (float)currentlyResearching.currentProgress / currentlyResearching.research.timeToResearch, 1f);
        }
    }

    public void NewResearchSelected(ResearchNode research)
    {
        if (currentlyResearching != research)
        {
            ResearchButton.instance.UpdateFill(currentlyResearching == null ? 0f : ((float)currentlyResearching.currentProgress / currentlyResearching.research.timeToResearch),
                (float)research.currentProgress / research.research.timeToResearch, 0.5f);
            currentlyResearching = research;
            currentlyResearchingImage.sprite = research.research.sprite;
        }
    }

    public void Open()
    {
        tweenAnimator.PerformTween(1);
        foreach(ResearchTree tree in trees)
        {
            tree.PlayAnimations();
        }
    }

    public void Close()
    {
        tweenAnimator.PerformTween(0);
    }
}
