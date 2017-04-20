using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPrefab : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public float speed;
    public bool isMovingToPosition = false;
    public Vector2 targetPosition; //serve per muoversi ma memorizza anche la posizione
    bool canCheckColumn = false; //permette di verificare in che column si trova la carta
    public bool canBeDrag = false;

    //servono per notificare la classe carta quando il prefab viene rilasciato su una column
    public delegate void OnDrop(GameObject targetColumn);
    public event OnDrop onDrop;
    public int orderInLayer;
    public SpriteRenderer[] childrenSprites;

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
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);
        if ((Vector2)transform.position == targetPosition)
        {
            isMovingToPosition = false;
        }
    }

    //verifica che la carta sia nella posizione corretta e in caso negativo da il permesso di spostarsi
    public void ReachPosition(Vector2 target)
    {
        this.targetPosition = target;
        if ((Vector2)transform.position != target)
        {
            isMovingToPosition = true;
        }
    }

    public void SetOrderInLayer (int order)
    {
        GetComponent<SpriteRenderer>().sortingOrder = order;
        foreach (SpriteRenderer sprite in childrenSprites)
        {
            sprite.sortingOrder = order;
        }
    }

    //fa in modo che mentre viene trascinata la carta si trovi sopra a tutte le altre carte
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canBeDrag)
        {
            orderInLayer = GetComponent<SpriteRenderer>().sortingOrder;
            SetOrderInLayer(20);
        }        
    }
    //quando il prefab viene trascinato
    public void OnDrag(PointerEventData eventData)
    {
        //AGGIUNGI il controllo per il touch!!
        if (canBeDrag)
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(eventData.position);
            EventSystem.current.SetSelectedGameObject(gameObject);
        }        
    }
    //verifica se è stata rilasciata su una column
    public void OnEndDrag(PointerEventData eventData) 
    {
        if (canBeDrag)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero, Mathf.Infinity);
            for (int i = 0; i < hits.Length; i++)
            {
                Collider2D hit = hits[i].collider;
                if (hit.gameObject.layer == 8)                            //il layer 8 è quello delle column
                {
                    onDrop(hit.gameObject);
                }
            }
        }
        onDrop(null);

    }

}
