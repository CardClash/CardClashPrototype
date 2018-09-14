using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterHitBoxManager : MonoBehaviour
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
        //Calcualtes the direction of the collision, collider to collidee
        Vector2 direction = col.transform.position - transform.position;
        direction = -direction.normalized;

        Debug.Log("Collider hit something!");
        //If the collider is a punch, it will deal damage to the opponent
        if(col.tag == "Punch")
            gameObject.GetComponent<FighterHealthScript>().TakeDamage(7, direction);
        else if (col == colliders[1])
            GetComponent<FighterHealthScript>().TakeDamage(5, direction);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
