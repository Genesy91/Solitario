using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//    COMMAND PATTERN
//un'istanza di questa classe viene creata ogni volta che viene eseguita una "mossa" spostando le carte o pescandole
//la classe contiene dei parametri che identificano la mossa e il metodo che la attua
//ogni istanza della classe viene poi messa in una lista che se viene visitata al contrario permette di annullare le mosse
//infatti la classe contiene anche un metodo per eseguire la mossa inversa
public class Move : Command
{

    Card card;
    Column originColumn;
    Column targetColumn;
    int numberOfCards;
    bool wasFlipped;
    int scoredPoints;

    public Move(Card card, Column origin, Column target, int points)
    {
        originColumn = origin;
        targetColumn = target;
        scoredPoints = points;
        this.card = card;
        numberOfCards = 0;
    }

    //esegue la mossa
    public override void Execute()
    {
        Stack<Card> tempStack = new Stack<Card>();
        do
        {
            tempStack.Push(originColumn.RemoveCard());
        } while (!tempStack.Peek().Equals(card));

        while (tempStack.Count > 0)
        {
            targetColumn.AddCards(tempStack.Pop());
        }
    }

    //annulla la mossa
    public override void Undo()
    {
        Stack<Card> tempStack = new Stack<Card>();
        do
        {
            tempStack.Push(targetColumn.RemoveCard());
        } while (!tempStack.Peek().Equals(card));
        //originColumn.AddCards(tempStack);
    }

    //serve per permettere al croupier di sottrarre i punti
    public override int GetPoints()
    {
        return scoredPoints;
    }

    
}
