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
    [SerializeField] float timeToReveal;
    [SerializeField] GameObject selected;
    [SerializeField] CardDisplay cardDisplay;

    internal List<ResearchNode> nextNodes = new List<ResearchNode>();
    internal ResearchNode sameLevelNodes;

    internal bool unlocked = false;
    internal bool researched;

    bool exited;
    bool cardResearch;
    bool playedCompletedAnimation;
    bool playedUnlockAnimation;

    internal int currentProgress;

    public void Setup()
    {
        if (research != null)
        {
            icon.sprite = research.sprite;
            explanation.text = research.explanation;
            timeToComplete.text =  research.timeToResearch.ToString();
            if (research.researchType == Research.ResearchType.Card)
            {
                ResearchGetCard researchCard = (ResearchGetCard)research;
                cardDisplay.DisplayCard(researchCard.cardToGet);
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
        progress.fillAmount = (float)currentProgress / (float)research.timeToResearch;
        if (currentProgress >= research.timeToResearch)
        {
            Researched();
        }
    }

    public void Researched()
    {
        selected.SetActive(false);
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
            selected.SetActive(true);
            SoundsController.instance.PlayOneShot("Click");
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
        if (cardResearch)
        {
            exited = false;
            StartCoroutine(DisplayCard());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!unlocked)
        {
            exited = true;
            animator.SetTrigger("Hide");
        }
        if (cardResearch)
        {
            exited = true;
            cardDisplay.gameObject.SetActive(false);
        }
    }

    IEnumerator RevealNotUnlocked()
    {
        yield return new WaitForSeconds(timeToReveal);
        if (!exited)
        {
            animator.SetTrigger("Unlocked");
            animator.ResetTrigger("Hide");
        }
    }



    IEnumerator DisplayCard()
    {
        yield return new WaitForSeconds(timeToReveal);
        if (!unlocked) 
        {
            yield return new WaitForSeconds(timeToReveal);
        }
        if (!exited)
        {
            cardDisplay.gameObject.SetActive(true);
        }
    }

    public void PlayCompleteSound()
    {

        SoundsController.instance.PlayOneShot("Complete");
    }

    public void Deselected()
    {
        selected.SetActive(false);
        cardDisplay.gameObject.SetActive(false);
    }
}
