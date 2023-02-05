using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject front;
    [SerializeField] GameObject back;
    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] Image typeFg;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI costMoney;
    [SerializeField] TextMeshProUGUI costMana;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI keywords;
    [SerializeField] bool handCard;
    [SerializeField] List<GameObject> cardLevel;
    [SerializeField] List<GameObject> cardLevelStars;
    [SerializeField] TweenAnimator notEnoughMoney;
    [SerializeField] TweenAnimator notEnoughMana;

    internal Card displayedCard;

    internal bool dragging;
    Camera myCamera;
    Coroutine draggingCoroutine;

    private void Start()
    {
        myCamera = Camera.main;
    }

    public void DisplayCard(Card cardToDisplay)
    {
        displayedCard = cardToDisplay;
        front.SetActive(true);
        if(cardToDisplay == null || cardToDisplay.cardImage == null)
        {
            Debug.Log("Card Missing: " + cardToDisplay.name);
            return;
        }
        image.sprite = cardToDisplay.cardImage;

        string keywordText = displayedCard.cardType.ToString();
        foreach (PasiveTowerStatsController.DamageTypes type in displayedCard.damageTypes)
        {
            if (type != PasiveTowerStatsController.DamageTypes.None)
            {
                keywordText += ", " + type.ToString();
            }
        }
        keywords.text = keywordText;

        costMoney.text = Mathf.Max(Mathf.CeilToInt(cardToDisplay.moneyCost * CostController.GetPlayingCostMultiplayer(cardToDisplay.cardType)) - CostController.currentTurnDiscount,0).ToString();
        costMana.text = Mathf.Max(Mathf.CeilToInt(displayedCard.manaCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)) - CostController.currentTurnDiscount,0).ToString();

        cardName.text = displayedCard.cardName;

        typeFg.color = CardTypeColors.GetColor(displayedCard.cardType);

        description.text = cardToDisplay.description;

        for (int i = 0; i < cardLevel.Count; i++)
        {
            cardLevel[i].SetActive(i < cardToDisplay.cardLevel);
            cardLevelStars[i].SetActive(i < cardToDisplay.cardLevel);
        }

        back.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (handCard && !Cover.cover && !Input.GetMouseButton(1))
        {
            dragging = true;
            draggingCoroutine = StartCoroutine(Dragging());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (handCard)
        {
            dragging = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (handCard)
        {
            if (!Cover.cover)
            {
                HandCardSlotController.instance.RearrengeCardSlotsWithSelectedCard(HandCardSlotController.instance.cardDisplays.IndexOf(this));
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (handCard)
        {
            HandCardSlotController.instance.RearrangeCardSlots();
        }
    }

    IEnumerator Dragging()
    {
        Vector3 diff = transform.position - Input.mousePosition;

        while (dragging)
        {
            Vector3 newPos = Input.mousePosition;
            newPos.z = 0f;
            transform.position = Input.mousePosition + diff;
            CheckIfActivatable();
            yield return null;
        }

        CheckIfPlayed();
    }

    void CheckIfPlayed()
    {
        if (myCamera.ScreenToViewportPoint(transform.position).y >= 0.3f)
        {
            if (displayedCard.cardType == CardType.Tower)
            {
                if (TowerPlacer.towerPlaced)
                {
                    Money.instance.TryPaying(Mathf.CeilToInt(displayedCard.moneyCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType) - CostController.currentTurnDiscount));
                    Mana.instance.TryPaying(Mathf.CeilToInt(displayedCard.manaCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType) - CostController.currentTurnDiscount));
                    CardUsed();
                }

                TowerPlacer.towerToPlace = null;
                front.SetActive(true);
                HandCardSlotController.instance.RearrangeCardSlots();
                TowerInfoWindow.instance.Close();
                return;
            }

            if (displayedCard.cardType == CardType.Action)
            {
                Money.instance.TryPaying(Mathf.CeilToInt(displayedCard.moneyCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType) - CostController.currentTurnDiscount));
                Mana.instance.TryPaying(Mathf.CeilToInt(displayedCard.manaCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType) - CostController.currentTurnDiscount));
                ActionCard actionCard = (ActionCard)displayedCard;
                actionCard.PlayAction();
                CardUsed();
                HandCardSlotController.instance.RearrangeCardSlots();
                return;
            }

            if (displayedCard.cardType == CardType.Spell)
            {
                if (!SpellPlacer.spellPlaced)
                {
                    Money.instance.TryPaying(Mathf.CeilToInt(displayedCard.moneyCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType) - CostController.currentTurnDiscount));
                    Mana.instance.TryPaying(Mathf.CeilToInt(displayedCard.manaCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType) - CostController.currentTurnDiscount));
                    SpellPlacer.SpellPlaced();
                    SpellPlacer.spellToPlace = null;
                    front.SetActive(true);
                    CardUsed();
                    AchievementManager.SpellCasted();
                    HandCardSlotController.instance.RearrangeCardSlots();
                }
                else
                {
                    front.SetActive(true);
                    //SpellPlacer.spellToPlace = null;
                    HandCardSlotController.instance.RearrangeCardSlots();

                }
                return;
            }

            if (displayedCard.cardType == CardType.Structure)
            {
                if (StructurePlacer.structurePlaced)
                {
                    Money.instance.TryPaying(Mathf.CeilToInt(displayedCard.moneyCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType) - CostController.currentTurnDiscount));
                    Mana.instance.TryPaying(Mathf.CeilToInt(displayedCard.manaCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType) - CostController.currentTurnDiscount));
                    CardUsed();
                }

                StructurePlacer.structureToPlace = null;
                front.SetActive(true);
                HandCardSlotController.instance.RearrangeCardSlots();
                TowerInfoWindow.instance.Close();
                return;
            }
            }
        else
        {
            ReturnCardToHand();
        }
    }

    void CardUsed()
    {

        Hand.instance.handCards.Remove(displayedCard);
        Discard.instance.DiscardCardFromHand(this);

        if (Hand.instance.handCards.Count == 0)
        {
            TipsManager.instance.CheckForTipDrawMoreCards();
        }
    }

    public void DestroyCard()
    {
        SoundsController.instance.PlayOneShot("Destroy");
        LeanTween.move(gameObject, new Vector3(UnityEngine.Random.Range(2000f, 2500f), UnityEngine.Random.Range(2000f, 2500f)), 1f);
        LeanTween.rotate(gameObject, new Vector3(0f, 0f, 1000f), 1f);
        StartCoroutine(ResetAfterTime(1f));
    }

    void CheckIfActivatable()
    {
        if (myCamera.ScreenToViewportPoint(transform.position).y >= 0.3f && front.activeSelf)
        {
            if (!CheckIfCardCanBePaid())
            {
                ReturnCard();
                return;
            }

            if (displayedCard.cardType == CardType.Action)
            {
                ActionCard actionCard = (ActionCard)displayedCard;
                if (!actionCard.CanItBePlayed())
                {
                    ReturnCard();
                    return;
                }
            }
            ActivateCard();

        }
        else if (myCamera.ScreenToViewportPoint(transform.position).y < 0.3f && !front.activeSelf)
        {
            ReturnCardToHand();
        }
    }

    private void ReturnCard()
    {
        dragging = false;
        if (draggingCoroutine != null)
        {
            StopCoroutine(draggingCoroutine);
        }
        HandCardSlotController.instance.RearrangeCardSlots();
    }

    public void ReturnCardToHand()
    {
        front.SetActive(true);
        HandCardSlotController.instance.RearrangeCardSlots();
        if (displayedCard.cardType == CardType.Tower)
        {
            TowerPlacer.towerToPlace = null;
            TowerInfoWindow.instance.Close();
            TileManager.instance.CheckForMisplacedTowers();
        }
        if (displayedCard.cardType == CardType.Action)
        {

        }
        if (displayedCard.cardType == CardType.Spell)
        {
            if (SpellPlacer.spellToPlace != null)
            {
                Destroy(SpellPlacer.spellToPlace.gameObject);
                SpellPlacer.spellToPlace = null;
            }
        }
        if (displayedCard.cardType == CardType.Structure)
        {
            StructurePlacer.structureToPlace = null;
            TileManager.instance.CheckForMisplacedTowers();

        }
        }

    bool CheckIfCardCanBePaid()
    {
        if(!Money.instance.CheckAmount(Mathf.CeilToInt(displayedCard.moneyCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)) - CostController.currentTurnDiscount))
        {
            notEnoughMoney.PerformTween(0);
        }
        if (!Mana.instance.CheckAmount(Mathf.CeilToInt(displayedCard.manaCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)) - CostController.currentTurnDiscount))
        {
            notEnoughMana.PerformTween(0);
        }
        return Money.instance.CheckAmount(Mathf.CeilToInt(displayedCard.moneyCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)) - CostController.currentTurnDiscount) &&
            Mana.instance.CheckAmount(Mathf.CeilToInt(displayedCard.manaCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)) - CostController.currentTurnDiscount);
    }

    private void ActivateCard()
    {
        if (displayedCard.cardType == CardType.Tower)
        {
            front.SetActive(false);
            TowerCard towerCard = (TowerCard)displayedCard;
            TowerPlacer.towerToPlace = towerCard.towerPrefab;
            TowerPlacer.towerPlaced = false;
            TowerPlacer.startingLevel = towerCard.cardLevel;
        }
        if (displayedCard.cardType == CardType.Action)
        {

        }
        if (displayedCard.cardType == CardType.Spell)
        {
            front.SetActive(false);
            SpellCard spellCard = (SpellCard)displayedCard;
            SpellPlacer.spellToPlace = Instantiate(spellCard.spellPrefab, new Vector3(0, 1000f,0f), Quaternion.identity, null);
            StartCoroutine(SpellPlacer.PlaceSpell());
            SpellPlacer.spellPlaced = false;
        }
        if (displayedCard.cardType == CardType.Structure)
        {
            front.SetActive(false);
            StructureCard towerCard = (StructureCard)displayedCard;
            StructurePlacer.structureToPlace = towerCard.structurePrefab;
        }
    }

    public IEnumerator ResetAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        front.SetActive(false);
        back.SetActive(false);
        displayedCard = null;
    }

    public void FlashCard()
    {
        StartCoroutine(FlashCardAnimation());
    }

    public IEnumerator FlashCardAnimation()
    {
        Color startingColor = typeFg.color;
        Color newColor = Color.red;

        typeFg.color = newColor;
        yield return new WaitForSeconds(0.1f);
        typeFg.color = startingColor;
        yield return new WaitForSeconds(0.1f);
        typeFg.color = newColor;
        yield return new WaitForSeconds(0.15f);
        typeFg.color = startingColor;
        yield return new WaitForSeconds(0.15f);
        typeFg.color = newColor;
        yield return new WaitForSeconds(0.2f);
        typeFg.color = startingColor;
        yield return new WaitForSeconds(0.2f);
        typeFg.color = newColor;
        yield return new WaitForSeconds(0.25f);
        typeFg.color = startingColor;
        yield return new WaitForSeconds(0.25f);
        typeFg.color = newColor;
        yield return new WaitForSeconds(0.3f);
        typeFg.color = startingColor;
        yield return new WaitForSeconds(0.3f);
        typeFg.color = newColor;
        yield return new WaitForSeconds(0.35f);
        typeFg.color = startingColor;
        yield return new WaitForSeconds(0.35f);
        typeFg.color = newColor;
        yield return new WaitForSeconds(0.4f);
        typeFg.color = startingColor;
    }
}
