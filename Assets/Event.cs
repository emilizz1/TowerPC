using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using I2.Loc;

[CreateAssetMenu(menuName = "Event")]
[Serializable]
public class Event : ScriptableObject
{
    public LocalizedString nameText;
    public LocalizedString descriptionText;
    public EventType type;
    public int positivity;
    public List<EventChoiceData> eventChoiceDatas;

    public void ChoiceMade(int choice)
    {
        if(eventChoiceDatas[choice].positiveEventEffect != EventEffect.None)
        {
            DoEffect(eventChoiceDatas[choice].positiveEventEffect, eventChoiceDatas[choice].amountToAdd, true, eventChoiceDatas[choice].cardToGet);
        }
        if (eventChoiceDatas[choice].negativeEventEffect != EventEffect.None)
        {
            DoEffect(eventChoiceDatas[choice].negativeEventEffect, eventChoiceDatas[choice].amountToRemove, false, eventChoiceDatas[choice].cardToGet);
        }

    }

    public void Preperation()
    {
        for (int i = 0; i < eventChoiceDatas.Count; i++)
        {

            if(eventChoiceDatas[i].positiveEventEffect == EventEffect.removeRandomCard || eventChoiceDatas[i].negativeEventEffect == EventEffect.removeRandomCard)
            {
                List<Card> allCards = new List<Card>(); 
                allCards.AddRange(Deck.instance.deckCards);
                allCards.AddRange(Discard.instance.discardCards);
                eventChoiceDatas[i].ChangeCard(allCards[UnityEngine.Random.Range(0, allCards.Count)]);
            }
        }
        }


