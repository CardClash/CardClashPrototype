using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public float xPos;
    public float yPos;
    public float width;
    public float height;
    public float spacingX;
    public float spacingY;

    private enum menuState { mainMenu, connectionMenu, confirmationMenu };
    private menuState myState;
    private string networkAddress;
    private bool host;

	// Use this for initialization
	void Start ()
    {
        myState = menuState.mainMenu;
        networkAddress = "IP";
	}
	
	// Update is called once per frame
	void Update ()
    {
        //width = Screen.width / 10.0f;
	}

    private void OnGUI()
    {
        if (myState == menuState.mainMenu)
        {
            if (GUI.Button(new Rect(xPos, yPos, width, height), "Play"))
            {
                myState = menuState.connectionMenu;
            }
        }
        else if (myState == menuState.connectionMenu)
        {
            if (GUI.Button(new Rect(xPos, yPos, width, height), "Host"))
            {
                host = true;
            }
            if (GUI.Button(new Rect(xPos, yPos + height + spacingY, width, height), "Connect"))
            {
                host = false;
            }
            networkAddress = GUI.TextField(new Rect(xPos, yPos + ((height + spacingY) * 2), width, (height * 0.75f)), networkAddress);
            if (GUI.Button(new Rect(xPos, yPos + ((height * 2.75f) + (spacingY * 3.0f)), width, height), "Cancel"))
            {
                myState = menuState.mainMenu;
            }
        }
    }
}
