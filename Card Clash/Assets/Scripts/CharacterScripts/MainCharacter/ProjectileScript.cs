using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {
    public float speed = 6;
    private FighterScript source;
	// Use this for initialization
	void Start () {
        source = GameObject.Find("Main Character").GetComponent<FighterScript>();

        /**
         * Sets the velocity depending on the projectile type and speed
         * Freezes the Y position if it is a straight projectile
         * Ignores collision with other projectiles
         **/
        if (source.facingRight && source.straightProjectile)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            GetComponent<Rigidbody2D>().velocity = source.transform.right * speed;
        }
        else if (!source.facingRight && source.straightProjectile)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            GetComponent<Rigidbody2D>().velocity = -source.transform.right * speed;
        }

        if (source.facingRight && source.lobbedProjectile)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GetComponent<Rigidbody2D>().velocity = source.transform.right * speed + source.transform.up * speed/2;
        }
        else if (!source.facingRight && source.lobbedProjectile)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GetComponent<Rigidbody2D>().velocity = -source.transform.right * speed + source.transform.up * speed/2;
        }

        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), source.GetComponent<BoxCollider2D>());
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    private void OnBecameInvisible()
    {
        //Destroys the projectile when it is not visible on screen
        source.straightProjectile = false;
        source.lobbedProjectile = false;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if it collides with anything besides the player, destroys itself
        if(collision.gameObject != GameObject.Find("Main Character"))
            Destroy(gameObject);
    
        //if it collides with the enemy, destroys the enemy
        if(collision.transform.tag == "Enemy")
        {
            source.straightProjectile = false;
            source.lobbedProjectile = false;
            Destroy(collision.gameObject);
        }
    }
}