    void DoEffect(EventEffect effect,int amount, bool positive, Card card)
    {
        List<Card> allCards = new List<Card>();
        switch (effect)
        {
            case (EventEffect.gold):
                if (positive)
                { 
                    if(amount == 999)
                    {
                        amount = Money.instance.currentAmount;
                    }
                    Money.instance.AddCurrency(amount, false);
                }
                else
                {
                    if (amount == 999)
                    {
                        amount = Money.instance.currentAmount;
                    }
                    Money.instance.TryPaying(Mathf.Min(amount, Money.instance.currentAmount));
                }
                break;
            case (EventEffect.health):
                if (positive)
                {
                    if (amount == 999)
                    {
                        amount = PlayerLife.instance.currentHP;
                    }
                    PlayerLife.instance.ChangeHealthAmount(Mathf.Min(amount, PlayerLife.instance.maxHp - PlayerLife.instance.currentHP));
                }
                else
                {
                    if (amount == 999)
                    {
                        amount = PlayerLife.instance.currentHP;
                    }
                    PlayerLife.instance.ChangeHealthAmount(-amount, true);
                }
                break;
            case (EventEffect.maxHealth):
                if (positive)
                {
                    PlayerLife.instance.MaxHealthAddition(amount);
                }
                else
                {
                    PlayerLife.instance.MaxHealthAddition(-amount);
                }
                break;
            case (EventEffect.curse):
                for (int i = 0; i < amount; i++)
                {
                    Deck.instance.AddCard(Instantiate(CardHolderManager.instance.curseCard));
                    ShowCardAnimation.instance.ShowCard(CardHolderManager.instance.curseCard, true);
                }
                break;
            case (EventEffect.mana):
                if (positive)
                {
                    if (amount == 999)
                    {
                        amount = Mana.instance.currentAmount;
                    }
                    Mana.instance.AddCurrency(amount, false);
                }
                else
                {
                    if (amount == 999)
                    {
                        amount = Mana.instance.currentAmount;
                    }
                    Mana.instance.TryPaying(Mathf.Min( amount, Mana.instance.currentAmount));
                }
                break;
            case (EventEffect.maxMana):
                if (positive)
                {
                    Mana.instance.IncreaseMaxMana(amount);
                }
                else
                {
                    Mana.instance.DecreaseMaxMana(amount);
                }
                break;
            case (EventEffect.research):
                for (int i = 0; i < amount; i++)
                {
                    ResearchWindow.instance.AdvanceResearch();
                }
                break;
            case (EventEffect.openMarket):
                MarketWindow.instance.Open(0);
                break;
            case (EventEffect.openForge):
                MarketWindow.instance.Open(1);
                break;
            case (EventEffect.openGraveyard):
                MarketWindow.instance.Open(2);
                break;
            case (EventEffect.bandits):
                GlobalConditionHolder.banditsAppear = true;
                break;
            case (EventEffect.noGold):
                CostController.AddNewPlayingCostMultiplayer(CardType.All, -0.9f);
                GlobalConditionHolder.noGold = true;
                break;
            case (EventEffect.handSize):
                Hand.instance.handSize += amount;                
                break;
            case (EventEffect.castleShield):
                PlayerLife.instance.IncreaseIgnorAmount( amount);
                break;
            case (EventEffect.fungus):
                GlobalConditionHolder.fungus = true;
                break;
            case (EventEffect.poisonDisabled):
                GlobalConditionHolder.poisonDisabled = true;
                break;
            case (EventEffect.engineCard):
                for (int i = 0; i < amount; i++)
                {
                    Deck.instance.AddCard(Instantiate(CardHolderManager.instance.engineCard));
                    ShowCardAnimation.instance.ShowCard(CardHolderManager.instance.engineCard, true);
                }
                break;
            case (EventEffect.removeAnyCard):
                allCards.AddRange(Deck.instance.deckCards);
                allCards.AddRange(Discard.instance.discardCards);
                CardShowcase.instance.Open("", allCards, ShowcasePurpose.Remove);
                break;
            case (EventEffect.ballistaSpeedReduction):
                TowerStats fireRateReduction = new TowerStats();
                fireRateReduction.fireRate += 0.8f;
                TowerPlacer.castleTower.statsMultiplayers.CombineStats(fireRateReduction);
                break;
            case (EventEffect.highChance):
                if (UnityEngine.Random.Range(0f, 1f) <= 0.7f)
                {
                    Money.instance.AddCurrency(50, false);
                }
                break;
            case (EventEffect.lowChance):
                if(UnityEngine.Random.Range(0f,1f) <= 0.3f)
                {
                    Money.instance.AddCurrency(120, false);
                }
                break;
            case (EventEffect.magicDamage):
                TowerStats additionalDamage = new TowerStats();
                additionalDamage.damage.Add(0.25f);
                additionalDamage.damage.Add(0.25f);
                additionalDamage.damage.Add(0.25f);
                PasiveTowerStatsController.AddAdditionalPasiveStats(DamageTypes.Magic, additionalDamage);
                break;
            case (EventEffect.curseChance):
                if(UnityEngine.Random.Range(0f,1f) <= 0.6f)
                {
                    Deck.instance.AddCard(Instantiate(CardHolderManager.instance.curseCard));
                    ShowCardAnimation.instance.ShowCard(CardHolderManager.instance.curseCard, true);
                }
                break;
            case (EventEffect.slowDisabled):
                GlobalConditionHolder.slowDisabled = true; 
                break;
            case (EventEffect.firstShield):
                //TODO Add this when shield enemy is created
                break;
            case (EventEffect.goldenCharm):
                GlobalConditionHolder.goldenCharm = true;
                break;
            case (EventEffect.goldenCharmToCoins):
                GlobalConditionHolder.goldenCharm = false;
                Money.instance.AddCurrency(200, false);
                break;
            case (EventEffect.leaveCard):
                allCards.AddRange(Deck.instance.deckCards);
                allCards.AddRange(Discard.instance.discardCards);
                CardShowcase.instance.Open("", allCards, ShowcasePurpose.Leave);

                Deck.instance.AddCard(Instantiate(CardHolderManager.instance.GetCardByName(SavedData.savesData.leftCard)));
                ShowCardAnimation.instance.ShowCard(CardHolderManager.instance.GetCardByName(SavedData.savesData.leftCard), true);
                break;
            case (EventEffect.sacrificeCard):
                allCards.AddRange(Deck.instance.deckCards);
                allCards.AddRange(Discard.instance.discardCards);
                CardShowcase.instance.Open("", allCards, ShowcasePurpose.Sacrifice);
                break;
            case (EventEffect.waterTiles):
                GlobalConditionHolder.waterTiles = true;
                break;
            case (EventEffect.gainCard):
                Deck.instance.AddCard(Instantiate(card));
                ShowCardAnimation.instance.ShowCard(card, true);
                break;
            case (EventEffect.upgradedGoldenCharm):
                GlobalConditionHolder.goldenCharm = false;
                GlobalConditionHolder.upgradedGoldenCharm = true;
                break;
            case (EventEffect.changeManaToCoins):
                Money.instance.AddCurrency(Mana.instance.currentAmount, false);
                Mana.instance.TryPaying(Mana.instance.currentAmount);
                break;
            case (EventEffect.spikyPlant):
                GlobalConditionHolder.spikyPlant = false;
                break;
            case (EventEffect.gainRandomCard):
                List<Card> possibleCards = new List<Card>();

                foreach(Card allCard in CardHolderManager.instance.allCards.cardsCollection[0].cards)
                {
                    if(allCard.cardLevel == 0)
                    {
                        if(allCard.cardCollectionType == CardCollectionType.Base ||
                            allCard.cardCollectionType == CharacterSelector.firstCharacter.type||
                            allCard.cardCollectionType == CharacterSelector.secondCharacter.type)
                        {
                            possibleCards.Add(allCard);
                        }
                    }
                }

                Card cardToAdd = possibleCards[UnityEngine.Random.Range(0, possibleCards.Count)];
                Deck.instance.deckCards.Add(cardToAdd);
                ShowCardAnimation.instance.ShowCard(cardToAdd, true);

                break;
            case (EventEffect.goldForSpells):
                GlobalConditionHolder.goldForSpells = true;
                break;
            case (EventEffect.upgradeAnyCard):
                allCards.AddRange(Deck.instance.deckCards);
                allCards.AddRange(Discard.instance.discardCards);
                CardShowcase.instance.Open("", allCards, ShowcasePurpose.Upgrade);
                break;
            case (EventEffect.removeRandomCard):
                //Need for it to pick random card and add it to card

                if (Deck.instance.deckCards.Contains(card))
                {
                    Deck.instance.deckCards.Remove(card);
                }
                else
                {
                    Discard.instance.discardCards.Remove(card);
                }
                break;
            case (EventEffect.goodBadLuck):
                int chance = UnityEngine.Random.Range(0, 3);
                if(chance == 0)
                {
                    ResearchWindow.instance.skipNextResearchResult = true;
                }
                else if( chance == 1)
                {
                    GlobalConditionHolder.maxHP = true;
                }
                else
                {
                    //Neutral just write text and icon
                }
                break;
            case (EventEffect.upgradeRandomCard):
                for (int i = 0; i < amount; i++)
                {
                    allCards.AddRange(Deck.instance.deckCards);
                    allCards.AddRange(Discard.instance.discardCards);
                    Card cardToUpgrade = allCards[UnityEngine.Random.Range(0, allCards.Count)];
                    if (Deck.instance.deckCards.Contains(cardToUpgrade))
                    {
                        Deck.instance.deckCards.Remove(cardToUpgrade);
                    }
                    else
                    {
                        Discard.instance.discardCards.Remove(cardToUpgrade);
                    }
                    cardToUpgrade = CardHolderManager.instance.GetUpgradedCard(cardToUpgrade);
                    Deck.instance.deckCards.Add(cardToUpgrade);
                    ShowCardAnimation.instance.ShowCard(card, true);
                }
                break;
            case (EventEffect.duplicateCard):
                allCards.AddRange(Deck.instance.deckCards);
                allCards.AddRange(Discard.instance.discardCards);
                CardShowcase.instance.Open("", allCards, ShowcasePurpose.Duplicate);
                break;
            case (EventEffect.reduceMaxManaHalf):
                Mana.instance.MaxManaAddition(-Mathf.RoundToInt(Mana.instance.maxAmount / 2f));
                break;
            case (EventEffect.cheaperNewHand):
                Deck.instance.newHandCostMultiplayer = 3;

                break;
            case (EventEffect.towerTax):
                GlobalConditionHolder.towerTax = true;
                break;
        }
    }
}

