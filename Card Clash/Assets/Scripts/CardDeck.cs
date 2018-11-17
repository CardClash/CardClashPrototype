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

    public float timeStopTimer = 0.0f;

    // Use this for initialization
    void Start()
    {

        cardList = new int[7]
        {
            10,
            11,
            12,
            13,
            14,
            15,
            16
        };

        keyList = new int[4]
        {
            0,
            1,
            2,
            3
        };

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
        //unusedCards.Enqueue(cardList[5]);

        //print(unusedCards.Peek());

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
                
                if (effects.played)
                {
                    effects.TimeStop();
                    unusedCards.Enqueue(hand[key]);
                    //print(unusedCards.Peek());
                    hand[key] = (int)unusedCards.Dequeue();
                }
                
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
