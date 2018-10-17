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

    private enum menuState { mainMenu, connectionMenu };
    private menuState myState;

	// Use this for initialization
	void Start ()
    {
        myState = menuState.mainMenu;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
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
        else if (myState == menuState.mainMenu)
        {
            if (GUI.Button(new Rect(xPos, yPos, width, height), "Host"))
            {

            }
            if (GUI.Button(new Rect(xPos + width, yPos + height + spacingY, width, height), "Connect"))
            {

            }
            if (GUI.Button(new Rect(xPos + (width * 2), yPos + ((height + spacingY) * 2), width, height), "Connect"))
            {
                myState = menuState.mainMenu;
            }
        }
    }
}
