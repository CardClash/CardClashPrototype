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

    //Collider on this GameObject
    private BoxCollider2D localCollider;

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

        //Create local collider
        localCollider = gameObject.AddComponent<BoxCollider2D>();
        //Set it as a trigger so it won't collide with the environment
        localCollider.isTrigger = true;
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collider hit something!");
        if(localCollider == colliders[0])
            col.GetComponentInParent<FighterHealthScript>().TakeDamage(7);
        else if (localCollider == colliders[1])
            col.GetComponentInParent<FighterHealthScript>().TakeDamage(5);
    }

    public void setHitBox(hitBoxes val)
    {
        if(val != hitBoxes.clear)
        {
            localCollider = colliders[(int)val];
            return;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
