using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//cardsDB è una lista di tutte le carte sempre ordinata e potrebbe non essere necessaria (CONTROLLARE)
//deckList è la lista usata per mescolarle
//deck è il mazzo usato in gioco
public class Croupier : MonoBehaviour
{
    public Card[] inspectrCardsInColumn; //DA CANELLARE!!

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


    public GameObject heartsDeck;
    public GameObject diamondsDeck;
    public GameObject clubsDeck;
    public GameObject spadesDeck;

    public List<Card> deckList;
    public Column deck;
    public Stack<Card> flippedCards;

    public GameObject cardPrefab;

    public Sprite[] numbersSprites;
    public Sprite[] seedsSprites;

    // Use this for initialization
    void Start()
    {
        CreateDeck();
        ShuffleDeck();
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
        deckList.AddRange(cardsDB);
        int i = deckList.Count - 1;
        while (i > 0)
        {
            Card tempCard = deckList[i];
            int k = Random.Range(0, i);
            deckList[i] = deckList[k];
            deckList[k] = tempCard;
            i--;
        }
        deck.cardsInColumn = new Stack<Card>(deckList);
    }

    //assegna le carte alle varie liste
    //NON uso la classe Move perchè non sono mosse ripercorribili ma la preparazione del gioco
    public void DealCards()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < i+1; j++)
            {
                columns[i].GetComponent<Column>().AddCard(deck.cardsInColumn.Pop());
            }
            columns[i].GetComponent<Column>().InspectorVisualizer(); //ELIMINARE!!!
            columns[i].GetComponent<Column>().FlipTopCard();
        }
        deck.InspectorVisualizer(); //ELIMINARE!!
    }


    //DA CANCELLARE!!!!
    public void InspectorVisualizer()
    {
        inspectrCardsInColumn = deck.cardsInColumn.ToArray();
    }
}
