using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelect : MonoBehaviour {

    public Selectable card1;
    public Selectable card2;
    public Selectable card3;
    public Selectable card4;

    private Selectable current;
    private Selectable next;

    // Use this for initialization
    void Start () {

        //Selects a card at the start
        current = card1;
        current.Select();
        
	}
	
	// Update is called once per frame
	void Update () {

        //Select card to the left (wraps if first card)
        if (Input.GetKeyDown(KeyCode.I))
        {
            next = current.FindSelectableOnLeft();
            next.Select();
            current = next;
        }

        //Select card to the right (wraps if last card)
        if (Input.GetKeyDown(KeyCode.O))
        {
            next = current.FindSelectableOnRight();
            next.Select();
            current = next;
        }

    }
}
