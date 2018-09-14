﻿using System.Collections;
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

    public void TakeDamage(int amount, Vector2 direction)
    {
        //increase the percentage by the amount of damage taken
        currentPercentage += amount;

        //based off of the Smash Bros. series knockback calculation
        float knockback = (((((currentPercentage / 10) + ((currentPercentage * amount) / 20)) * 1.4f) + 18) * 75);

        Vector2 force = new Vector2(knockback, 0);

        rigid.AddForce(force * direction, ForceMode2D.Force);
        //rigid.AddForce(new Vector2(0, 6.5f * currentPercentage), ForceMode2D.Impulse);

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
