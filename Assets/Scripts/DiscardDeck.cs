using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardDeck : MonoBehaviour {

    public List<Card> cardsInDeck;
    public bool isEmpty;
    public Vector2 xOffset;

    // Use this for initialization
    void Start()
    {
        cardsInDeck = new List<Card>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Card TopCard()
    {
        if (cardsInDeck.Count > 0)
        {
            return cardsInDeck[cardsInDeck.Count - 1];
        }
        else
        {
            return null;
        }
    }

    public void AddCard(Card newCard)
    {
        cardsInDeck.Add(newCard);
        isEmpty = false;
        newCard.cardPrefab.transform.SetParent(null);
        if (!newCard.isFlipped)
        {
            newCard.FlipCard();
        }
        newCard.ReachPosition(transform.position);
        CheckPositions();
        newCard.SetOrigin(gameObject);
    }

    public Card RemoveCard()
    {
        if (cardsInDeck.Count == 0)
        {
            isEmpty = true;
            return null;
        }
        Card tempCard = cardsInDeck[cardsInDeck.Count - 1];
        cardsInDeck.RemoveAt(cardsInDeck.Count - 1);
        CheckPositions();
        return tempCard;
    }

    //sposta le carte in modo da mostrarne 3
    public void CheckPositions()
    {
        int i = 0;
        while (i < 3 && cardsInDeck.Count - 1 - i >= 0)
        {
            Vector2 pos = transform.position;
            cardsInDeck[cardsInDeck.Count - 1 - i].ReachPosition(pos - i*xOffset);
            cardsInDeck[cardsInDeck.Count - 1 - i].SetOrder(cardsInDeck.Count - i);
            i++;
        }
    }
}
