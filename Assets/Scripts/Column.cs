using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour {

    //forse è meglio avere una variabile che punta sempre alla TopCard

    public Stack<Card> cardsInColumn;     //la testa della lista è quella più in fondo
    public Card[] inspectrCardsInColumn; //da cancellare! serve per vederla nell'inspector

    Vector2 bottomCardPosition;
    public float bottomCardYOffset;
    public float unflippedCardYOffset;
    public float flippedCardYOffset;

    // Use this for initialization
    void Start () {
        cardsInColumn = new Stack<Card>();
        bottomCardPosition = (Vector2)transform.position - new Vector2(0f, bottomCardYOffset);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //aggiunge una carta in cima sia nella lista logica che spostando i prefab
    public void AddCard(Card newCard)
    {
        
        if (cardsInColumn.Count == 0)
        {
            cardsInColumn.Push(newCard); //aggiunge allo stack
            newCard.SetOrder(cardsInColumn.Count);
            newCard.ReachPosition(bottomCardPosition);
            newCard.isBottomCard = true;
        }
        else
        {
            Card topCard = TopCard();
            if (topCard.isFlipped)
            {
                newCard.ReachPosition(topCard.Position() - new Vector2(0f, flippedCardYOffset));
            }
            else
            {
                //print(topCard.Postion());
                print(TopCard().Position());
                newCard.ReachPosition(topCard.Position() - new Vector2(0f, unflippedCardYOffset));
            }
            cardsInColumn.Push(newCard); //aggiunge allo stack
            newCard.SetOrder(cardsInColumn.Count);
        }        
    }

    //rimuove la carta in cima e la ritorna
    public Card RemoveCard()
    {
        return cardsInColumn.Pop();
    }

    //ritorna la carta in cima
    public Card TopCard()
    {
        return cardsInColumn.Peek();
    }

    //da cancellare
    public void InspectorVisualizer()
    {
        inspectrCardsInColumn = cardsInColumn.ToArray();
    }

    //gira la carta più in alto se non è girata
    public void FlipTopCard()
    {
        Card topCard = TopCard();
        if (topCard.isFlipped == false)
        {
            topCard.FlipCard();
        }
    }
}
