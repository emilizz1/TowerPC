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
    [SerializeField] Image progress;
    [SerializeField] TextMeshProUGUI explanation;
    [SerializeField] TextMeshProUGUI timeToComplete;
    [SerializeField] Animator animator;

    internal List<ResearchNode> nextNodes = new List<ResearchNode>();
    internal ResearchNode sameLevelNodes;

    bool exited;
    bool unlocked = false;
    internal bool researched;

    bool playedCompletedAnimation;
    bool playedUnlockAnimation;

    internal int currentProgress;

    public void Setup()
    {
        if (research != null)
        {
            icon.sprite = research.sprite;
            explanation.text = research.explanation;
            timeToComplete.text = "Time to complete: <b>" + research.timeToResearch.ToString() + "</b> turns";
            if (research.tier == 0)
            {
                Unlocked();
            }
        }
    }

    public void Advanced()
    {
        currentProgress++;
        progress.fillAmount = (float)currentProgress / research.timeToResearch;
        if (currentProgress >= research.timeToResearch)
        {
            Researched();
        }
    }

    public void Researched()
    {
        researched = true;
        research.Researched();
        if (sameLevelNodes != null)
        {
            sameLevelNodes.Locked();
        }
        foreach (ResearchNode node in nextNodes)
        {
            node.Unlocked();
        }
    }

    public void Unlocked()
    {
        unlocked = true;
        animator.ResetTrigger("Hide");
    }

    public void Locked()
    {
        unlocked = false;
        animator.SetTrigger("Hide");

    }

    public void StartResearch()
    {
        if (unlocked && !researched)
        {
            ResearchWindow.instance.NewResearchSelected(this);
        }
    }

    public void PlayAnimations()
    {
        if(!playedCompletedAnimation && researched)
        {
            playedCompletedAnimation = true;
            animator.SetTrigger("Completed");
        }

        if (!playedUnlockAnimation && unlocked)
        {
            playedUnlockAnimation = true;
            animator.SetTrigger("Unlocked");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!unlocked)
        {
            exited = false;
            StartCoroutine(RevealNotUnlocked());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!unlocked)
        {
            exited = true;
            animator.SetTrigger("Hide");
        }
    }

    IEnumerator RevealNotUnlocked()
    {
        yield return new WaitForSeconds(0.5f);
        if (!exited)
        {
            animator.SetTrigger("Unlocked");
            animator.ResetTrigger("Hide");
        }
    }
}
