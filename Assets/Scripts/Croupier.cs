using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//cardsDB è una lista di tutte le carte sempre ordinata e potrebbe non essere necessaria (CONTROLLARE)
//deckList è la lista usata per mescolarle
//deck è il mazzo usato in gioco
public class Croupier : MonoBehaviour
{
    bool card3Mode;

    public Card[] inspectrCardsInColumn; //DA CANELLARE!!

    public List<Card> cardsDB;

    public GameObject[] columns;


    public GameObject heartsDeck;
    public GameObject diamondsDeck;
    public GameObject clubsDeck;
    public GameObject spadesDeck;

    public List<Card> deckList;
    public Deck deck;
    public DiscardDeck discardDeck;

    public GameObject cardPrefab;

    public Sprite[] numbersSprites;
    public Sprite[] seedsSprites;

    Stack<Command> commandHistory;

    int score;
    public Text scoreUI;


    // Use this for initialization
    void Start()
    {
        CreateDeck();
        ShuffleDeck();
        DealCards();
        commandHistory = new Stack<Command>();
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
                newCard.requestMove += OnRequestMove; //iscrizione alla classe Card
            }
            //carte nere
            for (int seed = 2; seed < 4; seed++)
            {
                Card newCard = new Card((GameObject)Instantiate(cardPrefab), (Card.value)value,
                    (Card.seed)seed, Card.colour.black, numbersSprites[value], seedsSprites[seed], seedsSprites[seed]);
                cardsDB.Add(newCard);
                newCard.requestMove += OnRequestMove;
            }
        }
        foreach (Card card in cardsDB)
        {
            card.cardPrefab.GetComponent<CardPrefab>().targetPosition = card.cardPrefab.transform.position;
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
        deck.cardsInDeck = new Stack<Card>(deckList);
        InspectorVisualizer(); //ELIMINARE!
    }

    //assegna le carte alle varie liste
    //NON uso la classe Move perchè non sono mosse ripercorribili ma la preparazione del gioco
    public void DealCards()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < i + 1; j++)
            {
                columns[i].GetComponent<Column>().AddCards(deck.cardsInDeck.Pop());
            }
            columns[i].GetComponent<Column>().InspectorVisualizer(); //ELIMINARE!!!
            columns[i].GetComponent<Column>().FlipTopCard();
        }
        InspectorVisualizer(); //ELIMINARE!!
    }

    //istanzia una mossa quando richiesto da una carta (oncarddrop) verificando le regole in base alla column su cui è stata rilasciata
    public void OnRequestMove(Card card, GameObject originObject, GameObject targetObject)
    {
        Column target = targetObject.GetComponent<Column>(); 
        Command newMove = null;
        switch (targetObject.tag)
        {
            case "Column":
                newMove = CheckRulesColumns(Card.value.king, card, originObject, target);
                break;
            case "Hearts":
                newMove = CheckRulesDecks(Card.value.ace, Card.seed.hearts, card, originObject, target);
                break;
            case "Diamonds":
                newMove = CheckRulesDecks(Card.value.ace, Card.seed.diamonds, card, originObject, target);
                break;
            case "Clubs":
                newMove = CheckRulesDecks(Card.value.ace, Card.seed.clubs, card, originObject, target);
                break;
            case "Spades":
                newMove = CheckRulesDecks(Card.value.ace, Card.seed.spades, card, originObject, target);
                break;

        }
        if (newMove != null)
        {
            commandHistory.Push(newMove);
            newMove.Execute();
            score += newMove.GetPoints();
            scoreUI.text = score.ToString(); //segna il punteggio sulla UI
        }
        else
        {
            card.GoBack();
        }
    }

    //Controlla le regole in base ai parametri
    Command CheckRulesColumns(Card.value cardValueOnBottom, Card card, GameObject origin, Column target)
    {
        Card targetTopCard = target.TopCard();
        if (!origin.Equals(target))
        {
            if (targetTopCard != null)
            {
                if (targetTopCard.cardValue == card.cardValue + 1 && targetTopCard.cardColour != card.cardColour)
                {
                    
                    Command newMove = CheckIfFromDiscardDeck(card, origin, target, 5);
                    return newMove;
                }
            }
            else
            {
                if (card.cardValue == cardValueOnBottom)
                {
                    Command newMove = CheckIfFromDiscardDeck(card, origin, target, 5);
                    return newMove;
                }
            }
        }
        return null;
    }

    //verifica le regole in caso la carta sia stata rilasciata su uno dei deck
    Command CheckRulesDecks(Card.value cardValueOnBottom, Card.seed deckSeed, Card card, GameObject origin, Column target)
    {
        Card targetTopCard = target.TopCard();        
        if (!origin.Equals(target) && card.cardSeed == deckSeed)
        {
            if (targetTopCard != null)
            {
                if (targetTopCard.cardValue == card.cardValue - 1)
                {
                    Command newMove = CheckIfFromDiscardDeck(card, origin, target, 15);                  
                    return newMove;
                }
            }
            else
            {
                if (card.cardValue == cardValueOnBottom)
                {
                    Command newMove = CheckIfFromDiscardDeck(card, origin, target, 15);
                    return newMove;
                }
            }
        }
        return null;
    }

    //verifica se la carta è stata pescata e genera la mossa
    Command CheckIfFromDiscardDeck(Card card, GameObject origin, Column target, int points)
    {
        if (origin.tag == "Discard")
        {
            return new TakeCard(card, origin.GetComponent<DiscardDeck>(), target, points);
        }
        else if (origin.tag == "Column")
        {
            return new Move(card, origin.GetComponent<Column>(), target, points);
        }
        else return null;
    }

    //torna indietro di una mossa
    public void UndoMove()
    {
        if (commandHistory.Count > 0)
        {
            Command removedCommand = commandHistory.Pop();
            removedCommand.Undo();
            score -= removedCommand.GetPoints();
            scoreUI.text = score.ToString(); //segna il punteggio sulla UI
        }       
    }
    
    //METODI PER L'UI
    //pesca la/le carta/e
    public void DrawCards()
    {
        if (deck.cardsInDeck.Count > 0)
        {
            if (card3Mode)
            {

            }
            else
            {
                DrawCard newDraw = new DrawCard(deck, discardDeck);
                newDraw.Execute();
                commandHistory.Push(newDraw);
            }
        }
        else
        {
            RebuildDeck();
        }
        
    }

    public void RebuildDeck()
    {
        //deck.AddCards(discardDeck.cardsInColumn);
        discardDeck.cardsInDeck.Clear();
        InspectorVisualizer();
    }

    //DA CANCELLARE!!!!
    public void InspectorVisualizer()
    {
        inspectrCardsInColumn = deck.cardsInDeck.ToArray();
    }

}
