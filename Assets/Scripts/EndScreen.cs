using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.Loc;

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

    [SerializeField] Character.LevelUpDescription newWavesDescription;
    [SerializeField] Character.LevelUpDescription normalDifficultyDescription;
    [SerializeField] Character.LevelUpDescription hardDifficultyDescription;
    [SerializeField] Character.LevelUpDescription nightmareDifficultyDescription;

    [SerializeField] LocalizedString winText;
    [SerializeField] LocalizedString loseText;
    [SerializeField] LocalizedString wavesText;
    [SerializeField] LocalizedString enemiesText;
    [SerializeField] LocalizedString demoMaxText;
    [SerializeField] LocalizedString maxText;
    [SerializeField] LocalizedString playerLevelText;
    [SerializeField] LocalizedString levelText;

    int startingBaseLevel;
    int startingBaseProgress;
    int startingFirstCharacterProgress;
    int startingFirstCharacterLevel;
    int startingSecondCharacterProgress;
    int startingSecondCharacterLevel;

    int unlocksShowed;
    bool appeared;
    float timePassed = 0;

    public void Appear(string nameText)
    {
        if (appeared)
        {
            return;
        }
        appeared = true;

        bigName.text = nameText == "Win!"? winText : loseText;
        SoundsController.instance.PlayOneShot(nameText == "Win!" ? "Win!" : "Lose");

        if (nameText == "Win!")
        {
            if (TurnController.currentTurn >= 30)
            {
                AchievementManager.FinishedWave30();
            }
            if (PlayerLife.instance.IsHealthMax())
            {
                AchievementManager.FullLifeFinish();
            }

            int winsBefore = SavedData.savesData.wins;
            if(winsBefore < 10)
            {
                endScreenUnlocks[unlocksShowed].gameObject.SetActive(true);
                endScreenUnlocks[unlocksShowed].Display(newWavesDescription);
                unlocksShowed++;
            }

            if(SavedData.savesData.difficulty == 0)
            {
                if(SavedData.savesData.difficulty == 0)
                {
                    SavedData.savesData.difficultiesUnlocked = 1;
                    endScreenUnlocks[unlocksShowed].gameObject.SetActive(true);
                    endScreenUnlocks[unlocksShowed].Display(normalDifficultyDescription);
                    unlocksShowed++;
                }
            }
            else if (SavedData.savesData.difficulty == 1)
            {
                if (SavedData.savesData.difficulty == 1)
                {
                    SavedData.savesData.difficultiesUnlocked = 2;
                    endScreenUnlocks[unlocksShowed].gameObject.SetActive(true);
                    endScreenUnlocks[unlocksShowed].Display(hardDifficultyDescription);
                    unlocksShowed++;
                }
            }
            else if (SavedData.savesData.difficulty == 2)
            {
                if (SavedData.savesData.difficulty == 2)
                {
                    SavedData.savesData.difficultiesUnlocked = 3;
                    endScreenUnlocks[unlocksShowed].gameObject.SetActive(true);
                    endScreenUnlocks[unlocksShowed].Display(nightmareDifficultyDescription);
                    unlocksShowed++;
                }
            }

            SavedData.savesData.wins += 1;
        }

        SavedData.savesData.gamesPlayed = EnemyManager.instance.currentPlay + 1;
        SavedData.Save();
        Analytics.instance.FinishedMatch();

        Time.timeScale = 0f;
        wavesSurvived.text = wavesText +" " + TurnController.currentTurn.ToString();
        enemiesKilled.text = enemiesText +" " + EnemyManager.instance.enemiesKilled;

        parent.SetActive(true);

        startingBaseLevel = ProgressManager.GetLevel("Base");
        startingBaseProgress = ProgressManager.GetProgress("Base");
        baseLevel.text = playerLevelText +" " + startingBaseLevel;

        if(GameSettings.instance.demo && startingBaseLevel - 1 >= 2)
        {
            baseProgress.text = demoMaxText;
            baseFill.fillAmount = 1f;
        }
        else if (ProgressManager.baseLevelUps.Length <= startingBaseLevel - 1)
        {
            baseProgress.text = maxText;
            baseFill.fillAmount = 1f;
        }
        else
        {
            baseProgress.text = startingBaseProgress + "/" + ProgressManager.baseLevelUps[startingBaseLevel - 1];
            baseFill.fillAmount = (float)startingBaseProgress / (ProgressManager.baseLevelUps[startingBaseLevel - 1]);
        }

        startingFirstCharacterLevel = ProgressManager.GetLevel(CharacterSelector.firstCharacter.characterName);
        startingFirstCharacterProgress = ProgressManager.GetProgress(CharacterSelector.firstCharacter.characterName);
        firstLevel.text = levelText +" " + startingFirstCharacterLevel;
        firstIcon.sprite = CharacterSelector.firstCharacter.icon;
        if (GameSettings.instance.demo && startingFirstCharacterLevel - 1 >= 2)
        {
            firstProgress.text = demoMaxText;
            firstFill.fillAmount = 1f;
        }
        else if (ProgressManager.characterLevelUps.Length <= startingFirstCharacterLevel - 1)
        {
            firstProgress.text = maxText;
            firstFill.fillAmount = 1f;
        }
        else
        {
            firstProgress.text = startingFirstCharacterProgress + "/" + ProgressManager.characterLevelUps[startingFirstCharacterLevel - 1];
            firstFill.fillAmount = (float)startingFirstCharacterProgress / (ProgressManager.characterLevelUps[startingFirstCharacterLevel - 1]);
        }

        startingSecondCharacterLevel = ProgressManager.GetLevel(CharacterSelector.secondCharacter.characterName);
        startingSecondCharacterProgress = ProgressManager.GetProgress(CharacterSelector.secondCharacter.characterName);
        secondLevel.text = levelText + " " + startingSecondCharacterLevel;
        secondIcon.sprite = CharacterSelector.secondCharacter.icon;
        if (GameSettings.instance.demo && startingSecondCharacterLevel - 1 >= 2)
        {
            secondProgress.text = demoMaxText;
            secondFill.fillAmount = 1f;
        }
        else if (ProgressManager.characterLevelUps.Length <= startingSecondCharacterLevel - 1)
        {
            secondProgress.text = maxText;
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

        if (ProgressManager.baseLevelUps.Length >= startingBaseLevel - 1 || (GameSettings.instance.demo && 2 >= startingBaseLevel - 1))
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
                baseLevel.text = playerLevelText + " " + startingBaseLevel;
                startingValue = 0f;
                if (ProgressManager.baseLevelUps.Length <= startingBaseLevel - 1 || (GameSettings.instance.demo && 2 <= startingBaseLevel - 1))
                {
                    break;
                }

            }


            if (GameSettings.instance.demo && 2 <= startingBaseLevel - 1)
            {
                baseProgress.text = demoMaxText;
                baseFill.fillAmount = 1f;
            }
            else if (ProgressManager.baseLevelUps.Length <= startingBaseLevel - 1)
            {
                baseProgress.text = maxText;
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

        if (ProgressManager.characterLevelUps.Length >= startingFirstCharacterLevel - 1 || (GameSettings.instance.demo && 2 <= startingFirstCharacterLevel - 1))
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
                firstLevel.text = levelText+ " " + startingFirstCharacterLevel;
                startingValue = 0f;
                if (ProgressManager.characterLevelUps.Length <= startingFirstCharacterLevel - 1 || (GameSettings.instance.demo && 2 <= startingFirstCharacterLevel - 1))
                {
                    break;
                }
            }

            if (GameSettings.instance.demo && 2 <= startingFirstCharacterLevel - 1)
            {
                firstProgress.text = demoMaxText;
                firstFill.fillAmount = 1f;
            }
            else if (ProgressManager.characterLevelUps.Length <= startingFirstCharacterLevel - 1)
            {
                firstProgress.text = maxText;
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

        if (ProgressManager.characterLevelUps.Length >= startingSecondCharacterLevel - 1 || (GameSettings.instance.demo && 2 <= startingSecondCharacterLevel - 1))
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
                secondLevel.text = levelText + " " + startingSecondCharacterLevel;
                startingValue = 0f;
                if (ProgressManager.characterLevelUps.Length <= startingSecondCharacterLevel - 1 || (GameSettings.instance.demo  && 2 <= startingSecondCharacterLevel - 1))
                {
                    break;
                }

            }

            if (GameSettings.instance.demo && 2 <= startingSecondCharacterLevel - 1)
            {
                secondProgress.text = demoMaxText;
                secondFill.fillAmount = 1f;
            }
            else if (ProgressManager.characterLevelUps.Length <= startingSecondCharacterLevel - 1)
            {
                secondProgress.text = maxText;
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
