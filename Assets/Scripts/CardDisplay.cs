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
    [SerializeField] TextMeshProUGUI typeText;
    [SerializeField] Image typeBg;
    [SerializeField] Image typeFg;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI costMoney;
    [SerializeField] TextMeshProUGUI costMana;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] bool handCard;

    internal Card displayedCard;

    bool dragging;
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
        image.sprite = cardToDisplay.cardImage;

        if (cardToDisplay.moneyCost > 0)
        {
            costMoney.transform.parent.gameObject.SetActive(true);
            costMoney.text = cardToDisplay.moneyCost.ToString();
        }
        else
        {
            costMoney.transform.parent.gameObject.SetActive(false);
        }

        if (cardToDisplay.manaCost > 0)
        {
            costMana.transform.parent.gameObject.SetActive(true);
            costMana.text = cardToDisplay.manaCost.ToString();
        }
        else
        {
            costMana.transform.parent.gameObject.SetActive(false);
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
            Money.instance.TryPaying(displayedCard.moneyCost);
            Mana.instance.TryPaying(displayedCard.manaCost);

            if (displayedCard.cardType == CardType.tower)
            {
                if (TowerPlacer.towerPlaced)
                {
                    TowerPlacer.towerToPlace = null;
                    Hand.instance.DestroyCard(displayedCard);
                }
                else
                {
                    front.SetActive(true);
                    HandCardSlotController.instance.RearrangeCardSlots();
                }
                return;
            }

            if (displayedCard.cardType == CardType.action)
            {
                ActionCard actionCard = (ActionCard)displayedCard;
                actionCard.PlayAction();
                Discard.instance.DiscardCardFromHand(this);
                displayedCard = null;
                HandCardSlotController.instance.RearrangeCardSlots();
                return;
            }


            if (displayedCard.cardType == CardType.spell)
            {
                SpellPlacer.SpellPlaced();
                SpellPlacer.spellToPlace = null;
                front.SetActive(true);
                Discard.instance.DiscardCardFromHand(this);
                HandCardSlotController.instance.RearrangeCardSlots();

                return;
            }
        }
    }

    void CheckIfActivatable()
    {
        if (myCamera.ScreenToViewportPoint(transform.position).y >= 0.3f && front.activeSelf)
        {
            if (!CheckIfCardCanBePaid())
            {
                dragging = false;
                if(draggingCoroutine != null)
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
            if (displayedCard.cardType == CardType.tower)
            {
                front.SetActive(true);
                TowerPlacer.towerToPlace = null;
            }
            if (displayedCard.cardType == CardType.action)
            {

            }
            if (displayedCard.cardType == CardType.spell)
            {
                front.SetActive(true);
                Destroy(SpellPlacer.spellToPlace.gameObject);
                SpellPlacer.spellToPlace = null;
            }
        }
    }

    bool CheckIfCardCanBePaid()
    {
        return Money.instance.CheckAmount(displayedCard.moneyCost) && Mana.instance.CheckAmount(displayedCard.manaCost);
    }

    private void ActivateCard()
    {
        if (displayedCard.cardType == CardType.tower)
        {
            front.SetActive(false);
            TowerCard towerCard = (TowerCard)displayedCard;
            TowerPlacer.towerToPlace = towerCard.towerPrefab;
        }
        if (displayedCard.cardType == CardType.action)
        {

        }
        if (displayedCard.cardType == CardType.spell)
        {
            front.SetActive(false);
            SpellCard spellCard = (SpellCard)displayedCard;
            SpellPlacer.spellToPlace = Instantiate(spellCard.spellPrefab, null);
            StartCoroutine(SpellPlacer.PlaceSpell());
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
