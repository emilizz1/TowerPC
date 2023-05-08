using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.Loc;

public class ResearchWindow : MonoSingleton<ResearchWindow>
{
    [SerializeField] TweenAnimator tweenAnimator;
    [SerializeField] List<ResearchTree> trees;
    [SerializeField] TechTreeHolder defaultTechTree;
    [SerializeField] GameObject button;
    [SerializeField] Image characterIcon0;
    [SerializeField] Image characterIcon1;

    [Header("Research Info")]
    [SerializeField] TextMeshProUGUI researchName;
    [SerializeField] TextMeshProUGUI wavesNeeded;
    [SerializeField] TextMeshProUGUI explanation;
    [SerializeField] CardDisplay researchDisplay;
    [SerializeField] Image icon;
    [SerializeField] LocalizedString wavesToCompleteText;
    [SerializeField] LocalizedString effectText;

    internal  ResearchNode currentlyResearching;
    internal bool shouldOpenWindow = true;
    internal bool skipNextResearchResult;
    bool opened;

    private void Start()
    {
        int baseResearchLocked = ProgressManager.GetLevel("Base") >= 8 ? 0 : ProgressManager.GetLevel("Base") >= 5? 1:2;
        int firstResearchLocked = ProgressManager.GetLevel(CharacterSelector.firstCharacter.characterName) >= 7 ? 0 : ProgressManager.GetLevel(CharacterSelector.firstCharacter.characterName) >= 3? 1:2;
        int secondResearchLocked = ProgressManager.GetLevel(CharacterSelector.secondCharacter.characterName) >= 7 ? 0 : ProgressManager.GetLevel(CharacterSelector.secondCharacter.characterName) >= 3? 1:2;
        trees[0].SetupTree(CharacterSelector.firstCharacter.techTree, firstResearchLocked);
        trees[1].SetupTree(CharacterSelector.secondCharacter.techTree, secondResearchLocked);
        trees[2].SetupTree(defaultTechTree, baseResearchLocked);

        characterIcon0.sprite = CharacterSelector.firstCharacter.icon;
        characterIcon1.sprite = CharacterSelector.secondCharacter.icon;
    }

    public void AdvanceResearch()
    {

        if (currentlyResearching != null)
        {
            if (currentlyResearching.researched)
            {
                return;
            }
            float prevProgress = (float)currentlyResearching.currentProgress / currentlyResearching.research.timeToResearch;
            currentlyResearching.Advanced();
            ResearchButton.instance.UpdateFill(prevProgress, (float)currentlyResearching.currentProgress / currentlyResearching.research.timeToResearch, 1f);
            if (currentlyResearching.currentProgress >= currentlyResearching.research.timeToResearch)
            {
                if (IsThereResearchToComplete())
                {
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
            if (currentlyResearching != null)
            {
                currentlyResearching.Deselected();
            }
            currentlyResearching = research;
            ResearchButton.instance.UpdateFill(currentlyResearching == null ? 0f : ((float)currentlyResearching.currentProgress / currentlyResearching.research.timeToResearch),
                (float)research.currentProgress / research.research.timeToResearch, 0.5f);

            shouldOpenWindow = false;
            DisplayResearch(currentlyResearching);

            if (research.unlocked && !research.researched)
            {
                button.SetActive(true);
            }
            else
            {
                button.SetActive(false);
            }            
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
        DisplayResearch(currentlyResearching);
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

    public void DisplayResearch(ResearchNode researchToDisplay)
    {
        if(researchToDisplay == null)
        {
            return;
        }
        researchName.text = researchToDisplay.research.researchName;
        wavesNeeded.text = wavesToCompleteText + " " + (researchToDisplay.research.timeToResearch - researchToDisplay.currentProgress);
        explanation.text = effectText + " " + researchToDisplay.research.explanation;
        icon.sprite = researchToDisplay.research.sprite;
        if (researchToDisplay.cardResearch)
        {
            ResearchGetCard researchCard = (ResearchGetCard)researchToDisplay.research;
            researchDisplay.DisplayCard(researchCard.cardToGet);
            researchDisplay.gameObject.SetActive(true);
        }
        else
        {
            researchDisplay.gameObject.SetActive(false);
        }
    }
}
