using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDebuffIconController : MonoBehaviour
{
    [SerializeField] List<Image> Icons;

    public void AddNewIcon(Sprite icon)
    {
        foreach (Image iconImage in Icons)
        {
            if (!iconImage.enabled)
            {
                iconImage.enabled = true;
                iconImage.sprite = icon;
                iconImage.transform.SetAsFirstSibling();
                return;
            }
        }
    }

    public void RemoveIcon(Sprite icon)
    {
        foreach (Image iconImage in Icons)
        {
                if (iconImage.sprite == icon)
                {
                    iconImage.enabled = false;
                    iconImage.sprite = null;
                    iconImage.transform.SetAsLastSibling();
                    return;

                }
        }
    }

    public void ResetIcons()
    {
        int index = 0;
        foreach (Image iconImage in Icons)
        {
            Debug.Log("Reset " + index++);
            iconImage.enabled = false;
            iconImage.sprite = null;
        }
    }
}
