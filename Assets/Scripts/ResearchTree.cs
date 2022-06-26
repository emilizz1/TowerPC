using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchTree : MonoBehaviour
{
    [SerializeField] List<Transform> tiers;
    [SerializeField] GameObject nodePrefab;

    List<ResearchNode> nodes;

    public void SetupTree(TechTreeHolder techTree)
    {
        nodes = new List<ResearchNode>();
        foreach (Research research in techTree.researches)
        {
            ResearchNode newNode = Instantiate(nodePrefab, tiers[research.tier]).GetComponent<ResearchNode>();
            nodes.Add(newNode);
            newNode.research = research;
            newNode.Setup();
        }
    }

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
