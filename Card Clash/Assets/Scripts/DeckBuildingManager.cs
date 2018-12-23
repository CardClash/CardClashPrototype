using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckBuildingManager : MonoBehaviour {

    public string[] cardNames;
    public int upperLimit;

    public GameObject finishedButton;
    public GameObject dmg10Button;
    public GameObject speedUpButton;
    public GameObject healButton;
    public GameObject teleButton;
    public GameObject dmg25Button;
    public GameObject arrowButton;

    private Color highlightedColor;

    public GameObject totalCardText;
    public GameObject dmg10Text;
    public GameObject speedUpText;
    public GameObject healText;
    public GameObject teleText;
    public GameObject dmg25Text;
    public GameObject arrowText;

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

        finishedButton.GetComponent<Button>().interactable = false;

        highlightedColor = new Color();

        ColorUtility.TryParseHtmlString("00D1FF", out highlightedColor);
	}
	
	// Update is called once per frame
	void Update ()
    {
        #region Add Cards
        if(total < upperLimit)
        {
            if(dmg10Button == EventSystem.current.currentSelectedGameObject && Input.GetButtonDown("Submit"))
            {
                ++cardNums[0];
                ++total;
            }
            if (speedUpButton == EventSystem.current.currentSelectedGameObject && Input.GetButtonDown("Submit"))
            {
                ++cardNums[1];
                ++total;
            }
            if (healButton == EventSystem.current.currentSelectedGameObject && Input.GetButtonDown("Submit"))
            {
                ++cardNums[2];
                ++total;
            }
            if (teleButton == EventSystem.current.currentSelectedGameObject && Input.GetButtonDown("Submit"))
            {
                ++cardNums[3];
                ++total;
            }
            if (dmg25Button == EventSystem.current.currentSelectedGameObject && Input.GetButtonDown("Submit"))
            {
                ++cardNums[4];
                ++total;
            }
            if (arrowButton == EventSystem.current.currentSelectedGameObject && Input.GetButtonDown("Submit"))
            {
                ++cardNums[5];
                ++total;
            }
        }
        #endregion
        #region Remove Cards
        if (cardNums[0] > 0 && dmg10Button == EventSystem.current.currentSelectedGameObject && Input.GetButtonDown("Cancel"))
        {
            --cardNums[0];
            --total;
        }
        if (cardNums[1] > 0 && speedUpButton == EventSystem.current.currentSelectedGameObject && Input.GetButtonDown("Cancel"))
        {
            --cardNums[1];
            --total;
        }
        if (cardNums[2] > 0 && healButton == EventSystem.current.currentSelectedGameObject && Input.GetButtonDown("Cancel"))
        {
            --cardNums[2];
            --total;
        }
        if (cardNums[3] > 0 && teleButton == EventSystem.current.currentSelectedGameObject && Input.GetButtonDown("Cancel"))
        {
            --cardNums[3];
            --total;
        }
        if (cardNums[4] > 0 && dmg25Button == EventSystem.current.currentSelectedGameObject && Input.GetButtonDown("Cancel"))
        {
            --cardNums[4];
            --total;
        }
        if (cardNums[5] > 0 && arrowButton == EventSystem.current.currentSelectedGameObject && Input.GetButtonDown("Cancel"))
        {
            --cardNums[5];
            --total;
        }
        #endregion
        if (total == upperLimit)
        {
            finishedButton.GetComponent<Button>().interactable = true;

            if(finishedButton == EventSystem.current.currentSelectedGameObject && Input.GetButtonDown("Submit"))
            {
                MyDeckOfCards.deck = cardNums;
                Debug.Log(cardNums);
                SceneManager.LoadScene("Network Fight Scene");
            }
        }
        else
        {
            finishedButton.GetComponent<Button>().interactable = false;
        }

        #region Updating Text
        totalCardText.GetComponent<Text>().text = "Total Card Count: "  + total.ToString();
        dmg10Text.GetComponent<Text>().text = cardNums[0].ToString();
        speedUpText.GetComponent<Text>().text = cardNums[1].ToString();
        healText.GetComponent<Text>().text = cardNums[2].ToString();
        teleText.GetComponent<Text>().text = cardNums[3].ToString();
        dmg25Text.GetComponent<Text>().text = cardNums[4].ToString();
        arrowText.GetComponent<Text>().text = cardNums[5].ToString();
        #endregion
    }
}
