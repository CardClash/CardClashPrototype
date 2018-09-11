using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterHealthScript : MonoBehaviour {

    public int startingPercentage = 0;
    public int currentPercentage;
    private Rigidbody2D rigid;
    //bool isDead;

	// Use this for initialization
	void Start ()
    {
        //set current percentage to the starting percentage amount
        currentPercentage = startingPercentage;
        rigid = GetComponent<Rigidbody2D>();
	}

    public void TakeDamage(int amount, Vector2 hitPos)
    {
        //increase the percentage by the amount of damage taken
        currentPercentage += amount;

        Vector2 force = new Vector2(-(amount * currentPercentage) * 2.0f, (amount * currentPercentage) * 1.5f);

        rigid.AddForceAtPosition(force, hitPos);

    }

    public void TakeDamage(int amount)
    {
        //increase the percentage by the amount of damage taken
        currentPercentage += amount;
    }

    /*void Death()
    {
        isDead = true;
    }*/

}
