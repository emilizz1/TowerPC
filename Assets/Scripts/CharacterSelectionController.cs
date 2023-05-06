using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using I2.Loc;
using TMPro;

public class CharacterSelectionController : MonoSingleton<CharacterSelectionController>
{
    public static int MAX_SELECTED = 2;

    [SerializeField] TextMeshProUGUI selectedCount;
    [SerializeField] GameObject nextButton;
    [SerializeField] TextMeshProUGUI playerLevel;
    [SerializeField] GameObject demoMax;
    [SerializeField] LocalizedString playerLevelText;
    [SerializeField] LocalizedString selectedBaseText;
    [SerializeField] TweenAnimator animator;
    [SerializeField] TweenAnimator coverAnimator;
    [SerializeField] Image easyFill;
    [SerializeField] Image normalFill;
    [SerializeField] Image hardFill;
    [SerializeField] Image nightmareFill;


    List<Character> currentlySelected = new List<Character>();

    private void Start()
    {
        UpdateText();
        demoMax.SetActive(GameSettings.instance.demo && ProgressManager.GetLevel("Base")-1 >= 2);
        playerLevel.text = playerLevelText + " " + ProgressManager.GetLevel("Base");// + "/" + (ProgressManager.baseLevelUps.Length+1);
        currentlySelected = new List<Character>();
        //Soundtrack.instance.CharacterSelectScreen();
        SetupFills();
    }

    void SetupFills()
    {
        easyFill.transform.parent.parent.gameObject.SetActive(SavedData.savesData.wins > 0);
        easyFill.fillAmount = SavedData.savesData.wins / 10f;

        normalFill.transform.parent.parent.gameObject.SetActive(SavedData.savesData.normalWins > 0);
        normalFill.fillAmount = SavedData.savesData.normalWins / 10f;

        hardFill.transform.parent.parent.gameObject.SetActive(SavedData.savesData.hardWins > 0);
        hardFill.fillAmount = SavedData.savesData.hardWins / 10f;

        nightmareFill.transform.parent.parent.gameObject.SetActive(SavedData.savesData.nightmareWins > 0);
        nightmareFill.fillAmount = SavedData.savesData.nightmareWins / 10f;
    }

    public bool TrySelecting(Character character)
    {
        if(currentlySelected.Count < MAX_SELECTED)
        {
            currentlySelected.Add(character);
            UpdateText();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Deselected(Character character)
    {
        currentlySelected.Remove(character);
        UpdateText();
    }

    void UpdateText()
    {
        selectedCount.text = selectedBaseText + " " + currentlySelected.Count + " / " + MAX_SELECTED;
        nextButton.SetActive(currentlySelected.Count == MAX_SELECTED);
    }

    public void StartGame()
    {
        CharacterSelector.firstCharacter = currentlySelected[0];
        CharacterSelector.secondCharacter = currentlySelected[1];
        SceneManager.LoadScene(SceneManager.GAME);
    }
    public void ReturnToMenu()
    {
        animator.PerformTween(0);
        coverAnimator.PerformTween(0);
    }

    public void Open()
    {
        coverAnimator.gameObject.SetActive(true);
        coverAnimator.PerformTween(0);
        animator.PerformTween(1);
    }
}
