using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchTree : MonoBehaviour
{
    [SerializeField] List<ResearchNode> nodes;

    public void PlayAnimations()
    {
        StartCoroutine(PlayAnimationsAfterDelay(0.75f));
    }

    IEnumerator PlayAnimationsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (ResearchNode node in nodes)
        {
            node.PlayAnimations();
        }
    }
}
