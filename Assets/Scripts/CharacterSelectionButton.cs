using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using I2.Loc;
using TMPro;

public class CharacterSelectionButton : MonoBehaviour
{
    [SerializeField] Image selectedBg;
    [SerializeField] Character character;
    [SerializeField] int lockedLevel;
    [SerializeField] GameObject lockScreen;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] string selectSound;
    [SerializeField] GameObject demoMax;
    [SerializeField] LocalizedString selectedText;
    [SerializeField] TextMeshProUGUI startingHealth;
    [SerializeField] TextMeshProUGUI startingMana;
    [SerializeField] TextMeshProUGUI startingCoins;
    [SerializeField] GameObject cover;

    bool selected;
    bool locked;

    private void Start()
    {
        selectedBg.gameObject.SetActive(false);
        demoMax.SetActive(GameSettings.instance.demo && ProgressManager.GetLevel(character.characterName) - 1 >= 2);

        level.text = selectedText + " " +ProgressManager.GetLevel(character.characterName);// + "/" + (ProgressManager.characterLevelUps.Length + 1);

        startingHealth.text = character.startingMaxHealth.ToString();
        startingMana.text = character.startingMana.ToString();
        startingCoins.text = character.startingMoney.ToString();

        if (ProgressManager.GetLevel("Base") < lockedLevel)
        {
            button.interactable = false;
            lockScreen.SetActive(true);
            level.transform.parent.gameObject.SetActive(false);
            locked = true;
        }
    }

    public void Pressed()
    {
        if (selected)
        {
            CharacterSelectionController.instance.Deselected(character);
            selected = false;
            selectedBg.gameObject.SetActive(false);
            SoundsController.instance.PlayOneShot("Click");
        }
        else
        {
            if (CharacterSelectionController.instance.TrySelecting(character))
            {
                selectedBg.gameObject.SetActive(true);
                selected = true;
                SoundsController.instance.PlayOneShot(selectSound);

            }
        }
    }

    private void OnMouseEnter()
    {
        if (!locked)
        {
            cover.SetActive(true);
            gameObject.transform.localScale = new Vector3(1.05f, 1.05f, 1f);
        }
    }

    private void OnMouseExit()
    {
        if (!locked)
        {
            cover.SetActive(false);
            gameObject.transform.localScale = Vector3.one;
        }
    }
}
