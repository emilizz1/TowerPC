using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResearchNode : MonoBehaviour
{
    public Research research;
    [SerializeField] List<ResearchNode> nextNodes;
    [SerializeField] Image icon;
    [SerializeField] Image progress;
    [SerializeField] TextMeshProUGUI explanation;
    [SerializeField] TextMeshProUGUI timeToComplete;
    [SerializeField] Animator animator;

    public bool unlocked;
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
            if (unlocked)
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
        foreach (ResearchNode node in nextNodes)
        {
            node.Unlocked();
        }
    }

    public void Unlocked()
    {
        unlocked = true;    
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
}
