using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleTower : Tower
{
    public virtual void Start()
    {
        if (CharacterSelector.firstCharacter.characterName == "Knight" || CharacterSelector.secondCharacter.characterName == "Knight")
        {            
            PrepareTower(null);
            Activate();
            TowerPlacer.castleTower = this;
            rangeSprite.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public override void AddExperience()
    {

    }

    public override void LevelUp()
    {
        
    }
}
