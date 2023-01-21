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
    bool opened;

    private void Start()
    {
        int baseResearchLocked = ProgressManager.GetLevel("Base") >= 11 ? 0 : ProgressManager.GetLevel("Base") >= 5? 1:2;
        int firstResearchLocked = ProgressManager.GetLevel(CharacterSelector.firstCharacter.characterName) >= 7 ? 0 : ProgressManager.GetLevel(CharacterSelector.firstCharacter.characterName) >= 3? 1:2;
        int secondResearchLocked = ProgressManager.GetLevel(CharacterSelector.secondCharacter.characterName) >= 7 ? 0 : ProgressManager.GetLevel(CharacterSelector.secondCharacter.characterName) >= 3? 1:2;
        trees[0].SetupTree(defaultTechTree, baseResearchLocked);
        trees[1].SetupTree(CharacterSelector.firstCharacter.techTree, firstResearchLocked);
        trees[2].SetupTree(CharacterSelector.secondCharacter.techTree, secondResearchLocked);

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
                if (IsThereResearchToComplete())
                {
                    currentlyResearchingImage.transform.parent.gameObject.SetActive(false);
                    button.SetActive(false);
                    shouldOpenWindow = true;
                }
            }
            else
            {
                shouldOpenWindow = false;
            }
        }
    }

    private bool IsThereResearchToComplete()
    {
        foreach (ResearchTree researchTree in trees)
        {
            if (researchTree.HasResearchToComplete())
            {
                return true;
            }
        }

        return false;
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
            shouldOpenWindow = false;
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
        if (opened)
        {
            return;
        }
        opened = true;
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
        if (!opened)
        {
            return;
        }
        opened = false;
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
