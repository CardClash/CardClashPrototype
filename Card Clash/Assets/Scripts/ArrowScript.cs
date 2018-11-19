using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    private NetworkFighterScript source;
    public float arrowSpeed;
    private bool shot;
    private bool amServer;
    private bool flipped;

    // Use this for initialization
    void Start ()
    {
        shot = false;
        flipped = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (source)
        {
            //shoots arrow at arrowSpeed
            GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(arrowSpeed, 0));
            if (arrowSpeed < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            }
        }

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

    public void Shoot(bool facingRight)
    {
        if (facingRight)
        {
            arrowSpeed *= -1;
        }
        shot = true;
        //print("true");
    }

    public void SetSource(NetworkFighterScript _source)
    {
        _source.MyArrow = gameObject;
        amServer = _source.isServer;
        source = _source;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Old place of OnTriggerEnter2D code.
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
            if (collision.gameObject != source)
            {
                //Vector2 direction = transform.position - collision.transform.position;
                //direction = direction.normalized;
                //collision.gameObject.GetComponent<FighterHealthScript>().TakeHitDamage(4, direction);

                if (amServer)
                {
                    //collision.gameObject.GetComponent<FighterHealthScript>().CmdTakeDamage(20);
                }
                else
                {
                    source.GetComponent<NetworkFighterScript>().CmdAddOpponentDamage(20);
                    Destroy(gameObject);
                }
            }
        }
    }
}
