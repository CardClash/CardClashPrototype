using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private int lives;
    private int damage;
    private int mana;
    private int maxMana;
    private GameObject myCharacter;
    private DeckScript myDeck;
    private List<GameObject> myHand;
    private List<GameObject> myGraveyard;

    public int Lives
    {
        get { return lives; }
        set { lives = value; }
    }

    public int Damage
    {
        get { return damage; }
        set
        {
            if (value < 0)
            {
                damage = 0;
            }
            else if (value > 999)
            {
                damage = 999;
            }
            else
            {
                damage = value;
            }
        }
    }

    public GameObject Character
    {
        get { return myCharacter; }
        set { myCharacter = value; }
    }

    public DeckScript Deck
    {
        get { return myDeck; }
        set { myDeck = value; }
    }

    public List<GameObject> Hand
    {
        get { return myHand; }
        set { myHand = value; }
    }

    public List<GameObject> Graveyard
    {
        get { return myGraveyard; }
        set { myGraveyard = value; }
    }
}