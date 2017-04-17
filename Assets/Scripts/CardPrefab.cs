using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPrefab : MonoBehaviour
{

    public float speed;
    public bool isMovingToPosition = false;
    public Vector2 target; //serve per muoversi ma memorizza anche la posizione
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingToPosition == true)
        {
            MoveToPosition();
        }
    }


    //sposta la carta fino alla posizione corretta
    void MoveToPosition()
    {
        float delta = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, delta);
        if ((Vector2)transform.position == target)
        {
            isMovingToPosition = false;
        }
    }

    //verifica che la carta sia nella posizione corretta e in caso negativo da il permesso di spostarsi
    public void ReachPosition(Vector2 target)
    {
        this.target = target;
        if ((Vector2) transform.position != target)
        {
            isMovingToPosition = true;
            
        }
    }

}
