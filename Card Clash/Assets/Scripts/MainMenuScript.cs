using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public GameObject MenuScreen;
    public GameObject InstructionButton;
    public GameObject InstructionScreen;
    public GameObject UIInstructions;
    public GameObject ControlsInstructions;
    public GameObject CreditsScreen;
    public GameObject ExitCreditsButton;
    public GameObject Background;

    private float timer;

    // Use this for initialization
    void Start () {
        SwitchToMain();
        timer = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

        if (UIInstructions.activeSelf == true && timer >= 0.1f)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                SwitchToMain();
            }
            if (Input.GetButtonDown("Submit"))
            {
                SwitchToControls();
            }
        }

        if (ControlsInstructions.activeSelf == true && timer >= 0.1f)
        {
            if(Input.GetButtonDown("Cancel"))
            {
                SwitchToInstructions();
            }
            if (Input.GetButtonDown("Submit"))
            {
                SwitchToMain();
            }
        }
	}
    public void SwitchToMain()
    {
        MenuScreen.SetActive(true);
        Background.SetActive(true);
        InstructionScreen.SetActive(false);
        CreditsScreen.SetActive(false);
        UIInstructions.SetActive(false);
        ControlsInstructions.SetActive(false);
        InstructionButton.GetComponent<Button>().Select();
    }

    public void SwitchToInstructions()
    {
        MenuScreen.SetActive(false);
        Background.SetActive(false);
        InstructionScreen.SetActive(true);
        CreditsScreen.SetActive(false);
        UIInstructions.SetActive(true);
        ControlsInstructions.SetActive(false);
        timer = 0;
    }

    public void SwitchToControls()
    {
        MenuScreen.SetActive(false);
        Background.SetActive(false);
        InstructionScreen.SetActive(true);
        CreditsScreen.SetActive(false);
        UIInstructions.SetActive(false);
        ControlsInstructions.SetActive(true);
        timer = 0;
    }

    public void SwitchToCredits()
    {
        MenuScreen.SetActive(false);
        Background.SetActive(true);
        InstructionScreen.SetActive(false);
        CreditsScreen.SetActive(true);
        ExitCreditsButton.GetComponent<Button>().Select();
    }

    public void Close()
    {
        Application.Quit();
    }

}
