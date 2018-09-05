using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerScript : MonoBehaviour {

    public GameManagerScript GM;

    //Debug Mode and Pause Menu switch, TextStyle for Debug Mode
    //private bool debugMode;
    //private GUIStyle debugStyle;



    // Use this for initialization
    void Start () {
        /*debugMode = false;

        //create TextStyle for debugging GUI, loading font
        debugStyle = new GUIStyle();
        debugStyle.fontSize = 45;
        debugStyle.font = (Font)Resources.Load("Yanone Kaffeesatz/YK-Regular");*/

    }

    // Update is called once per frame
    void Update ()
    {
        CheckKeys();

        //activate debug mode if bool is true, or reset to normal mode
        /*if (debugMode)
        {
            DebugMode();
        }
        else
        {
            for (int i = 0; i < GM.cams.Length; i++)
            {
                GM.cams[i].backgroundColor = Color.HSVToRGB(217, 60, 47);
            }

            GameObject.Find("Debug Light").GetComponent<Light>().color = Color.white;
        }*/
    }

    //Debug Code used to check cameras
    /*void DebugMode()
    {
        //Sets lighting to red
        for (int i = 0; i < GM.cams.Length; i++)
        {
            GM.cams[i].backgroundColor = Color.green;
        }

        GameObject.Find("Debug Light").GetComponent<Light>().color = Color.red;

        //switches to first camera
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GM.cams[GM.currentCamIndex].gameObject.SetActive(false);
            GM.cams[0].gameObject.SetActive(true);
            Debug.Log("The " + GM.cams[0].GetComponent<Camera>().name + " is now enabled.");
            GM.currentCamIndex = 0;
        }

        //switches to second camera
        if (Input.GetKeyDown(KeyCode.U))
        {
            GM.cams[GM.currentCamIndex].gameObject.SetActive(false);
            GM.cams[1].gameObject.SetActive(true);
            Debug.Log("The " + GM.cams[1].GetComponent<Camera>().name + " is now enabled.");
            GM.currentCamIndex = 1;
        }

        //switches to third camera
        if (Input.GetKeyDown(KeyCode.I))
        {
            GM.cams[GM.currentCamIndex].gameObject.SetActive(false);
            GM.cams[2].gameObject.SetActive(true);
            Debug.Log("The " + GM.cams[2].GetComponent<Camera>().name + " is now enabled.");
            GM.currentCamIndex = 2;
        }

        //switches to fourth camera
        if (Input.GetKeyDown(KeyCode.O))
        {
            GM.cams[GM.currentCamIndex].gameObject.SetActive(false);
            GM.cams[3].gameObject.SetActive(true);
            Debug.Log("The " + GM.cams[3].GetComponent<Camera>().name + " is now enabled.");
            GM.currentCamIndex = 3;
        }
    }*/

    //Check for Key Inputs
    void CheckKeys()
    {
        //debug mode switch
        /*if (Input.GetKeyDown(KeyCode.D))
        {
            debugMode = !debugMode;
        }*/

        if(Input.GetKeyDown(KeyCode.P))
        {
            GM.TogglePauseMenu();
        }

    }

    private void OnGUI()
    {
        //prints "Debug Mode" on screen
        /*if (debugMode)
        {
            GUI.Label(new Rect(300, 25, 200, 100), "DEBUG MODE", debugStyle);
        }*/
    }
}
