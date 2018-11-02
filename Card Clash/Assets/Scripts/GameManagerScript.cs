using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManagerScript : NetworkBehaviour
{
    
    public float timeStopTimer = 5.0f;

    public void TimeStop()
    {
        timeStopTimer -= Time.deltaTime;

        if (timeStopTimer <= 0.0f)
        {
            Time.timeScale = 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            timeStopTimer = 5.0f;
        }
    }

    public Canvas canvas;

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
