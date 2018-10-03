using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour {

    //Selectable array position, card id
    private Dictionary<int, int> hand;
    public int[] handList;
    
    private CardEffects effects;
    public int[] keyList;
    public int[] cardList;
    public static System.Random rand;
    public Queue unusedCards;

    // Use this for initialization
    void Start () {

        cardList = new int[6];
        cardList[0] = 10;
        cardList[1] = 11;
        cardList[2] = 12;
        cardList[3] = 13;
        cardList[4] = 14;
        cardList[5] = 15;

        keyList = new int[4];
        keyList[0] = 0;
        keyList[1] = 1;
        keyList[2] = 2;
        keyList[3] = 3;

        //Shuffle the deck
        Shuffle(cardList);

        //Currently held cards
        hand = new Dictionary<int, int>
        {
            { 0, cardList[0] },
            { 1, cardList[1] },
            { 2, cardList[2] },
            { 3, cardList[3] }
        };

        unusedCards = new Queue();

        unusedCards.Enqueue(cardList[4]);
        unusedCards.Enqueue(cardList[5]);

        print(unusedCards.Peek());

        effects = GetComponent<CardEffects>();

        handList = new int[4];
    }

    //When card is selected (Used in CardSelect)
    public void CardPick(int current)
    {
        
        foreach(int key in keyList)
        {
            
            if (key == current)
            {
                
                effects.PlayCard(hand[key]);
                Debug.Log(hand[key]);
                unusedCards.Enqueue(hand[key]);
                //print(unusedCards.Peek());
                hand[key] = (int)unusedCards.Dequeue();
                break;
            }
        }
    }

    public static void Shuffle(int[] a)
    {
        int n = a.Length;
        rand = new System.Random();

        for (int i = 0; i < a.Length; i++)
        {
            Swap(a, i, i + rand.Next(n-i));
        }
    }

    public static void Swap(int[] arr, int a, int b)
    {
        int temp = arr[a];
        arr[a] = arr[b];
        arr[b] = temp; 
    }

    //Lets CardSelect know the ID of the cards in hand
    public int[] GetHand()
    {
        handList[0] = hand[0];
        handList[1] = hand[1];
        handList[2] = hand[2];
        handList[3] = hand[3];

        return handList;
    }

}
