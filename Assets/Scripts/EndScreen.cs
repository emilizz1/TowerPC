using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreen : MonoSingleton<EndScreen>
{
    [SerializeField] GameObject parent;
    [SerializeField] TextMeshProUGUI wavesSurvived;
    [SerializeField] TextMeshProUGUI enemiesKilled;
    [SerializeField] TextMeshProUGUI bigName;

    [SerializeField] TextMeshProUGUI baseLevel;
    [SerializeField] TextMeshProUGUI baseProgress;
    [SerializeField] Image baseFill;

    [SerializeField] TextMeshProUGUI firstLevel;
    [SerializeField] TextMeshProUGUI firstProgress;
    [SerializeField] Image firstFill;
    [SerializeField] Image firstIcon;

    [SerializeField] TextMeshProUGUI secondLevel;
    [SerializeField] TextMeshProUGUI secondProgress;
    [SerializeField] Image secondFill;
    [SerializeField] Image secondIcon;

    [SerializeField] List<EndScreenUnlock> endScreenUnlocks;
    [SerializeField] BaseLevelUpDescriptions baseLevelUpDescriptions;

    int startingBaseLevel;
    int startingBaseProgress;
    int startingFirstCharacterProgress;
    int startingFirstCharacterLevel;
    int startingSecondCharacterProgress;
    int startingSecondCharacterLevel;

    int unlocksShowed;
    float timePassed = 0;

    public void Appear(string nameText)
    {
        bigName.text = nameText;
        SoundsController.instance.PlayOneShot(nameText == "Win!" ? "Win!" : "Lose");


        PlayerPrefs.SetInt("GamesPlayed", EnemyManager.instance.currentPlay + 1);
        Analytics.instance.FinishedMatch();

        Time.timeScale = 0f;
        wavesSurvived.text = "Waves Survived: " + TurnController.currentTurn.ToString();
        enemiesKilled.text = "Enemies Stopped: " + EnemyManager.instance.enemiesKilled;

        parent.SetActive(true);

        startingBaseLevel = ProgressManager.GetLevel("Base");
        startingBaseProgress = ProgressManager.GetProgress("Base");
        baseLevel.text = "Player Level: " + startingBaseLevel;
        if (ProgressManager.baseLevelUps.Length == startingBaseLevel - 1)
        {
            baseProgress.text = "Max";
            baseFill.fillAmount = 1f;
        }
        else
        {
            baseProgress.text = startingBaseProgress + "/" + ProgressManager.baseLevelUps[startingBaseLevel - 1];
            baseFill.fillAmount = (float)startingBaseProgress / (ProgressManager.baseLevelUps[startingBaseLevel - 1]);
        }

        startingFirstCharacterLevel = ProgressManager.GetLevel(CharacterSelector.firstCharacter.characterName);
        startingFirstCharacterProgress = ProgressManager.GetProgress(CharacterSelector.firstCharacter.characterName);
        firstLevel.text = "Level: " + startingFirstCharacterLevel;
        firstIcon.sprite = CharacterSelector.firstCharacter.icon;
        if (ProgressManager.characterLevelUps.Length == startingFirstCharacterLevel - 1)
        {
            firstProgress.text = "Max";
            firstFill.fillAmount = 1f;
        }
        else
        {
            firstProgress.text = startingFirstCharacterProgress + "/" + ProgressManager.characterLevelUps[startingFirstCharacterLevel - 1];
            firstFill.fillAmount = (float)startingFirstCharacterProgress / (ProgressManager.characterLevelUps[startingFirstCharacterLevel - 1]);
        }

        startingSecondCharacterLevel = ProgressManager.GetLevel(CharacterSelector.secondCharacter.characterName);
        startingSecondCharacterProgress = ProgressManager.GetProgress(CharacterSelector.secondCharacter.characterName);
        secondLevel.text = "Level: " + startingSecondCharacterLevel;
        secondIcon.sprite = CharacterSelector.secondCharacter.icon;
        if (ProgressManager.characterLevelUps.Length == startingSecondCharacterLevel - 1)
        {
            secondProgress.text = "Max";
            secondFill.fillAmount = 1f;
        }
        else
        {
            secondProgress.text = startingSecondCharacterProgress + "/" + ProgressManager.characterLevelUps[startingSecondCharacterLevel - 1];
            secondFill.fillAmount = (float)startingSecondCharacterProgress / (ProgressManager.characterLevelUps[startingSecondCharacterLevel - 1]);
        }

        ProgressManager.ChangeProgress("Base", EnemyManager.instance.enemiesKilled, false);
        ProgressManager.ChangeProgress(CharacterSelector.firstCharacter.characterName, EnemyManager.instance.enemiesKilled, true);
        ProgressManager.ChangeProgress(CharacterSelector.secondCharacter.characterName, EnemyManager.instance.enemiesKilled, true);

        StartCoroutine(AddExperience());
    }

    IEnumerator AddExperience()
    {
        int progressToRemove = 0;
        float startingValue = 0f;

        if (ProgressManager.baseLevelUps.Length != startingBaseLevel - 1)
        {
            startingValue = baseFill.fillAmount;
            while ((startingBaseProgress + EnemyManager.instance.enemiesKilled - progressToRemove) >= ProgressManager.baseLevelUps[startingBaseLevel - 1])
            {
                timePassed = 0;
                while (timePassed < 1f)
                {
                    timePassed += Time.unscaledDeltaTime;
                    baseFill.fillAmount = Mathf.Lerp(startingValue, 1f, timePassed);
                    baseProgress.text = Mathf.RoundToInt(ProgressManager.baseLevelUps[startingBaseLevel - 1] * baseFill.fillAmount) + "/" + ProgressManager.baseLevelUps[startingBaseLevel - 1];
                    yield return null;
                }

                endScreenUnlocks[unlocksShowed].gameObject.SetActive(true);
                endScreenUnlocks[unlocksShowed].Display(baseLevelUpDescriptions.levelUpDescriptions[startingBaseLevel - 1]);
                unlocksShowed++;

                progressToRemove += ProgressManager.baseLevelUps[startingBaseLevel - 1];
                startingBaseLevel++;
                baseLevel.text = "Player Level: " + startingBaseLevel;
                startingValue = 0f;
                if (ProgressManager.baseLevelUps.Length == startingBaseLevel - 1)
                {
                    break;
                }

            }

            if (ProgressManager.baseLevelUps.Length == startingBaseLevel - 1)
            {
                baseProgress.text = "Max";
                baseFill.fillAmount = 1f;
            }
            else
            {
                timePassed = 0;
                while (timePassed < 1f)
                {
                    timePassed += Time.unscaledDeltaTime;
                    baseFill.fillAmount = Mathf.Lerp(startingValue, ((float)startingBaseProgress + EnemyManager.instance.enemiesKilled - progressToRemove) / ProgressManager.baseLevelUps[startingBaseLevel - 1], timePassed);
                    baseProgress.text = Mathf.RoundToInt(ProgressManager.baseLevelUps[startingBaseLevel - 1] * baseFill.fillAmount) + "/" + ProgressManager.baseLevelUps[startingBaseLevel - 1];
                    yield return null;
                }
            }
        }

        Debug.Log("First check " + ProgressManager.characterLevelUps.Length + "  " + (startingFirstCharacterLevel - 1));
        if (ProgressManager.characterLevelUps.Length != startingFirstCharacterLevel - 1)
        {
            progressToRemove = 0;
            startingValue = firstFill.fillAmount;
            while ((startingFirstCharacterProgress + EnemyManager.instance.enemiesKilled - progressToRemove) >= ProgressManager.characterLevelUps[startingFirstCharacterLevel - 1])
            {
                timePassed = 0;
                while (timePassed < 1f)
                {
                    timePassed += Time.unscaledDeltaTime;
                    firstFill.fillAmount = Mathf.Lerp(startingValue, 1f, timePassed);
                    firstProgress.text = Mathf.RoundToInt(ProgressManager.characterLevelUps[startingFirstCharacterLevel - 1] * firstFill.fillAmount) + "/" + ProgressManager.characterLevelUps[startingFirstCharacterLevel - 1];
                    yield return null;

                }


                endScreenUnlocks[unlocksShowed].gameObject.SetActive(true);
                endScreenUnlocks[unlocksShowed].Display(CharacterSelector.firstCharacter.levelUpDescriptions[startingFirstCharacterLevel - 1]);
                unlocksShowed++;

                progressToRemove += ProgressManager.characterLevelUps[startingFirstCharacterLevel - 1];
                startingFirstCharacterLevel++;
                firstLevel.text = "Level: " + startingFirstCharacterLevel;
                startingValue = 0f;
                if (ProgressManager.characterLevelUps.Length == startingFirstCharacterLevel - 1)
                {
                    break;
                }
            }

            if (ProgressManager.characterLevelUps.Length == startingFirstCharacterLevel - 1)
            {
                firstProgress.text = "Max";
                firstFill.fillAmount = 1f;
            }
            else
            {
                timePassed = 0;
                while (timePassed < 1f)
                {
                    timePassed += Time.unscaledDeltaTime;
                    firstFill.fillAmount = Mathf.Lerp(startingValue, ((float)startingFirstCharacterProgress + EnemyManager.instance.enemiesKilled - progressToRemove) / ProgressManager.characterLevelUps[startingFirstCharacterLevel - 1], timePassed);
                    firstProgress.text = Mathf.RoundToInt(ProgressManager.characterLevelUps[startingFirstCharacterLevel - 1] * firstFill.fillAmount) + "/" + ProgressManager.characterLevelUps[startingFirstCharacterLevel - 1];
                    yield return null;
                }
            }
        }

        if (ProgressManager.characterLevelUps.Length != startingSecondCharacterLevel - 1)
        {
            progressToRemove = 0;
            startingValue = secondFill.fillAmount;
            while ((startingSecondCharacterProgress + EnemyManager.instance.enemiesKilled - progressToRemove) >= ProgressManager.characterLevelUps[startingSecondCharacterLevel - 1])
            {
                timePassed = 0;
                while (timePassed < 1f)
                {
                    timePassed += Time.unscaledDeltaTime;
                    secondFill.fillAmount = Mathf.Lerp(startingValue, 1f, timePassed);
                    secondProgress.text = Mathf.RoundToInt(ProgressManager.characterLevelUps[startingSecondCharacterLevel - 1] * secondFill.fillAmount) + "/" + ProgressManager.characterLevelUps[startingSecondCharacterLevel - 1];
                    yield return null;

                }

                endScreenUnlocks[unlocksShowed].gameObject.SetActive(true);
                endScreenUnlocks[unlocksShowed].Display(CharacterSelector.secondCharacter.levelUpDescriptions[startingSecondCharacterLevel - 1]);
                unlocksShowed++;

                progressToRemove += ProgressManager.characterLevelUps[startingSecondCharacterLevel - 1];
                startingSecondCharacterLevel++;
                secondLevel.text = "Level: " + startingSecondCharacterLevel;
                startingValue = 0f;
                if (ProgressManager.characterLevelUps.Length == startingSecondCharacterLevel - 1)
                {
                    break;

                }

            }

            if (ProgressManager.characterLevelUps.Length == startingSecondCharacterLevel - 1)
            {
                secondProgress.text = "Max";
                secondFill.fillAmount = 1f;
            }
            else
            {
                timePassed = 0;
                while (timePassed < 1f)
                {
                    timePassed += Time.unscaledDeltaTime;
                    secondFill.fillAmount = Mathf.Lerp(startingValue, ((float)startingSecondCharacterProgress + EnemyManager.instance.enemiesKilled - progressToRemove) / ProgressManager.characterLevelUps[startingSecondCharacterLevel - 1], timePassed);
                    secondProgress.text = Mathf.RoundToInt(ProgressManager.characterLevelUps[startingSecondCharacterLevel - 1] * secondFill.fillAmount) + "/" + ProgressManager.characterLevelUps[startingSecondCharacterLevel - 1];
                    yield return null;
                }
            }
        }
    }

    public void PressedPlayAgain()
    {
        SceneManager.LoadScene(SceneManager.CHARACTER_SELECTION);
    }

    public void PressedQuit()
    {

        Application.Quit();
    }

    public void PressedMenu()
    {
        SceneManager.LoadScene(SceneManager.MENU);

    }
}
