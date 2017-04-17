using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card {

    public value cardValue;
    public seed cardSeed;
    public colour cardColour;
    public GameObject cardPrefab;
    public Sprite cardNumber;
    public Sprite cardSuit;
    public Sprite cardPip;
    public bool isFlipped = false;
    public bool isBottomCard = false;

	public enum value {
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

    public enum seed {
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
    public Card (GameObject prefab, value value, seed seed, colour colour, Sprite number, Sprite suit, Sprite pip)
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
    }

    //dice al prefab di raggiungere la posizione target
    public void ReachPosition(Vector2 target)
    {
        cardPrefab.GetComponent<CardPrefab>().ReachPosition(target);
    }

    //ritorna la posizione che il prefab deve avere, non quella attuale!!
    public Vector2 Position()
    {
        return cardPrefab.GetComponent<CardPrefab>().target;
    }

    //setta l'order in layer
    public void SetOrder(int position)
    {
        cardPrefab.GetComponent<SpriteRenderer>().sortingOrder = position;
    }

    //gira la carta
    public void FlipCard()
    {
        cardPrefab.GetComponent<Animator>().SetTrigger("flipCard");
    }


}
