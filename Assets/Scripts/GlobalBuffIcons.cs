using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I2.Loc;

public class GlobalBuffIcons : MonoSingleton<GlobalBuffIcons>
{
    [SerializeField] List<GlobalBuffIconDisplay> globalBuffIconDisplays;
    [SerializeField] Sprite knightSprite;
    [SerializeField] LocalizedString knightText;
    [SerializeField] Sprite mageSprite;
    [SerializeField] LocalizedString mageText;
    [SerializeField] Sprite admiralSprite;
    [SerializeField] LocalizedString admiralText;
    [SerializeField] Sprite hunterSprite;
    [SerializeField] LocalizedString hunterText;

    int displayedCount;

    private void Start()
    {
        if (CharacterSelector.firstCharacter.characterName == "Knight" || CharacterSelector.secondCharacter.characterName == "Knight")
        {
            DisplayBuff(knightSprite, knightText);
        }
        if (CharacterSelector.firstCharacter.characterName == "Mage" || CharacterSelector.secondCharacter.characterName == "Mage")
        {
            DisplayBuff(mageSprite, mageText);
        }
        if (CharacterSelector.firstCharacter.characterName == "Admiral" || CharacterSelector.secondCharacter.characterName == "Admiral")
        {
            DisplayBuff(admiralSprite, admiralText);
        }
        if (CharacterSelector.firstCharacter.characterName == "Hunter" || CharacterSelector.secondCharacter.characterName == "Hunter")
        {
            DisplayBuff(hunterSprite, hunterText);
        }
    }

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
