using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDebuffIconController : MonoBehaviour
{
    [SerializeField] List<Image> Icons;

    public void AddNewIcon(Sprite icon)
    {
        foreach(Image iconImage in Icons)
        {
            if (!iconImage.enabled)
            {
                iconImage.enabled = true;
                iconImage.sprite = icon;
                iconImage.transform.SetAsFirstSibling();
            }
        }
    }

    public void RemoveIcon(Sprite icon) 
    {
        foreach (Image iconImage in Icons)
        {
            if (iconImage.enabled)
            {
                if(iconImage.sprite == icon)
                {
                    iconImage.enabled = false;
                    iconImage.sprite = null;
                    iconImage.transform.SetAsLastSibling();

                }
            }
        }
    }
}
