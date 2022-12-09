using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchWindow : MonoSingleton<ResearchWindow>
{
    [SerializeField] TweenAnimator tweenAnimator;
    [SerializeField] Image currentlyResearchingImage;
    [SerializeField] List<ResearchTree> trees;
    [SerializeField] TechTreeHolder defaultTechTree;
    [SerializeField] GameObject button;
    [SerializeField] Image characterIcon0;
    [SerializeField] Image characterIcon1;

    ResearchNode currentlyResearching;
    internal bool shouldOpenWindow = true;

    private void Start()
    {
        trees[0].SetupTree(defaultTechTree);
        trees[1].SetupTree(CharacterSelector.firstCharacter.techTree);
        trees[2].SetupTree(CharacterSelector.secondCharacter.techTree);

        characterIcon0.sprite = CharacterSelector.firstCharacter.icon;
        characterIcon1.sprite = CharacterSelector.secondCharacter.icon;
    }

    public void AdvanceResearch()
    {
        if (currentlyResearching != null)
        {
            float prevProgress = (float)currentlyResearching.currentProgress / currentlyResearching.research.timeToResearch;
            currentlyResearching.Advanced();
            ResearchButton.instance.UpdateFill(prevProgress, (float)currentlyResearching.currentProgress / currentlyResearching.research.timeToResearch, 1f);
            if(currentlyResearching.currentProgress >= currentlyResearching.research.timeToResearch)
            {
                button.SetActive(false);
                shouldOpenWindow =true;
            }
        }
    }

    public void NewResearchSelected(ResearchNode research)
    {
        if (currentlyResearching != research)
        {
            currentlyResearchingImage.transform.parent.gameObject.SetActive(true);
            ResearchButton.instance.UpdateFill(currentlyResearching == null ? 0f : ((float)currentlyResearching.currentProgress / currentlyResearching.research.timeToResearch),
                (float)research.currentProgress / research.research.timeToResearch, 0.5f);
            currentlyResearching = research;
            currentlyResearchingImage.sprite = research.research.sprite;
            button.SetActive(true);
        }
    }

    public void IfNoResearchSelectedOpen()
    {
        if (shouldOpenWindow)
        {
            Open();
        }
    }

    public void Open()
    {
        Cover.cover = true;
        tweenAnimator.PerformTween(1);
        shouldOpenWindow = false;
        foreach (ResearchTree tree in trees)
        {
            tree.PlayAnimations();
        }

        if(currentlyResearching == null)
        {
            currentlyResearchingImage.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            currentlyResearchingImage.transform.parent.gameObject.SetActive(true);

        }
    }

    public void Close()
    {
        Cover.cover = false;
        tweenAnimator.PerformTween(0);
        TurnController.ResearchSelected();

        if (currentlyResearching == null)
        {
            shouldOpenWindow = true;
        }
        else if (currentlyResearching.currentProgress >= currentlyResearching.research.timeToResearch)
        {
            shouldOpenWindow = true;
        }

    }
}
