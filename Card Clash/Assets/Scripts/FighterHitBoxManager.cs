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
        Debug.Log("Collider hit something!");
        if(col.tag == "Punch")
            gameObject.GetComponent<FighterHealthScript>().TakeDamage(7, col.transform.position);
        else if (col == colliders[1])
            GetComponent<FighterHealthScript>().TakeDamage(5, col.transform.position);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
