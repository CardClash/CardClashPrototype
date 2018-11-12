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
        }
        else if (col.tag == "DamageBall")
        {
            print(col.GetComponent<DamageBallScript>().Damage);
            gameObject.GetComponent<FighterHealthScript>().TakeHitDamage(col.GetComponent<DamageBallScript>().Damage, Vector3.zero);
            print("me");
            col.GetComponent<DamageBallScript>().Damage = 0;
            col.GetComponent<DamageBallScript>().CmdSetDamage(0);
            //collision.GetComponent<DamageBallScript>().Target = null;
            col.GetComponent<DamageBallScript>().ResetLoc();
            col.GetComponent<DamageBallScript>().CmdResetLoc();
        }
    }
}
