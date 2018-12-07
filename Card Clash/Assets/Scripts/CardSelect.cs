using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelect : MonoBehaviour {

    public Selectable card1;
    public Selectable card2;
    public Selectable card3;
    public Selectable card4;
    public Selectable[] backs;
    public Sprite[] cardArt;
    public Sprite template;
    public Text[] manaCostTexts;
    public Text[] cardNames;
    public Text[] cardTexts;
    public float manaCostTextHeight;
    public float cardNamesTextHeight;
    public float cardTextsTextHeight;

    private CardDeck cardDeck;
    private CardEffects cardEffects;

    private int current;
    private int next;
    private Selectable[] select;

    private int[] cardList;

    // Use this for initialization
    void Start()
    {
        select = new Selectable[] { card1, card2, card3, card4 };
        //Selects a card at the start       
        current = 0;
        select[0].Select();
        
        cardDeck = GetComponent<CardDeck>();
        cardEffects = GetComponent<CardEffects>();

        cardList = new int[4];

        foreach (Text txt in manaCostTexts)
        {
            txt.text = "";
            RectTransform rect = txt.GetComponent<RectTransform>();
            rect.anchoredPosition = (new Vector3(rect.anchoredPosition.x, manaCostTextHeight, rect.position.z));
        }

        foreach (Text txt in cardNames)
        {
            txt.text = "";
            RectTransform rect = txt.GetComponent<RectTransform>();
            rect.anchoredPosition = (new Vector3(rect.anchoredPosition.x, cardNamesTextHeight, rect.position.z));
        }

        foreach (Text txt in cardTexts)
        {
            txt.text = "";
            RectTransform rect = txt.GetComponent<RectTransform>();
            rect.anchoredPosition = (new Vector3(rect.anchoredPosition.x, cardTextsTextHeight, rect.position.z));
        }

        PopulateManaCosts();
        PopulateCardNames();
        PopulateCardText();
    }
	
	// Update is called once per frame
	void Update()
    {
        //Update card sprites
        ShownCards();

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
            //Debug.Log("Using Card! " + current);
            cardDeck.CardPick(current);
            
        }

        PopulateManaCosts();
        PopulateCardNames();
        PopulateCardText();
    }


    private void ShownCards()
    {
        if (!cardDeck)
        {
            return;
        }

        //Gets ID's of cards in hand in order
        cardList = cardDeck.GetHand();

        for (int i = 0; i < cardList.Length; i++)
        {
            cardList[i] = cardList[i] - 10;
        }

        foreach (Selectable back in backs)
        {
            back.GetComponent<Image>().sprite = template;
        }

        //Changes sprite on card to corresponding image
        select[0].GetComponent<Image>().sprite = cardArt[cardList[0]];
        select[1].GetComponent<Image>().sprite = cardArt[cardList[1]];
        select[2].GetComponent<Image>().sprite = cardArt[cardList[2]];
        select[3].GetComponent<Image>().sprite = cardArt[cardList[3]];
    }

    private void PopulateManaCosts()
    {
        if (cardEffects.ManaCosts != null)
        {
            for (int i = 0; i < manaCostTexts.Length; i++)
            {
                manaCostTexts[i].text = cardEffects.ManaCosts[cardDeck.GetHand()[i]];
            }
        }
    }

    private void PopulateCardNames()
    {
        if (cardEffects.CardNames != null)
        {
            for (int i = 0; i < cardNames.Length; i++)
            {
                cardNames[i].text = cardEffects.CardNames[cardDeck.GetHand()[i]];
            }
        }
    }

    private void PopulateCardText()
    {
        if (cardEffects.CardTexts != null)
        {
            for (int i = 0; i < cardTexts.Length; i++)
            {
                cardTexts[i].text = cardEffects.CardTexts[cardDeck.GetHand()[i]];
            }
        }
    }

}
