using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterScript : MonoBehaviour {
    
    public float playerSpeed;
    [Range(5, 20)]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;
    private bool isGrounded;
    public bool facingRight = false;
    private Rigidbody2D rigid;

    public GameObject bullet;
    public bool straightProjectile;
    public bool lobbedProjectile;

    public Animator anim;

	// Use this for initialization
	void Start ()
    {
        rigid = GetComponent<Rigidbody2D>();
        //load in the projectile
        bullet = (GameObject)Resources.Load("Projectile");
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //locks rotation
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x, 
            0, 0);
        //takes in input for the x-axis
        float inputX = Input.GetAxis("Horizontal");

        //sets the velocity to the playerSpeed and the direction of the x-axis input
        rigid.velocity = new Vector2(
            playerSpeed * inputX,
            rigid.velocity.y);

        //flips the direction if they are going right...
        if(inputX > 0 && !facingRight)
        {
            Flip();
        }
        //... and vice versa
        else if(inputX < 0 && facingRight)
        {
            Flip();
        }


        //if the player is on the ground and you are pressing space or A, jump
        if(anim.GetBool("isGrounded") && Input.GetKeyDown("joystick button 0"))
        {
            anim.SetBool("isJumping", true);
            anim.Play("fighterJumpAnim");
            rigid.velocity = Vector2.up * jumpVelocity;
        }

        //if the player is falling, set the falling speed using the fallMaultiplier value
        if(rigid.velocity.y < 0)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
            anim.Play("fighterFallAnim");
            rigid.velocity += Vector2.up * Physics2D.gravity.y *
                (fallMultiplier - 1) * Time.deltaTime;
        }
        //if the player is rising from a jump from a jump and you aren't holding space, set the falling speed using the lowJumpMultiplier value
        /*else if(rigid.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rigid.velocity += (Vector2.up * Physics2D.gravity.y *
                            (lowJumpMultiplier - 1) * Time.deltaTime);
        }*/
        //if the player is rising from a jump from a jump and you aren't holding A, set the falling speed using the lowJumpMultiplier value
        else if (rigid.velocity.y > 0 && !Input.GetKey("joystick button 0"))
        {
            rigid.velocity += (Vector2.up * Physics2D.gravity.y *
                            (lowJumpMultiplier - 1) * Time.deltaTime);
        }

        //press R to reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

        //press Z to shoot the straight projectile
        if (Input.GetKeyDown(KeyCode.Z))
        {
            straightProjectile = true;
            //create a projectile and set it depending on the way the player is facing
            if(facingRight)
                Instantiate(bullet, new Vector3(transform.position.x + 0.5f, transform.position.y), Quaternion.identity);
            else if(!facingRight)
                Instantiate(bullet, new Vector3(transform.position.x - 0.5f, transform.position.y), Quaternion.identity);
        }

        //press X to shoot the lobbed projectile
        if(Input.GetKeyDown(KeyCode.X))
        {
            lobbedProjectile = true;
            //create a projectile and set it depending on the way the player is facing
            if (facingRight)
            {
                Instantiate(bullet, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f), Quaternion.identity);
            }
            else if(!facingRight)
            {
                Instantiate(bullet, new Vector3(transform.position.x - 0.5f, transform.position.y + 0.5f), Quaternion.identity);
            }
        }

        //clamps player's velocity to the playerSpeed 1.5x
        rigid.velocity = Vector2.ClampMagnitude(rigid.velocity, playerSpeed * 1.5f);

    }

    //switches the facingRight bool
    void Flip()
    {
        facingRight = !facingRight;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the player collides with the ground, isGrounded is true
        if (collision.transform.tag == "Ground")
        {
            anim.SetBool("isGrounded", true);
            anim.SetBool("isFalling", false);
            anim.Play("fighterIdleAnim");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //if the player stops colliding with the ground, isGrounded is false
        if (collision.transform.tag == "Ground")
        {
           anim.SetBool("isGrounded", false);
        }
    }

    //resets player's velocity and position
    private void Reset()
    {
        transform.position = new Vector3(0.0f, 2.5f, 0.0f);

        rigid.velocity = new Vector2();
    }
}
