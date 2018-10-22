

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkFighterScript : NetworkBehaviour
{

    public float playerSpeed;
    private int lives;
    public int playerState;
    [Range(5, 20)]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;
    private bool isGrounded;
    private GameObject networkManager;
    [SyncVar]
    public bool facingRight = false;
    [SyncVar]
    private bool corrected = false;
    public bool readied;
    public bool matchStarted;
    private Rigidbody2D rigid;
    private GameObject opponent;
    public int playerNumber;
    private float playerMana;
    public int manaDisplay;
    public int actualMana;

    private bool host;

    public Animator anim;
    public GameObject endGameText;

    public GameObject Opponent
    {
        get { return opponent; }
    }

    public int PlayerNumber
    {
        get { return playerNumber; }
        set { playerNumber = value; }
    }

    public int PlayerState
    {
        get { return playerState; }
    }

    public bool Ready
    {
        get { return readied; }
    }

    public bool MatchStarted
    {
        get { return matchStarted; }
        set { matchStarted = value; }
    }

    public bool Host
    {
        get { return host; }
        set { host = value; }
    }
    
    void Start()
    {
        if (playerNumber > 2 || playerNumber < 1)
        {
            playerNumber = 1;
        }

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);

        lives = 4;

        playerMana = 1;
        actualMana = 0;

        //set player state to 0, meaning they haven't lost or won
        playerState = 0;

        endGameText = GameObject.Find("EndGameText");
        //endGameText.GetComponent<Text>().text = "Not Ready\nPress spacebar to be ready";

        readied = false;
        matchStarted = false;
        
        //GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void OnStartLocalPlayer()
    {
        host = isServer;
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f);
        //CmdEnableRender();
        networkManager = GameObject.Find("Network Manager");
        networkManager.GetComponent<CardEffects>().Initialize();
        playerNumber = networkManager.GetComponent<CharacterSelect>().GetPlayerNumber();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (!matchStarted)
        //{
        //    transform.position = new Vector3(0, 0, transform.position.z);

        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        readied = !readied;
        //    }

        //    if (readied)
        //    {
        //        endGameText.GetComponent<Text>().text = "Ready\nPress spacebar to not be ready";
        //        if (opponent && opponent.GetComponent<NetworkFighterScript>().Ready && isServer)
        //        {
        //            networkManager.GetComponent<NetworkSpawnHandler>().CmdStartMatch();
        //        }
        //    }
        //    else
        //    {
        //        endGameText.GetComponent<Text>().text = "Not Ready\nPress spacebar to be ready";
        //    }
        //    return;
        //}

        //run all usual code if the player hasn't won or lost yet
        if (playerState == 0)
        {
            endGameText.GetComponent<Text>().text = "";
            gameObject.SetActive(true);
            //Debug.Log(playerMana);

            if (!isLocalPlayer)
            {
                return;
            }

            networkManager.GetComponent<CardEffects>().SetSources(gameObject);

            SetCamera();

            DisplayHealth();

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

            if (Input.GetButtonDown("Teleport"))
                TeleportDir(inputX);

            //Checks player state
            CheckPlayerState();

            CheckInput();

            ManaSystem();
        }

        //run this code if the player has won
        if (playerState == 1)
        {
            print("You Won!");
            print("Press Any Button to Exit");

            endGameText.SetActive(true);

            endGameText.GetComponent<Text>().text = "You Win!\nPress Any Button to Exit";

            if (Input.anyKey)
            {
                Debug.Break();
                Application.Quit();
            }
        }

        //run this code if the player has lost
        if (playerState == 2)
        {
            print("You Lost...");
            print("Press Any Button to Exit");

            endGameText.SetActive(true);

            endGameText.GetComponent<Text>().text = "You Lose...\nPress Any Button to Exit";

            if (Input.anyKey)
            {
                Debug.Break();
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

        playerMana = 1;
    }

    private void FullReset()
    {
        transform.position = new Vector3(0.0f, 2.5f, -1.0f);

        rigid.velocity = new Vector2();

        GetComponent<FighterHealthScript>().CmdReset();

        playerSpeed = 10;

        playerMana = 1;

        lives = 4;

        playerState = 0;
    }

    private void CheckBoundaries()
    {
        //If the player position is outside the boundaries of the stage, reset them to the stage
        if (transform.position.x < -40.0f)
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

        //clamps player's velocity to the playerSpeed 1.5x
        rigid.velocity = Vector2.ClampMagnitude(rigid.velocity, playerSpeed * 1.5f);
    }

    private void SetCamera()
    {
        if(!isLocalPlayer)
        {
            return;
        }
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

    public void DisplayHealth()
    {
        if (playerNumber == 1)
        {
            //Set % of local player dmg
            GameObject.Find("DamageTextPlayer1").GetComponent<Text>().text = gameObject.GetComponent<FighterHealthScript>().Damage.ToString() + "%";

            if (opponent != null)
            {
                //Set % of opponent dmg
                GameObject.Find("DamageTextPlayer2").GetComponent<Text>().text = opponent.GetComponent<FighterHealthScript>().Damage.ToString() + "%";

                opponent.GetComponent<NetworkFighterScript>().CorrectFlip();
            }
        }
        else if (playerNumber == 2)
        {
            //Set % of local player dmg
            GameObject.Find("DamageTextPlayer2").GetComponent<Text>().text = gameObject.GetComponent<FighterHealthScript>().Damage.ToString() + "%";

            if (opponent != null)
            {
                //Set % of opponent dmg
                GameObject.Find("DamageTextPlayer1").GetComponent<Text>().text = opponent.GetComponent<FighterHealthScript>().Damage.ToString() + "%";

                opponent.GetComponent<NetworkFighterScript>().CorrectFlip();
            }
        }
    }

    public void ManaSystem()
    {
      manaDisplay = (int)playerMana;
      playerMana = playerMana + Time.deltaTime;
      //Debug.Log("Under -1");


      if (playerMana >= 10)
        {
            actualMana = actualMana + 1;
            playerMana = 1;
        }
      while (actualMana <= -1)
        {
            
            actualMana = 0;
        }
    }

    public void CheckPlayerState()
    {
        //if the player loses all their lives, they lose
        if (lives <= 0)
        {
            //set player state to 2, meaning they lost
            playerState = 2;
        }

        //if your opponent loses, you win
        if (opponent && opponent.GetComponent<NetworkFighterScript>().PlayerState == 2)
        {
            playerState = 1;
        }
    }

    public void CheckInput()
    {
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

        //press escape or the select button to quit
        if (Input.GetButton("Quit"))
        {
            Application.Quit();
        }
    }

    

    public void TeleportDir(float xDir)
    {
        float yDir = Input.GetAxis("Vertical");

        //Calculate the direction of the input
        Vector2 dir = new Vector2(xDir, yDir);
        //Calculate the magnitude of the
        float mag = dir.magnitude;

        //Set the distance to 5 depending on direction
        if (dir.x < 0)
        {
            dir.x = -5;
        }
        else if(dir.x > 0)
        {
            dir.x = 5;
        }
        else
        {
            dir.x = 0;
        }

        if (dir.y < 0)
        {
            dir.y = -5;
        }
        else
        {
            dir.y = 5;
        }

        //add the direction to the position with the max distance being 5, multiplied by the xDir and yDir (-1 to 1)
        transform.position += new Vector3(dir.x, dir.y, 0) * mag;
    }

    [Command]
    public void CmdEnableRender()
    {
        GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
    }
}