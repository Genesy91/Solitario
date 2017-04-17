using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//    COMMAND PATTERN
//un'istanza di questa classe viene creata ogni volta che viene eseguita una "mossa" spostando le carte o pescandole
//la classe contiene dei parametri che identificano la mossa e il metodo che la attua
//ogni istanza della classe viene poi messa in una lista che se viene visitata al contrario permette di annullare le mosse
//infatti la classe contiene anche un metodo per eseguire la mossa inversa
public class Move {

    Card card;
    Column originColumn;
    Column targetColumn;

    public Move (Card movedCard, Column origin, Column target)
    {
        card = movedCard;
        originColumn = origin;
        targetColumn = target;

        Execute();
    }

    //esegue la mossa
    public void Execute()
    {
        targetColumn.AddCard(card);
        originColumn.RemoveCard();
    }

    //annulla la mossa
    public void Undo()
    {
        originColumn.AddCard(card);
        targetColumn.RemoveCard();
    }
}
