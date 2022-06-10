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
    [SerializeField] Animator animator;

    public bool unlocked;
    internal bool researched;

    internal int currentProgress;

    private void Start()
    {
        if (research != null)
        {
            icon.sprite = research.sprite;
            explanation.text = research.explanation;
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
        animator.SetTrigger("Unlocked");
    }

    public void StartResearch()
    {
        if (unlocked && !researched)
        {
            ResearchWindow.instance.NewResearchSelected(this);
        }
    }
}
