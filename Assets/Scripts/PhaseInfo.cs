using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhaseInfo : MonoSingleton<PhaseInfo>
{
    [SerializeField] List<TextMeshProUGUI> phaseNames;

    public void PhaseChanged(TurnController.TurnPhase newPhase)
    {
        switch (newPhase)
        {
            case (TurnController.TurnPhase.Drawing):
                phaseNames[0].color = Color.white;
                phaseNames[4].color = new Color(1f, 1f, 1f, 0.5f);
                return;
            case (TurnController.TurnPhase.Research):
                phaseNames[1].color = Color.white;
                phaseNames[0].color = new Color(1f, 1f, 1f, 0.5f);
                return;
            case (TurnController.TurnPhase.Preperation):
                phaseNames[2].color = Color.white;
                phaseNames[1].color = new Color(1f, 1f, 1f, 0.5f);
                return;
            case (TurnController.TurnPhase.EnemyWave):
                phaseNames[3].color = Color.white;
                phaseNames[2].color = new Color(1f, 1f, 1f, 0.5f);
                return;
            case (TurnController.TurnPhase.Market):
                phaseNames[4].color = Color.white;
                phaseNames[3].color = new Color(1f, 1f, 1f, 0.5f);
                return;
        }
    }
}
