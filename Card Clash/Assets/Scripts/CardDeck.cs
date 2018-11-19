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
    public Queue<int> discardPile;

    public float timeStopTimer = 0.0f;

    // Use this for initialization
    void Start()
    {
        int[] deck = MyDeckOfCards.deck;
        int deckSize = 0;
        for (int i = 0; i < deck.Length; i++)
        {
            print(deck[i]);
            deckSize += deck[i];
        }
        cardList = new int[deckSize];
        //for (int i = 0; i < cardList.Length; i++)
        //{
        //    for (int j = 0; j < deck[i]; j++)
        //    {

        //    }
        //}
        int count = 0;
        for (int i = 0; i < deck.Length; i++)
        {
            for (int j = 0; j < deck[i]; j++)
            {
                cardList[count] = i + 10;
                ++count;
                print(i + 10);
            }
        }

        //cardList = new int[6]
        //{
        //    10,
        //    11,
        //    12,
        //    13,
        //    //14,
        //    15
        //    ,
        //    16
        //};

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

        for (int i = 4; i < cardList.Length; i++)
        {
            unusedCards.Enqueue(cardList[i]);
        }

        discardPile = new Queue<int>(unusedCards.Count);

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
                //Debug.Log(hand[key]);
                
                if (effects.played)
                {
                    //print(hand[key]);
                    effects.SetCardID(hand[key] - 10);
                    effects.TimeStop();
                    //print(unusedCards.Peek());
                    if (unusedCards.Count <= 0)
                    {
                        int[] shuffleMe = new int[discardPile.Count];
                        int iteration = discardPile.Count;
                        for (int i = 0; i < iteration; i++)
                        {
                            shuffleMe[i] = discardPile.Dequeue();
                        }
                        Shuffle(shuffleMe);
                        for (int i = 0; i < shuffleMe.Length; i++)
                        {
                            unusedCards.Enqueue(shuffleMe[i]);
                        }
                    }
                    discardPile.Enqueue(hand[key]);
                    hand[key] = (int)unusedCards.Dequeue();
                }
                
                break;
            }
        }
    }

    

    public static void Shuffle(int[] a)
    {
        print("shuffle");
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
