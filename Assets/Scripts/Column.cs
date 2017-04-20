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
        
        //if (gameObject.tag == "Deck" || gameObject.tag == "Discard")  //nel caso si stiano pescando carte dal mazzo o stiano ritornando ad esso (annullamento di una mossa)
        //{
        //    newCard.FlipCard();
        //}
        InspectorVisualizer(); //ELIMINARE!!
    }

    ////aggiunge un insieme di carte in cima, sia nella lista logica che spostando i prefab nella column fisica
    //public void AddCards(Stack<Card> newCards)
    //{
    //    bottomCardPosition = (Vector2)transform.position - new Vector2(0f, bottomCardYOffset);
    //    Card firstCard = newCards.Peek();
    //    if (cardsInColumn.Count == 0)
    //    {
    //        firstCard.ReachPosition(bottomCardPosition);
    //        firstCard.isBottomCard = true;
    //        firstCard.cardPrefab.transform.SetParent(null);
    //        print("count0");
    //    }
    //    else
    //    {
    //        Card topCard = TopCard(); //si posiziona a distanze diverse a seconda dell'orientamento della carta in cima
    //        if (topCard.isFlipped)
    //        {
    //            firstCard.ReachPosition(topCard.TargetPosition() - new Vector2(0f, flippedCardYOffset));
    //            //firstCard.cardPrefab.transform.SetParent(topCard.cardPrefab.transform, true); //serve per farle muovere insieme
    //        }
    //        else
    //        {
    //            firstCard.ReachPosition(topCard.TargetPosition() - new Vector2(0f, unflippedCardYOffset));
    //        }
    //        firstCard.cardPrefab.transform.SetParent(TopCard().cardPrefab.transform, true);
    //    }
    //    while (newCards.Count != 0)
    //    {
    //        cardsInColumn.Push(newCards.Pop()); //aggiunge allo stack
    //        TopCard().SetOrigin(this);
    //        TopCard().SetOrder(cardsInColumn.Count);

    //        if (gameObject.tag == "Discard" && !TopCard().isFlipped) //nel caso si stiano pescando carte dal mazzo o stiano ritornando ad esso (annullamento di una mossa)
    //        {
    //            TopCard().FlipCard();
    //        }
    //        if (gameObject.tag == "Deck") //nel caso si stiano pescando carte dal mazzo o stiano ritornando ad esso (annullamento di una mossa)
    //        {
    //            TopCard().FlipCard();
    //        }
    //    }
    //    InspectorVisualizer(); //ELIMINARE!!
    //}



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
