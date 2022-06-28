using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionChecker : MonoBehaviour
{
    [SerializeField] Character first;
    [SerializeField] Character second;

    private void Awake()
    {
        if(CharacterSelector.firstCharacter == null)
        {
            CharacterSelector.firstCharacter = first;
        }

        if (CharacterSelector.secondCharacter == null)
        {
            CharacterSelector.secondCharacter = second;
        }
    }
}
