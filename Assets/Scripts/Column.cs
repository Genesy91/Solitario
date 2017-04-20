using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour {

    //forse è meglio avere una variabile che punta sempre alla TopCard

    public Stack<Card> cardsInColumn = new Stack<Card>(13);     //la testa della lista è quella più in fondo

    public Card[] inspectrCardsInColumn; //da CANCELLARE! serve per vederla nell'inspector

    Vector2 bottomCardPosition;
    public float bottomCardYOffset;
    public float unflippedCardYOffset;
    public float flippedCardYOffset;

    public int cardCounter = 0;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

	}

    //aggiunge una carta in cima, sia nella lista logica che spostando il prefab nella column fisica
    public void AddCards(Card newCard)
    {
        
        if (cardsInColumn.Count == 0)                                                            //nel caso sia la prima della column
        {
            bottomCardPosition = (Vector2)transform.position - new Vector2(0f, bottomCardYOffset);
            newCard.ReachPosition(bottomCardPosition);
            newCard.isBottomCard = true;
            newCard.cardPrefab.transform.SetParent(null);
        }
        else
        {
            Card topCard = TopCard();
            if (gameObject.tag == "Column" && topCard.isFlipped && topCard.cardValue != newCard.cardValue + 1)    //serve quando con un Undo una carta torna su una posizione            
            {                                                                                                         //nella quale precedentemente c'era una carta coperta
                topCard.FlipCard();
            }
            if (topCard.isFlipped)                                                                //verifica se la carta su cui viene rilasciata e girata oppure no
            {
                newCard.ReachPosition(topCard.TargetPosition() - new Vector2(0f, flippedCardYOffset));
                newCard.cardPrefab.transform.SetParent(topCard.cardPrefab.transform, true);       //serve per farle muovere insieme
            }
            else
            {
                newCard.ReachPosition(topCard.TargetPosition() - new Vector2(0f, unflippedCardYOffset));
            }
            
        }
        cardsInColumn.Push(newCard);                                  //aggiunge allo stack
        newCard.SetOrigin(gameObject);
        cardCounter++;
        newCard.SetOrder(cardCounter);                           //order in layer ....BUG?
        
    }


    //rimuove la carta in cima e la ritorna
    public Card RemoveCard()
    {
        Card tempCard = cardsInColumn.Pop();
        if (cardsInColumn.Count > 0)
        {
            FlipTopCard();
            tempCard.isBottomCard = false;
        }
        InspectorVisualizer(); //ELIMINARE!!
        tempCard.SetOrder(20);
        cardCounter--;
        return tempCard;
    }

    //ritorna la carta in cima
    public Card TopCard()
    {
        if (cardsInColumn.Count > 0)
        {
            return cardsInColumn.Peek();
        }
        else
        {
            return null;
        }
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




    //DA CANCELLARE!!!!
    public void InspectorVisualizer()
    {
        inspectrCardsInColumn = cardsInColumn.ToArray();
    }

}
