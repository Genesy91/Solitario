using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCard : Command
{
    Deck deck;
    DiscardDeck discardDeck;

    public DrawCard (Deck mainDeck, DiscardDeck discard)
    {
        deck = mainDeck;
        discardDeck = discard;
    }

    public override void Execute()
    {
        discardDeck.AddCard(deck.RemoveCard());
    }

    public override int GetPoints()
    {
        return 0;
    }

    public override void Undo()
    {
        deck.AddCard(discardDeck.RemoveCard());
    }
}
