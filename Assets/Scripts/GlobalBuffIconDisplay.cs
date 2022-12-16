using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GlobalBuffIconDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject parent;
    
    public void Display(Sprite icon, string explanation)
    {
        image.sprite = icon;
        text.text = explanation;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        parent.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        parent.SetActive(false);
    }
}
