using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {
    
    public Canvas canvas;

    //Window camera
    public Camera cam;

    public Camera optionalCamMain;
    public Camera optionalCamStart;

	// Use this for initialization
	void Start () {
        //Set the camera to true
        if (cam != null)
        {
            cam.gameObject.SetActive(true);
        }
        else if (optionalCamMain != null && optionalCamStart != null)
        {
            optionalCamMain.gameObject.SetActive(false);
            optionalCamStart.gameObject.SetActive(true);
        }
        else
        {
            throw new System.Exception("Set up a working set of cameras in the Game Manager");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
       if (optionalCamMain != null && optionalCamStart != null && Input.GetKeyDown(KeyCode.Comma))
        {
            if (optionalCamMain.gameObject.activeSelf)
            {
                optionalCamMain.gameObject.SetActive(false);
                optionalCamStart.gameObject.SetActive(true);
            }
            else
            {
                optionalCamStart.gameObject.SetActive(false);
                optionalCamMain.gameObject.SetActive(true);
            }
        }
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
