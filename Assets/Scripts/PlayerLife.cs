using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLife : MonoSingleton<PlayerLife>
{
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] Image fill;
    [SerializeField] MeshRenderer castle;
    [SerializeField] List<GameObject> shields;

    internal int regen;

    internal int maxHp;
    internal int currentHP;
    int maxIgnoredEachRound;
    int currentIgnoredEachRound;

    List<Color> rendererColors;
    Color fillNormalColor;

    private void Start()
    {
        regen = CharacterSelector.firstCharacter.characterName == "Hunter" || CharacterSelector.secondCharacter.characterName == "Hunter" ? 1 : 0;
        maxHp = CharacterSelector.firstCharacter.startingMaxHealth + CharacterSelector.secondCharacter.startingMaxHealth;
        currentHP = maxHp;
        fillNormalColor = fill.color;
        UpdateHealth();
        if (rendererColors == null)
        {
            SetupRendererColors();
        }
    }

    void UpdateHealth()
    {
        amountText.text = Mathf.Max(0, currentHP) + " / " + maxHp;
        fill.fillAmount = (float)currentHP / maxHp;
        for (int i = 0; i < shields.Count; i++)
        {
            shields[i].SetActive(i < currentIgnoredEachRound);
        }
    }

    public void ChangeHealthAmount(int change, bool ignoreShields = false)
    {
        if (change < 0 && !ignoreShields)
        {
            if(Mana.instance.shouldUseManaShield && Mana.instance.currentAmount >= 50)
            {
                Mana.instance.TryPaying(50);
                SoundsController.instance.PlayOneShot("Mana");
                return;

            }
            if (currentIgnoredEachRound > 0)
            {
                currentIgnoredEachRound--;
                SoundsController.instance.PlayOneShot("Mana");
                UpdateHealth();
                return;
            }
        }
        if(change < 0)
        {
            StartCoroutine(HitAnimation());
        }
        currentHP += change;
        UpdateHealth();
        SoundsController.instance.PlayOneShot("Damaged");
        if(currentHP <= 0)
        {
            Lost();
        }
    }

    public void MaxHealthAddition(int amount)
    {
        maxHp += amount;
        currentHP = Mathf.Min(maxHp, currentHP + amount);
        UpdateHealth();
    }

    public void Regen()
    {
        if (GlobalConditionHolder.maxHP)
        {
            maxHp += 1;
            currentHP += 1;
        }
        if (GlobalConditionHolder.upgradedGoldenCharm)
        {
            currentHP = Mathf.Clamp(currentHP + TurnController.actionsPlayed, 0, maxHp);
        }
        currentHP = Mathf.Clamp(currentHP + regen, 0, maxHp);
        UpdateHealth();
    }

    void Lost()
    {
        EndScreen.instance.Appear("Lost");
    }

    public void IncreaseIgnorAmount(int amount)
    {
        maxIgnoredEachRound += amount;
        currentIgnoredEachRound += amount;
        UpdateHealth();
    }

    public void NewRound()
    {
        currentIgnoredEachRound = maxIgnoredEachRound;
        UpdateHealth();
    }

    private void SetupRendererColors()
    {
        rendererColors = new List<Color>();
        foreach (Material material in castle.materials)
        {
            rendererColors.Add(material.color);
        }
    }

    IEnumerator HitAnimation()
    {
        for (int i = 0; i < rendererColors.Count; i++)
        {
            castle.materials[i].color = Color.red;
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < rendererColors.Count; i++)
        {
            castle.materials[i].color = rendererColors[i];
        }
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < rendererColors.Count; i++)
        {
            castle.materials[i].color = Color.red;
        }
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < rendererColors.Count; i++)
        {
            castle.materials[i].color = rendererColors[i];
        }
    }

    public  bool IsHealthMax()
    {
        return currentHP >= maxHp;
    }
}
