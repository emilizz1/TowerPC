using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;

public class Analytics : MonoSingleton<Analytics>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

    }

    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            Debug.LogError("Analytics not initialized " + e);
        }
    }


    public void StartedMatch()
    {
#if !UNITY_EDITOR

        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "BaseLevel", ProgressManager.GetLevel("Base") },
            { "firstCharLevel", ProgressManager.GetLevel(CharacterSelector.firstCharacter.characterName) },
            { "FirstCharName", CharacterSelector.firstCharacter.characterName},
            { "SecondCharLevel", ProgressManager.GetLevel(CharacterSelector.secondCharacter.characterName) },
            { "SecondCharName", CharacterSelector.secondCharacter.characterName},
        };

        AnalyticsService.Instance.CustomData("MatchStart", parameters);

        AnalyticsService.Instance.Flush();
#endif
    }

    public void FinishedMatch()
    {
#if !UNITY_EDITOR
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "BaseLevel", ProgressManager.GetLevel("Base") },
            { "CastleHealth", PlayerLife.instance.currentHP },
            { "firstCharLevel", ProgressManager.GetLevel(CharacterSelector.firstCharacter.characterName) },
            { "FirstCharName", CharacterSelector.firstCharacter.characterName},
            { "SecondCharLevel", ProgressManager.GetLevel(CharacterSelector.secondCharacter.characterName) },
            { "SecondCharName", CharacterSelector.secondCharacter.characterName},
            { "WavesPlayed", TurnController.currentTurn },
        };

        AnalyticsService.Instance.CustomData("FinishedMatch", parameters);

        AnalyticsService.Instance.Flush();
#endif
    }

    public void LevelUp(string name, int level)
    {
#if !UNITY_EDITOR
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "Level", level },
            { "Character", name }
        };

        AnalyticsService.Instance.CustomData("LevelUp", parameters);

        AnalyticsService.Instance.Flush();
#endif
    }

    public void Researched(string researchName)
    {
#if !UNITY_EDITOR
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "Research", researchName },
            { "Wave", TurnController.currentTurn },
            { "BaseLevel", ProgressManager.GetLevel("Base") }
        };

        AnalyticsService.Instance.CustomData("Researched", parameters);

        AnalyticsService.Instance.Flush();
#endif

    }
}