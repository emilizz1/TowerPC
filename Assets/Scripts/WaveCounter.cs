using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveCounter : MonoSingleton<WaveCounter>
{
    [SerializeField] TextMeshProUGUI text;


    public void DisplayCounter()
    {
        text.text = "Wave " + TurnController.currentTurn + "/" + EnemyManager.instance.wavesTarget;
    }
}
