using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterSelect : NetworkBehaviour {

    public int playerCount;
    public GameObject movable;
    public GameObject[] options;

    private NetworkManagerHUD networkHUD;
    private bool doneSelect;
    private bool finished;
    private int playerNum;

	// Use this for initialization
	void Start ()
    {
        networkHUD = gameObject.GetComponent<NetworkManagerHUD>();
        networkHUD.enabled = true;
        doneSelect = true;
        finished = false;
        playerNum = 1;
        if (options.Length != playerCount)
        {
            throw new System.Exception("options.Length != playerCount in CharacterSelect script");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!finished)
        {
            float inputX = Input.GetAxis("Horizontal");

            if (inputX > 0 && doneSelect)
            {
                playerNum++;
                if (playerNum > playerCount)
                {
                    playerNum = 1;
                }
                doneSelect = false;
            }

            else if (inputX < 0 && doneSelect)
            {
                playerNum--;
                if (playerNum < 1)
                {
                    playerNum = playerCount;
                }
                doneSelect = false;
            }

            else if (inputX == 0)
            {
                doneSelect = true;
            }
        }

        movable.transform.position = options[playerNum - 1].transform.position;

        if (Input.GetButtonDown("Submit"))
        {
            finished = true;
            networkHUD.enabled = true;
            movable.SetActive(false);
            for (int i = 0; i < options.Length; i++)
            {
                options[i].SetActive(false);
            }
        }
	}

    public int GetPlayerNumber()
    {
        return playerNum;
    }
}
