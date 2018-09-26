using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelect : MonoBehaviour {

    public Selectable card1;
    public Selectable card2;
    public Selectable card3;
    public Selectable card4;

    private CardDeck cardDeck;

    private int current;
    private int next;
    private Selectable[] select;


    // Use this for initialization
    void Start () {
        select = new Selectable[] { card1, card2, card3, card4 };
        //Selects a card at the start       
        current = 0;
        select[0].Select();
        
        cardDeck = GetComponent<CardDeck>();
	}
	
	// Update is called once per frame
	void Update () {

        //Highlight card to the left (wraps if first card)
        if (Input.GetButtonDown("Switch Card Left"))
        {
            next = current - 1;
            if(next < 0 )
            {
                next = 3;
            }
            select[next].Select();
            current = next;
        }

        //Highlight card to the right (wraps if last card)
        if (Input.GetButtonDown("Switch Card Right"))
        {
            next = current + 1;
            if (next > 3)
            {
                next = 0;
            }
            select[next].Select();
            current = next;
        }

        //Select Card
        if (Input.GetButtonDown("Use Card"))
        {
            Debug.Log("Using Card! " + current);
            cardDeck.CardPick(current);
        }

      }


}
