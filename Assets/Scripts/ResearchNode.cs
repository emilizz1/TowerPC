using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class ResearchNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Research research;
    [SerializeField] Image icon;
    [SerializeField] float timeToReveal;
    [SerializeField] TweenAnimator coverAnimator;
    [SerializeField] TweenAnimator sizeAnimator;
    public GameObject researchingIcon;

    internal List<ResearchNode> nextNodes = new List<ResearchNode>();
    internal ResearchNode sameLevelNodes;

    internal bool unlocked = false;
    internal bool researched;

    internal bool cardResearch;
    bool playedCompletedAnimation;
    bool playedUnlockAnimation;

    internal int currentProgress;

    public void Setup()
    {
        if (research != null)
        {
            research.Initialize();
            icon.sprite = research.sprite;
            researchingIcon.SetActive(false);

            if (research.researchType == Research.ResearchType.Card)
            {
                cardResearch = true;
            }
            if (research.tier == 0)
            {
                Unlocked();
            }
        }
    }

    public void Advanced()
    {
        currentProgress++;
        if (currentProgress >= research.timeToResearch)
        {
            Researched();
        }
    }

    public void Researched()
    {
        researched = true;
        if (ResearchWindow.instance.skipNextResearchResult)
        {
            ResearchWindow.instance.skipNextResearchResult = false;
        }
        else
        {
            research.Researched();
        }
        if (sameLevelNodes != null)
        {
            sameLevelNodes.Locked();
        }
        foreach (ResearchNode node in nextNodes)
        {
            node.Unlocked();
        }
        if (GlobalConditionHolder.coinsFromResearch)
        {
            Money.instance.AddCurrency(25, false);
        }
    }

    public void Unlocked()
    {
        unlocked = true;
        //coverAnimator.PerformTween(1);
        //sizeAnimator.PerformTween(1);
    }

    public void Locked()
    {
        unlocked = false;

    }

    public void StartResearch()
    {
        researchingIcon.SetActive(true);
        SoundsController.instance.PlayOneShot("Click");
        ResearchWindow.instance.NewResearchSelected(this);
    }

    public void PlayAnimations()
    {
        if(!playedCompletedAnimation && researched)
        {
            sizeAnimator.PerformTween(0);
            playedCompletedAnimation = true;
        }

        if (!playedUnlockAnimation && unlocked)
        {
            coverAnimator.PerformTween(1);
            sizeAnimator.PerformTween(1);
               playedUnlockAnimation = true;
        }

        if(playedUnlockAnimation && !unlocked)
        {
            coverAnimator.PerformTween(0);
            sizeAnimator.PerformTween(0);
        }
    }

    public void PlayCompleteSound()
    {
        SoundsController.instance.PlayOneShot("Complete");
    }

    public void Deselected()
    {
        researchingIcon.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ResearchWindow.instance.DisplayResearch(this);
        if (unlocked && !researched)
        {
            transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(ResearchWindow.instance.currentlyResearching != null)
        {
            ResearchWindow.instance.DisplayResearch(ResearchWindow.instance.currentlyResearching);

        }
        if (unlocked && !researched)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);

        }
    }
}
