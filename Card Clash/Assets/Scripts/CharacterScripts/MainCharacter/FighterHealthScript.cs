using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FighterHealthScript : NetworkBehaviour {

    public int startingPercentage = 0;
    [SyncVar]
    public int currentPercentage;
    private Rigidbody2D rigid;
    //bool isDead;

    public int Damage
    {
        get
        {
            return currentPercentage;
        }
    }

	// Use this for initialization
	void Start ()
    {
        //set current percentage to the starting percentage amount
        currentPercentage = startingPercentage;
        rigid = GetComponent<Rigidbody2D>();
	}

    public void TakeHitDamage(int amount)
    {
        //increase the percentage by the amount of damage taken
        CmdTakeDamage(amount);
        
        //based off of the Smash Bros. series knockback calculation
        float knockback = (((((currentPercentage / 10) + ((currentPercentage * amount) / 20)) * 1.4f) + 18) * 75);

        Vector2 force = new Vector2(knockback, 0);

        //rigid.AddForce(new Vector2(100, 100) * direction, ForceMode2D.Force);
        //rigid.AddForce(new Vector2(0, 6.5f * currentPercentage), ForceMode2D.Force);

        //rigid.velocity = new Vector2(0, 100);

        rigid.AddForce(new Vector2(knockback, knockback), ForceMode2D.Force);

    }

    public void TakeDamage(int amount)
    {
        //increase the percentage by the amount of damage taken
        currentPercentage += amount;
    }

    [Command]
    public void CmdTakeHitDamage(int amount)
    {
        //increase the percentage by the amount of damage taken
        currentPercentage += amount;

        //based off of the Smash Bros. series knockback calculation
        float knockback = (((((currentPercentage / 10) + ((currentPercentage * amount) / 20)) * 1.4f) + 18) * 75);

        Vector2 force = new Vector2(knockback, 0);

        //rigid.AddForce(new Vector2(100, 100) * direction, ForceMode2D.Force);
        //rigid.AddForce(new Vector2(0, 6.5f * currentPercentage), ForceMode2D.Force);

        //rigid.velocity = new Vector2(0, 100);

        rigid.AddForce(new Vector2(knockback, knockback), ForceMode2D.Force);

    }

    [Command]
    public void CmdTakeDamage(int amount)
    {
        //increase the percentage by the amount of damage taken
        currentPercentage += amount;
    }

    [Command]
    public void CmdReset()
    {
        currentPercentage = 0;
    }

}
