using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

    public Stack<Card> cardsInDeck;
    public bool isEmpty;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Card TopCard()
    {
        if (cardsInDeck.Count > 0)
        {
            return cardsInDeck.Peek();
        }
        else
        {
            return null;
        }
    }

    public void AddCard (Card newCard)
    {
        cardsInDeck.Push(newCard);
        isEmpty = false;
        newCard.cardPrefab.transform.SetParent(null);
        if (newCard.isFlipped)
        {
            newCard.FlipCard();
        }
        newCard.ReachPosition(transform.position);
        newCard.SetOrder(cardsInDeck.Count);

    }

    public Card RemoveCard()
    {
        
        if (cardsInDeck.Count == 0)
        {
            isEmpty = true;
            return null;
        }
        Card tempCard = cardsInDeck.Pop();
        return tempCard;
    }
}
