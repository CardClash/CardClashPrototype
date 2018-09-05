using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckScript : MonoBehaviour
{
    private List<GameObject> deck;

    public void Shuffle()
    {
        List<GameObject> nextDeck = new List<GameObject>();
        System.Random rand = new System.Random();
        while (deck.Count > 0)
        {
            int index = rand.Next(deck.Count);
            nextDeck.Add(deck[index]);
            deck.RemoveAt(index);
        }
        deck = nextDeck;
    }

    private GameObject Draw()
    {
        GameObject topDeck = deck[0];
        deck.RemoveAt(0);
        return topDeck;
    }

    public List<GameObject> Draw(int count)
    {
        List<GameObject> topDecks = new List<GameObject>(count);
        for (int i = 0; i < count; i++)
        {
            topDecks.Add(Draw());
        }
        return topDecks;
    }
}