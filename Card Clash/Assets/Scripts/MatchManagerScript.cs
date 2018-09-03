using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GamePhase { CardBegin, CardMainOne, CardCombat, CardMainTwo, CardEnd, SkirmishBegin, SkirmishMain, SkirmishEnd };

public class MatchManagerScript : MonoBehaviour
{
    public int battleTimer;
    public int cardTimer;

    private GamePhase currentPhase;
    private float timer;
    private List<GameObject> players;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		if (timer <= 0)
        {
            currentPhase++;
        }
        else
        {
            timer -= Time.deltaTime;
        }
	}
}