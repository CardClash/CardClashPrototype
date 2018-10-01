using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FighterHitBoxManager : NetworkBehaviour
{
    //Colliders we'll be using for now, set them in the editor
    public BoxCollider2D punch;
    public BoxCollider2D kick;

    //array to organize them
    private BoxCollider2D[] colliders;

    //Enum for hitboxes
    public enum hitBoxes
    {
        punchBox,
        kickBox,
        clear
    }

	// Use this for initialization
	void Start ()
    {
        //Set up the array
        colliders = new BoxCollider2D[] { punch, kick };
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("OnTriggerEnter2D");
        //Calcualtes the direction of the collision, collider to collidee
        Vector2 direction = transform.position - col.transform.position;
        direction = direction.normalized;

        //If the collider is a punch, it will deal damage to the opponent
        if (col.tag == "Punch")
        { 
            Debug.Log("Punched something");
            gameObject.GetComponent<FighterHealthScript>().TakeHitDamage(7);
        }
        else if (col == colliders[1])
            GetComponent<FighterHealthScript>().TakeHitDamage(5);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
