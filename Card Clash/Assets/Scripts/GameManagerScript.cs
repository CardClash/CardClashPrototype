using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    //public UIManagerScript UI;
    public Canvas canvas;

    //Window camera
    public Camera cam;

	// Use this for initialization
	void Start () {
        //Set the camera to true
       cam.gameObject.SetActive(true);

	}
	
	// Update is called once per frame
	void Update ()
    {
       
	}
    
    //Toggles pause menu, sets the time to freeze or un-freeze
    public void TogglePauseMenu()
    {
        if(canvas.transform.GetChild(0).gameObject.activeSelf)
        {
            canvas.transform.GetChild(0).gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            canvas.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
