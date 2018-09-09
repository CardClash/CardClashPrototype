using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelect : MonoBehaviour {

    public Selectable card1;
    public Selectable card2;
    public Selectable card3;
    public Selectable card4;

    private int current;
    private int next;
    private Selectable[] cards;

    // Use this for initialization
    void Start () {

        cards = new Selectable[] { card1, card2, card3, card4 };
        //Selects a card at the start       
        current = 0;
        cards[0].Select();
	}
	
	// Update is called once per frame
	void Update () {

        //Select card to the left (wraps if first card)
        if (Input.GetKeyDown(KeyCode.I))
        {
            next = current - 1;
            if(next < 0 )
            {
                next = 3;
            }
            cards[next].Select();
            current = next;
        }

        //Select card to the right (wraps if last card)
        if (Input.GetKeyDown(KeyCode.O))
        {
            next = current + 1;
            if (next > 3)
            {
                next = 0;
            }
            cards[next].Select();
            current = next;
        }

    }
}
