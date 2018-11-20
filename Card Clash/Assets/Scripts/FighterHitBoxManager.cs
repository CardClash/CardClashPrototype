using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FighterHitBoxManager : NetworkBehaviour
{
    //Colliders we'll be using for now, set them in the editor
    public BoxCollider2D punch;
    public BoxCollider2D kick;

    //Enum for hitboxes
    public enum HitBoxes
    {
        punchBox,
        kickBox,
        clear
    }

	// Use this for initialization
	void Start ()
    {

	}

    void OnTriggerEnter2D(Collider2D col)
    {
        //Calcualtes the direction of the collision, collider to collidee
        Vector2 direction = transform.position - col.transform.position;
        direction = direction.normalized;

        //If the collider is a punch, it will deal damage to the opponent
        if (col.tag == "Punch")
        {
            gameObject.GetComponent<FighterHealthScript>().TakeHitDamage(7, direction);
            //gameObject.GetComponent<FighterHealthScript>().CmdTakeHitDamage(7);
            
            //disable hitbox to prevent multiple collisions
            col.enabled = false;
        }
        else if (col.tag == "Projectile" && col.gameObject != GetComponent<NetworkFighterScript>().MyArrow)
        {
            //print(GetComponent<NetworkFighterScript>().MyArrow);
            //Vector2 direct = col.transform.position - transform.position;
            //direct.Normalize();
            //gameObject.GetComponent<FighterHealthScript>().TakeHitDamage(20, direct);
            
            
            if (isServer)
            {
                GetComponent<FighterHealthScript>().CmdTakeDamage(20);
                Destroy(col.gameObject);
            }
            else
            {
                //GetComponent<FighterHealthScript>().CmdTakeDamage(20);
                
                //GetComponent<NetworkFighterScript>().CmdAddOpponentDamage(20);
                //GetComponent<NetworkFighterScript>().OpponentDamage = GetComponent<NetworkFighterScript>().OpponentDamage + 20;
            }
        }
    }
}
