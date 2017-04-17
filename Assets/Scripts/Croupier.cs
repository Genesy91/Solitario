using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Croupier : MonoBehaviour
{
    public List<Card> cardsDB;

    //public List<Card> column1;
    //public List<Card> column2;
    //public List<Card> column3;
    //public List<Card> column4;
    //public List<Card> column5;
    //public List<Card> column6;
    //public List<Card> column7;
    //List<Card>[] columns;

    public GameObject column1;
    public GameObject column2;
    public GameObject column3;
    public GameObject column4;
    public GameObject column5;
    public GameObject column6;
    public GameObject column7;
    GameObject[] columns;


    public List<Card> heartsDeck;
    public List<Card> diamondsDeck;
    public List<Card> clubsDeck;
    public List<Card> spadesDeck;

    public List<Card> deck;
    public List<Card> flippedCards;

    public GameObject cardPrefab;

    public Sprite[] numbersSprites;
    public Sprite[] seedsSprites;

    // Use this for initialization
    void Start()
    {
        CreateDeck();
        ShuffleDeck();
        //columns = new List<Card>[7] { column1, column2, column3, column4, column5, column6, column7 };
        columns = new GameObject[7] { column1, column2, column3, column4, column5, column6, column7 };
        DealCards();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //crea il mazzo di carte ordinato
    public void CreateDeck()
    {
        for (int value = 0; value < 13; value++)
        {
            //carte rosse
            for (int seed = 0; seed < 2; seed++)
            {
                Card newCard = new Card((GameObject)Instantiate(cardPrefab), (Card.value)value, 
                    (Card.seed)seed, Card.colour.red, numbersSprites[value], seedsSprites[seed], seedsSprites[seed]);
                cardsDB.Add(newCard);
            }
            //carte nere
            for (int seed = 2; seed < 4; seed++)
            {
                Card newCard = new Card((GameObject)Instantiate(cardPrefab), (Card.value)value,
                    (Card.seed)seed, Card.colour.black, numbersSprites[value], seedsSprites[seed], seedsSprites[seed]);
                cardsDB.Add(newCard);
            }
        }
    }

    //mescola le carte e popola il mazzo vero e proprio
    public void ShuffleDeck()
    {
        deck.AddRange(cardsDB);
        int i = deck.Count - 1;
        while (i > 0)
        {
            Card tempCard = deck[i];
            int k = Random.Range(0, i);
            deck[i] = deck[k];
            deck[k] = tempCard;
            i--;
        }
    }

    //assegna le carte alle varie liste
    public void DealCards()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < i+1; j++)
            {
                columns[i].GetComponent<Column>().AddCard(deck[0]);
                deck.RemoveAt(0);
            }
            columns[i].GetComponent<Column>().InspectorVisualizer(); //eliminare!!!
            columns[i].GetComponent<Column>().FlipTopCard();
        }
    }
}
