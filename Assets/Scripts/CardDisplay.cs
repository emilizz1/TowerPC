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
    [SerializeField] Image typeBg;
    [SerializeField] Image typeFg;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI costMoney;
    [SerializeField] TextMeshProUGUI costMana;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI keywords;
    [SerializeField] bool handCard;
    [SerializeField] List<GameObject> cardLevel;

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

        if (cardToDisplay.moneyCost > 0)
        {
            costMoney.transform.parent.gameObject.SetActive(true);
            costMoney.text = Mathf.CeilToInt(cardToDisplay.moneyCost * CostController.GetPlayingCostMultiplayer(cardToDisplay.cardType)).ToString();
        }
        else
        {
            costMoney.transform.parent.gameObject.SetActive(false);
        }

        if (cardToDisplay.manaCost > 0)
        {
            costMana.transform.parent.gameObject.SetActive(true);
            costMana.text = Mathf.CeilToInt(displayedCard.manaCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)).ToString();
        }
        else
        {
            costMana.transform.parent.gameObject.SetActive(false);
        }

        cardName.text = displayedCard.cardName;

        typeBg.color = CardTypeColors.GetColor(displayedCard.cardType);
        typeFg.color = CardTypeColors.GetColor(displayedCard.cardType);

        description.text = cardToDisplay.description;

        for (int i = 0; i < cardLevel.Count; i++)
        {
            cardLevel[i].SetActive(i < cardToDisplay.cardLevel);
        }

        back.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (handCard && !Cover.cover)
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
        if (!Cover.cover)
        {
            HandCardSlotController.instance.RearrengeCardSlotsWithSelectedCard(HandCardSlotController.instance.cardDisplays.IndexOf(this));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HandCardSlotController.instance.RearrangeCardSlots();
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
            if (displayedCard.cardType == CardType.tower)
            {
                if (TowerPlacer.towerPlaced)
                {
                    Money.instance.TryPaying(Mathf.CeilToInt(displayedCard.moneyCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)));
                    Mana.instance.TryPaying(Mathf.CeilToInt(displayedCard.manaCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)));
                    Hand.instance.handCards.Remove(displayedCard);
                    Discard.instance.DiscardCardFromHand(this);
                    displayedCard = null;
                }

                TowerPlacer.towerToPlace = null;
                front.SetActive(true);
                HandCardSlotController.instance.RearrangeCardSlots();
                TowerInfoWindow.instance.Close();
                return;
            }

            if (displayedCard.cardType == CardType.action)
            {
                Money.instance.TryPaying(Mathf.CeilToInt(displayedCard.moneyCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)));
                Mana.instance.TryPaying(Mathf.CeilToInt(displayedCard.manaCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)));
                ActionCard actionCard = (ActionCard)displayedCard;
                actionCard.PlayAction();
                Hand.instance.handCards.Remove(displayedCard);
                Discard.instance.DiscardCardFromHand(this);
                displayedCard = null;
                HandCardSlotController.instance.RearrangeCardSlots();
                return;
            }

            if (displayedCard.cardType == CardType.spell)
            {
                if (!SpellPlacer.spellPlaced)
                {
                    Money.instance.TryPaying(Mathf.CeilToInt(displayedCard.moneyCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)));
                    Mana.instance.TryPaying(Mathf.CeilToInt(displayedCard.manaCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)));
                    SpellPlacer.SpellPlaced();
                    SpellPlacer.spellToPlace = null;
                    front.SetActive(true);
                    Hand.instance.handCards.Remove(displayedCard);
                    Discard.instance.DiscardCardFromHand(this);
                    displayedCard = null;
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

            if (displayedCard.cardType == CardType.structure)
            {
                if (StructurePlacer.structurePlaced)
                {
                    Money.instance.TryPaying(Mathf.CeilToInt(displayedCard.moneyCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)));
                    Mana.instance.TryPaying(Mathf.CeilToInt(displayedCard.manaCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)));
                    Hand.instance.handCards.Remove(displayedCard);
                    Discard.instance.DiscardCardFromHand(this);
                    displayedCard = null;
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

    void CheckIfActivatable()
    {
        if (myCamera.ScreenToViewportPoint(transform.position).y >= 0.3f && front.activeSelf)
        {
            if (!CheckIfCardCanBePaid())
            {
                dragging = false;
                if (draggingCoroutine != null)
                {
                    StopCoroutine(draggingCoroutine);
                }
                HandCardSlotController.instance.RearrangeCardSlots();
                return;
            }
            ActivateCard();

        }
        else if (myCamera.ScreenToViewportPoint(transform.position).y < 0.3f && !front.activeSelf)
        {
            ReturnCardToHand();
        }
    }

    public void ReturnCardToHand()
    {
        front.SetActive(true);
        HandCardSlotController.instance.RearrangeCardSlots();
        if (displayedCard.cardType == CardType.tower)
        {
            TowerPlacer.towerToPlace = null;
            TowerInfoWindow.instance.Close();
            TileManager.instance.CheckForMisplacedTowers();
        }
        if (displayedCard.cardType == CardType.action)
        {

        }
        if (displayedCard.cardType == CardType.spell)
        {
            if (SpellPlacer.spellToPlace != null)
            {
                Destroy(SpellPlacer.spellToPlace.gameObject);
                SpellPlacer.spellToPlace = null;
            }
        }
        if (displayedCard.cardType == CardType.structure)
        {
            StructurePlacer.structureToPlace = null;

        }
        }

    bool CheckIfCardCanBePaid()
    {
        return Money.instance.CheckAmount(Mathf.CeilToInt(displayedCard.moneyCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType))) &&
            Mana.instance.CheckAmount(Mathf.CeilToInt(displayedCard.manaCost * CostController.GetPlayingCostMultiplayer(displayedCard.cardType)));
    }

    private void ActivateCard()
    {
        if (displayedCard.cardType == CardType.tower)
        {
            front.SetActive(false);
            TowerCard towerCard = (TowerCard)displayedCard;
            TowerPlacer.towerToPlace = towerCard.towerPrefab;
            TowerPlacer.startingLevel = towerCard.cardLevel;
        }
        if (displayedCard.cardType == CardType.action)
        {

        }
        if (displayedCard.cardType == CardType.spell)
        {
            front.SetActive(false);
            SpellCard spellCard = (SpellCard)displayedCard;
            SpellPlacer.spellToPlace = Instantiate(spellCard.spellPrefab, new Vector3(0, 1000f,0f), Quaternion.identity, null);
            StartCoroutine(SpellPlacer.PlaceSpell());
            SpellPlacer.spellPlaced = false;
        }
        if (displayedCard.cardType == CardType.structure)
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
}
