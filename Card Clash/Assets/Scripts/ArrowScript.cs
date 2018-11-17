using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    private NetworkFighterScript source;
    public float arrowSpeed;
    private bool shot;

    // Use this for initialization
    void Start ()
    {
        shot = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //shoots arrow at arrowSpeed
        GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(arrowSpeed, 0));

        //Destroys the arrow if it leaves the stage
        if (transform.position.x < -40.0f)
        {
            shot = false;
            Destroy(gameObject);
        }
        if (transform.position.x > 44.0f)
        {
            shot = false;
            Destroy(gameObject);
        }
        if (transform.position.y < -18.0f)
        {
            shot = false;
            Destroy(gameObject);
        }
        if (transform.position.y > 24.0f)
        {
            shot = false;
            Destroy(gameObject);
        }
    }

    public void Shoot()
    {
        shot = true;
        print("true");
    }

    public void SetSource(NetworkFighterScript _source)
    {
        source = _source;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroys the arrow if it touches the ground
        if (collision.transform.tag == "Ground")
        {
            shot = false;
            Destroy(gameObject);
        }

        //Damages the player that touches the arrow
        if (collision.transform.tag == "Player")
        {
            Vector2 direction = transform.position - collision.transform.position;
            direction = direction.normalized;
            source.GetComponent<FighterHealthScript>().TakeHitDamage(4, direction);
            //Destroy(gameObject);
        }
    }
}
