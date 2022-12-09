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

        SetupConnections();
    }

    void SetupConnections()
    {
        for (int i = 0; i < tiers.Count; i++)
        {
            foreach (ResearchNode startNode in nodes)
            {
                if (startNode.research.tier == i)
                {
                    foreach (ResearchNode nextNode in nodes)
                    {
                        if (nextNode.research.tier == i + 1)
                        {
                            if (startNode.nextNodes == null)
                            {
                                startNode.nextNodes = new List<ResearchNode>();
                            }
                            startNode.nextNodes.Add(nextNode);
                        }
                        else if(nextNode.research.tier == i && startNode != nextNode)
                        {
                            startNode.sameLevelNodes = nextNode;
                        }
                    }
                }
            }
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
