﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterScript : MonoBehaviour {
    
    public float playerSpeed;
    private int lives;
    public int playerState;
    [Range(5, 20)]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;
    private bool isGrounded;
    [HideInInspector]public bool facingRight = false;
    private Rigidbody2D rigid;
    public Animator anim;

	// Use this for initialization
	void Start ()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        lives = 4;

        //set player state to 0, meaning they haven't lost or won
        playerState = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //run all usual code if the player hasn't won or lost
        if(playerState == 0)
        {
            anim.SetFloat("Speed", Mathf.Abs(rigid.velocity.x));

            //takes in "Horizontal" input for movement on the X-Axis (Refer to the Project-> Project Settings -> Input)
            float inputX = Input.GetAxis("Horizontal");

            //Moves the character each frame
            if (inputX != 0)
                Move(inputX);

            //Flips the direction the character is facing
            Flip(inputX);

            //Checks if the character is within the boundaries of the stage
            CheckBoundaries();

            //Checks for jump
            CheckJump();

            //press R or Start to reset
            if (Input.GetButton("Reset"))
            {
                Reset();
            }

            if (Input.GetKey("3"))
                FullReset();

            //press J or the A button to punch
            if (Input.GetButtonDown("Punch"))
            {
                Debug.Log("Punch!");
                anim.SetTrigger("hitPunch");
            }

            //clamps player's velocity to the playerSpeed 1.5x
            rigid.velocity = Vector2.ClampMagnitude(rigid.velocity, playerSpeed * 1.5f);

            //press escape or the select button to quit
            if (Input.GetButton("Quit"))
            {
                Application.Quit();
            }
        }
        
        //run this code if the player has won
        if(playerState == 1)
        {
            print("You Won!");
            print("Press Any Button to Exit");

            if(Input.anyKey)
            {
                Application.Quit();
            }
        }

        //run this code if the player has lost
        if(playerState == 2)
        {
            print("You Lost...");
            print("Press Any Button to Exit");

            if (Input.anyKey)
            {
                Application.Quit();
            }
        }
    }

    //switches the facingRight bool
    void Flip(float inputX)
    {
        //flips the direction if they are going right...
        if (inputX < 0 && !facingRight)
        {
            facingRight = !facingRight;
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        //... and vice versa
        else if (inputX > 0 && facingRight)
        {
            facingRight = !facingRight;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the player collides with the ground, isGrounded is true
        if (collision.transform.tag == "Ground")
        {
            anim.SetBool("isGrounded", true);
            anim.SetBool("isFalling", false);
        }

        // Ignores collision between player colliders
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
        transform.position = new Vector3(0.0f, 2.5f, -1.0f);

        rigid.velocity = new Vector2();

        GetComponent<FighterHealthScript>().currentPercentage = 0;

        playerSpeed = 10;
    }

    private void FullReset()
    {
        transform.position = new Vector3(0.0f, 2.5f, -1.0f);

        rigid.velocity = new Vector2();

        GetComponent<FighterHealthScript>().currentPercentage = 0;

        playerSpeed = 10;

        lives = 4;

        playerState = 0;
    }

    private void CheckBoundaries()
    {
        //if the player loses all their lives, they lose
        if (lives <= 0)
        {
            //set player state to 2, meaning they lost
            playerState = 2;
        }
        //If the player position is outside the boundaries of the stage, reset them to the stage and remove a life
        if(transform.position.x < -40.0f)
        {
            Reset();
            lives--;
        }
        if (transform.position.x > 44.0f)
        {
            Reset();
            lives--;
        }
        if (transform.position.y < -18.0f)
        {
            Reset();
            lives--;
        }
        if (transform.position.y > 24.0f)
        {
            Reset();
            lives--;
        }
    }

    private void CheckJump()
    {
        //if the player is on the ground and you are pressing space or A, jump
        if ((anim.GetBool("isGrounded") && Input.GetButtonDown("Jump")))
        {
            anim.SetBool("isJumping", true);
            //anim.Play("fighterJumpAnim");
            rigid.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        }

        //if the player is falling, set the falling speed using the fallMaultiplier value
        if (rigid.velocity.y < 0)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
            //anim.Play("fighterFallAnim");
            rigid.AddForce(Vector2.up * Physics2D.gravity.y *
                (fallMultiplier - 1) * Time.deltaTime, ForceMode2D.Impulse);
        }
        //if the player is rising from a jump from a jump and you aren't holding A, set the falling speed using the lowJumpMultiplier value
        else if (rigid.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigid.AddForce(Vector2.up * Physics2D.gravity.y *
                            (lowJumpMultiplier - 1) * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    private void Move(float inputX)
    {
        rigid.velocity = new Vector2(playerSpeed * inputX, rigid.velocity.y);
    }
}
