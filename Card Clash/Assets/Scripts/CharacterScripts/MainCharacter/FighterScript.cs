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

    public Animator anim;

	// Use this for initialization
	void Start ()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        anim.SetFloat("Speed", Mathf.Abs(rigid.velocity.x));
        //locks rotation
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x, 
            0, 0);
        //takes in input for the x-axis
        float inputX = Input.GetAxis("Horizontal");

        Move(inputX);

        Flip(inputX);

        CheckBoundaries();

        CheckJump();

        //press R or Start to reset
        if (Input.GetButton("Reset"))
        {
            Reset();
        }

        //press J or the A button to punch
        if (Input.GetButtonDown("Punch"))
        {
            Debug.Log("Punch!");
            anim.SetBool("isPunching", true);
        }
        else if(!Input.GetButton("Punch"))
        {
            anim.SetBool("isPunching", false);
        }

        //clamps player's velocity to the playerSpeed 1.5x
        rigid.velocity = Vector2.ClampMagnitude(rigid.velocity, playerSpeed * 1.5f);

        //press escape or the select button to quit
        if(Input.GetButton("Quit"))
        {
            Application.Quit();
        }

    }

    //switches the facingRight bool
    void Flip(float inputX)
    {
        //flips the direction if they are going right...
        if (inputX < 0 && !facingRight)
        {
            facingRight = !facingRight;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        //... and vice versa
        else if (inputX > 0 && facingRight)
        {
            facingRight = !facingRight;
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the player collides with the ground, isGrounded is true
        if (collision.transform.tag == "Ground")
        {
            anim.SetBool("isGrounded", true);
            anim.SetBool("isFalling", false);
            //anim.Play("fighterIdleAnim");
        }
        if (collision.transform.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
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

    private void CheckBoundaries()
    {
        if(transform.position.x < -40.0f)
        {
            Reset();
        }
        if (transform.position.x > 44.0f)
        {
            Reset();
        }
        if (transform.position.y < -18.0f)
        {
            Reset();
        }
        if (transform.position.y > 24.0f)
        {
            Reset();
        }
    }

    private void CheckJump()
    {
        //if the player is on the ground and you are pressing space or A, jump
        if ((anim.GetBool("isGrounded") && Input.GetButtonDown("Jump")))
        {
            anim.SetBool("isJumping", true);
            //anim.Play("fighterJumpAnim");
            rigid.velocity = Vector2.up * jumpVelocity;
        }

        //if the player is falling, set the falling speed using the fallMaultiplier value
        if (rigid.velocity.y < 0)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
            //anim.Play("fighterFallAnim");
            rigid.velocity += Vector2.up * Physics2D.gravity.y *
                (fallMultiplier - 1) * Time.deltaTime;
        }
        //if the player is rising from a jump from a jump and you aren't holding A, set the falling speed using the lowJumpMultiplier value
        else if (rigid.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigid.velocity += (Vector2.up * Physics2D.gravity.y *
                            (lowJumpMultiplier - 1) * Time.deltaTime);
        }
    }

    private void Move(float inputX)
    {
        //sets the velocity to the playerSpeed and the direction of the x-axis input
        rigid.velocity = new Vector2(
            playerSpeed * inputX,
            rigid.velocity.y);
    }
}
