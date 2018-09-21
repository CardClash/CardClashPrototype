using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour {

    //Selectable array position, card id
    private Dictionary<int, int> hand;
    
    private CardEffects effects;
    public int[] keyList;

    // Use this for initialization
    void Start () {

        keyList = new int[4];
        keyList[0] = 0;
        keyList[1] = 1;
        keyList[2] = 2;
        keyList[3] = 3;

        //Currently held cards
        hand = new Dictionary<int, int>
        {
            { 0, 10 },
            { 1, 11 },
            { 2, 12 },
            { 3, 13 }
        };



        effects = GetComponent<CardEffects>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    //When card is selected (Used in CardSelect)
    public void CardPick(int current)
    {
        
        foreach(int key in keyList)
        {
            
            if (key == current)
            {
                
                effects.PlayCard(hand[key]);
                break;
            }
        }
    }

}
