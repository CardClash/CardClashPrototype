﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour {

    //Selectable array position, card id
    private Dictionary<int, int> hand;
    
    private CardEffects effects;
    public int[] keyList;
    public int[] cardList;
    public static System.Random rand;

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

        // Swap(cardList, 1, 2);
        //rand = new System.Random();
        //float test = rand.Next(10);


        
        



        effects = GetComponent<CardEffects>();
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

}
