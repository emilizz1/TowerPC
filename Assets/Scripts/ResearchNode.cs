using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class ResearchNode : MonoBehaviour
{
    public Research research;
    [SerializeField] Image icon;
    [SerializeField] float timeToReveal;
    [SerializeField] TweenAnimator coverAnimator;
    [SerializeField] TweenAnimator sizeAnimator;

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
    }

    private void OnMouseEnter()
    {
        if(unlocked && !researched)
        {
            transform.localScale = new Vector3(0.85f, 0.85f, 1f);
        }
    }

    private void OnMouseExit()
    {
        if (unlocked && !researched)
        {
            transform.localScale = new Vector3(0.75f, 0.75f, 1f);

        }
    }
}
