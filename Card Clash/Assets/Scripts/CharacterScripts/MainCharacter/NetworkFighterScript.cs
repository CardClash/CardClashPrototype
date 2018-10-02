using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkFighterScript : NetworkBehaviour
{

    public float playerSpeed;
    [Range(5, 20)]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;
    private bool isGrounded;
    private GameObject networkManager;
    [SyncVar]
    private bool facingRight = false;
    [SyncVar]
    private bool corrected = false;
    private Rigidbody2D rigid;
    private GameObject opponent;

    public Animator anim;

    //public bool FacingRight
    //{
    //    get { return facingRight; }
    //}

    public GameObject Opponent
    {
        get { return opponent; }
    }
    
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f);
        networkManager = GameObject.Find("Network Manager");
        networkManager.GetComponent<CardEffects>().Initialize();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.SetActive(true);

        if (!isLocalPlayer)
        {
            return;
        }
        
        networkManager.GetComponent<CardEffects>().SetSources(gameObject);

        SetCamera();

        //Set % of local player dmg
        GameObject.Find("DamageTextPlayer1").GetComponent<Text>().text = gameObject.GetComponent<FighterHealthScript>().Damage.ToString() + "%";

        if (opponent != null)
        {
            //Set % of opponent dmg
            GameObject.Find("DamageTextPlayer2").GetComponent<Text>().text = opponent.GetComponent<FighterHealthScript>().Damage.ToString() + "%";

            opponent.GetComponent<NetworkFighterScript>().CorrectFlip();
        }

        if (opponent == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                if (player != this.gameObject)
                {
                    opponent = player;
                }
            }
        }

        

        //if(isServer)
        //{
        //    tag = "HostPlayer";
        //}
        //else
        //{
        //    tag = "ClientPlayer";
        //}

        //takes in "Horizontal" input for movement on the X-Axis (Refer to the Project-> Project Settings -> Input)
        float inputX = Input.GetAxis("Horizontal");

        //Flips the direction the character is facing
        Flip(inputX);
        CmdFlip(inputX);
        CorrectFlip();

        anim.SetFloat("Speed", Mathf.Abs(rigid.velocity.x));

        //Moves the character each frame
        if (inputX != 0)
            Move(inputX);

        //Checks if the character is within the boundaries of the stage
        CheckBoundaries();

        //Checks for jump
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
            GetComponent<NetworkAnimator>().SetTrigger("hitPunch");
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

    //switches the facingRight bool
    void Flip(float inputX)
    {
        //flips the direction if they are going right...
        if (inputX < 0 && !facingRight)
        {
            facingRight = !facingRight;
            corrected = false;
        }
        //... and vice versa
        else if (inputX > 0 && facingRight)
        {
            facingRight = !facingRight;
            corrected = false;
        }
    }

    [Command]
    //switches the facingRight bool
    void CmdFlip(float inputX)
    {
        //flips the direction if they are going right...
        if (inputX < 0 && !facingRight)
        {
            facingRight = !facingRight;
            corrected = false;
        }
        //... and vice versa
        else if (inputX > 0 && facingRight)
        {
            facingRight = !facingRight;
            corrected = false;
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

        //GetComponent<FighterHealthScript>().currentPercentage = 0;

        GetComponent<FighterHealthScript>().CmdReset();

        playerSpeed = 10;
    }

    private void CheckBoundaries()
    {
        //IF the player position is outside the boundaries of the stage, reset them to the stage
        if (transform.position.x < -40.0f)
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

    private void SetCamera()
    {
        GameObject cam = GameObject.Find("Window Camera");
        if (cam != null)
        {
            cam.GetComponent<WindowCamera>().SetMainCharacter(gameObject);
        }
    }

    public void CorrectFlip()
    {
        if (!corrected)
        {
            if (facingRight)
            {
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
