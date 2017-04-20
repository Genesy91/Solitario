using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Classe Card con le principali caratteristiche di una carta. Contiene il prefab e i metodi per gestirla
[System.Serializable]
public class Card
{

    public value cardValue;
    public seed cardSeed;
    public colour cardColour;
    public GameObject cardPrefab;
    public Sprite cardNumber;
    public Sprite cardSuit;
    public Sprite cardPip;
    public bool isFlipped = false;
    public bool isBottomCard = false;
    GameObject origin;

    public delegate void RequestMove(Card card, GameObject origin, GameObject target);
    public event RequestMove requestMove;

    public enum value
    {
        ace,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
        ten,
        jack,
        queen,
        king
    }

    public enum seed
    {
        hearts,
        diamonds,
        clubs,
        spades
    }

    public enum colour
    {
        black,
        red
    }

    //quando una carta viene istanziata, le viene assegnato un prefab neutro e lo personalizza in base ai parametri ricevuti
    public Card(GameObject prefab, value value, seed seed, colour colour, Sprite number, Sprite suit, Sprite pip)
    {
        cardPrefab = prefab;
        cardValue = value;
        cardSeed = seed;
        cardColour = colour;
        cardNumber = number;
        cardSuit = suit;
        cardPip = pip;
        cardPrefab.transform.Find("Number").GetComponent<SpriteRenderer>().sprite = number;
        cardPrefab.transform.Find("Suit").GetComponent<SpriteRenderer>().sprite = suit;
        cardPrefab.transform.Find("Pip").GetComponent<SpriteRenderer>().sprite = pip;
        cardPrefab.GetComponent<CardPrefab>().onDrop += OnCardDrop; //iscrizione all'altra classe
    }

    //dice al prefab di raggiungere la posizione target
    public void ReachPosition(Vector2 target)
    {
        cardPrefab.GetComponent<CardPrefab>().ReachPosition(target);
    }

    public void GoBack()
    {
        CardPrefab prefab = cardPrefab.GetComponent<CardPrefab>();
        cardPrefab.transform.position = prefab.targetPosition;
        SetOrder(prefab.orderInLayer);
    }

    //ritorna la posizione che il prefab deve avere, non quella attuale!!
    public Vector2 TargetPosition()
    {
        return cardPrefab.GetComponent<CardPrefab>().targetPosition;
    }

    //setta l'order in layer
    public void SetOrder(int position)
    {
        cardPrefab.GetComponent<CardPrefab>().SetOrderInLayer(position);
    }

    //gira la carta
    public void FlipCard()
    {
        cardPrefab.GetComponent<Animator>().SetTrigger("flipCard");
        if (isFlipped)
        {
            isFlipped = false;
            cardPrefab.GetComponent<CardPrefab>().canBeDrag = false;
        }
        else
        {
            isFlipped = true;
            cardPrefab.GetComponent<CardPrefab>().canBeDrag = true;
        }
    }

    //setta la column da cui parte
    public void SetOrigin(GameObject originColumn)
    {
        origin = originColumn;
    }

    //notifica il croupier della mossa eseguita o annulla tutto in caso di di mossa a vuoto (carta rilasciata su nessuna column)
    public void OnCardDrop(GameObject targetColumn)
    {
        if (requestMove != null)
        {
            if (targetColumn == null)
            {
                GoBack();
            }
            else
            {
                requestMove(this, origin, targetColumn);   //richiama il listener nel croupier passandogli se stesso, 
            }                                                                     //la column di partenza e quella di arrivo
        }
    }
}
