using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Usata quando si prelevano carte dal discardDeck
public class TakeCard : Command {

    Card card;
    DiscardDeck discardDeck;
    Column targetColumn;
    int scoredPoints;

    public TakeCard(Card card, DiscardDeck origin, Column target, int points)
    {
        discardDeck = origin;
        targetColumn = target;
        scoredPoints = points;
        this.card = card;
    }

    public override void Execute()
    {
        targetColumn.AddCards(discardDeck.RemoveCard());
    }

    public override int GetPoints()
    {
        return scoredPoints;
    }

    public override void Undo()
    {
        discardDeck.AddCard(targetColumn.RemoveCard());
    }

}