[Serializable]
public struct EventChoiceData
{
    public LocalizedString choiceDescription;
    public EventEffect positiveEventEffect;
    public int amountToAdd;
    public EventEffect negativeEventEffect;
    public int amountToRemove;
    public Card cardToGet;

    public void ChangeCard(Card card)
    {
        cardToGet = card;
    }
}

[Serializable]
public enum EventType
{
    Base,
    Knight,
    Mage,
    Admiral,
    Hunter,
    special
}

[Serializable]
public enum EventEffect
{
    None,
    gold,
    health,
    maxHealth,
    mana, 
    maxMana,
    curse,
    research,
    openMarket,
    openForge,
    openGraveyard,
    bandits,
    noGold,
    handSize,
    castleShield,
    fungus,
    poisonDisabled,
    engineCard,
    removeAnyCard,
    ballistaSpeedReduction,
    highChance,
    lowChance,
    magicDamage,
    curseChance,
    slowDisabled,
    firstShield,
    goldenCharm,
    goldenCharmToCoins,
    leaveCard,
    sacrificeCard,
    waterTiles,
    gainCard,
    upgradedGoldenCharm,
    changeManaToCoins,
    spikyPlant,
    gainRandomCard,
    goldForSpells,
    upgradeAnyCard,
    removeRandomCard,
    goodBadLuck,
    upgradeRandomCard,
    duplicateCard,
    reduceMaxManaHalf,
    cheaperNewHand,
    towerTax
}

