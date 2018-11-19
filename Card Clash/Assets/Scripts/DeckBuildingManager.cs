using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckBuildingManager : MonoBehaviour {

    public float leftOffset;
    public float upOffset;
    public float spacingRight;
    public float spacingDown;
    public float buttonHeight;
    public string[] cardNames;
    public float smallWidth;
    public int upperLimit;

    private int[] cardNums;
    private int total;

	// Use this for initialization
	void Start ()
    {
        cardNums = new int[cardNames.Length];
        for (int i = 0; i < cardNums.Length; i++)
        {
            cardNums[i] = 0;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnGUI()
    {
        for (int i = 0; i < cardNames.Length; i++)
        {
            GUI.Button(new Rect(leftOffset, upOffset + (spacingDown * i), 200, buttonHeight), cardNames[i]);
            if (cardNums[i] > 0)
            {
                if (GUI.Button(new Rect(leftOffset + spacingRight, upOffset + (spacingDown * i), smallWidth, buttonHeight), "-"))
                {
                    --cardNums[i];
                    --total;
                }
            }
            GUI.Button(new Rect(leftOffset + (spacingRight * 2), upOffset + (spacingDown * i), smallWidth, buttonHeight), cardNums[i].ToString());
            if (total < upperLimit)
            {
                if (GUI.Button(new Rect(leftOffset + (spacingRight * 3), upOffset + (spacingDown * i), smallWidth, buttonHeight), "+"))
                {
                    ++cardNums[i];
                    ++total;
                }
            }
        }
        if (total == upperLimit)
        {
            if (GUI.Button(new Rect(leftOffset, upOffset + (spacingDown * cardNames.Length + 1), smallWidth, buttonHeight), "Done"))
            {
                MyDeckOfCards.deck = cardNums;
                SceneManager.LoadScene("Network Fight Scene");
            }
        }
    }
}
