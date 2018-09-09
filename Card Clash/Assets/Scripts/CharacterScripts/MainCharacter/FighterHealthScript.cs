using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterHealthScript : MonoBehaviour {

    public int startingPercentage = 0;
    public int currentPercentage;

    bool isDead;

	// Use this for initialization
	void Start ()
    {
        isDead = false;
        //set current percentage to the starting percentage amount
        currentPercentage = startingPercentage;
	}

    public void TakeDamage(int amount)
    {
        //increase the percentage by the amount of damage taken
        currentPercentage += amount;
    }

    void Death()
    {
        isDead = true;
    }

}
