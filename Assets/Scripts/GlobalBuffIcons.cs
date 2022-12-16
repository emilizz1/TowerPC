using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBuffIcons : MonoSingleton<GlobalBuffIcons>
{
    [SerializeField] List<GlobalBuffIconDisplay> globalBuffIconDisplays;

    int displayedCount;

    public void DisplayBuff(Sprite icon, string text)
    {
        if(displayedCount == globalBuffIconDisplays.Count)
        {
            return;
        }

        globalBuffIconDisplays[displayedCount].gameObject.SetActive(true);
        globalBuffIconDisplays[displayedCount].Display(icon,text);
        displayedCount++;
    }
}
