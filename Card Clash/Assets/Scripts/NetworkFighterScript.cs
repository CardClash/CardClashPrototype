

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkFighterScript : NetworkBehaviour
{
    #region Variables
    public float playerSpeed;
    public float playerHitSpeed;
    [SyncVar]
    private int lives;
    [SyncVar]
    public int playerState;
    [Range(5, 20)]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;
    private float gravityScale;
    private bool isGrounded;
    private GameObject networkManager;
    [SyncVar]
    public bool facingRight = false;
    [SyncVar]
    private bool corrected = false;
    public bool readied;
    public bool matchStarted;
    private Rigidbody2D rigid;
    private FighterHealthScript localPlayerHealthScript;
    private GameObject opponent;
    private NetworkFighterScript o_NetFighterScript;
    public int playerNumber;
    public float playerMana;
    public int manaDisplay;
    private int actualMana;
    [SyncVar]
    private float timeStopTimer = 0.0f;
    private float opponentTimeStopTimer;
    public float cardTimeScale = 0.5f;
    private float defaultTime;

    [SyncVar]
    private int artArrayNum;

    private bool gameStarted = false;
    private bool isHit = false;

    private Image[] manaGems;
    public Image telegraph1;
    public Image telegraph2;
    private Slider manaBar;

    private Image[] lifePlayer1;
    private Image[] lifePlayer2;

    private Text damageTextPlayer1;
    private Text damageTextPlayer2;

    private bool host;

    public Animator anim;
    public GameObject endGameText;

    public GameObject deathExplosion;
    private Animator animDeathExplosion;
    private SpriteRenderer renderDeathExplosion;
    private GameObject deathObj;

    [SyncVar]
    private int opponentDamage;
    private int lastDamage;

    //[SyncVar]
    //private float gravTimer;
    //[SyncVar]
    //private float opponentGravTimer;
    //private float totalGravTimer;

    //public float gravScale = 2.0f;
    //private float baseGravScale;
    
    public GameObject arrow;
    private GameObject myArrow;
    #endregion

    #region Get/Set functions
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
        set { playerState = value; }
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

    public int Lives
    {
        get { return lives; }
        set { lives = value; }
    }

    public int Mana
    {
        get { return actualMana; }
        set { actualMana = value; }
    }
    
    public bool IsHit
    {
        get { return isHit; }
        set { isHit = value; }
    }

    public float TimeStopTimer
    {
        get { return timeStopTimer; }
        set { timeStopTimer = value; }
    }

    public float GravityScale
    {
        get { return gravityScale; }
        set { gravityScale = value; }
    }

    public int OpponentDamage
    {
        get { return opponentDamage; }
        set { opponentDamage = value; }
    }

    public GameObject MyArrow
    {
        get { return myArrow; }
        set { myArrow = value; }
    }

    public int ArtArrayNum
    {
        get { return artArrayNum; }
        set
        {
            int arrayNum = value;
            if (arrayNum < 0)
            {
                arrayNum = 0;
            }
            else if (arrayNum > 6)
            {
                arrayNum = 6;
            }
            CmdSetArtArrayNum(arrayNum);
            artArrayNum = arrayNum;
        }
    }

    //public float GravTimer
    //{
    //    get { return gravTimer; }
    //    set { gravTimer = value; }
    //}

    //public float OpponentGravTimer
    //{
    //    get { return opponentGravTimer; }
    //    set { opponentGravTimer = value; }
    //}
    #endregion


    void Start()
    {
        if (playerNumber > 2 || playerNumber < 1)
        {
            playerNumber = 1;
        }

        defaultTime = Time.timeScale;
        timeStopTimer = 0.0f;
        opponentTimeStopTimer = 0.0f;

        //baseGravScale = GetComponent<Rigidbody2D>().gravityScale;
        //totalGravTimer = 0;
        //opponentGravTimer = 0;
        //gravTimer = 0;

        ArtArrayNum = 0;

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        localPlayerHealthScript = GetComponent<FighterHealthScript>();

        CmdSetLives(4);

        //set player state to 0, meaning they haven't lost or won
        CmdSetPlayerState(0);

        endGameText = GameObject.Find("EndGameText");
        //endGameText.GetComponent<Text>().text = "Not Ready\nPress spacebar to be ready";

        readied = false;
        matchStarted = false;

        manaGems = new Image[5];
        manaGems[0] = GameObject.Find("ManaGem1").GetComponent<Image>();
        manaGems[1] = GameObject.Find("ManaGem2").GetComponent<Image>();
        manaGems[2] = GameObject.Find("ManaGem3").GetComponent<Image>();
        manaGems[3] = GameObject.Find("ManaGem4").GetComponent<Image>();
        manaGems[4] = GameObject.Find("ManaGem5").GetComponent<Image>();

        manaBar = GameObject.Find("ManaPower").GetComponent<Slider>();

        lifePlayer1 = new Image[4];
        lifePlayer1[0] = GameObject.Find("Life1Player1").GetComponent<Image>();
        lifePlayer1[1] = GameObject.Find("Life2Player1").GetComponent<Image>();
        lifePlayer1[2] = GameObject.Find("Life3Player1").GetComponent<Image>();
        lifePlayer1[3] = GameObject.Find("Life4Player1").GetComponent<Image>();

        lifePlayer2 = new Image[4];
        lifePlayer2[0] = GameObject.Find("Life1Player2").GetComponent<Image>();
        lifePlayer2[1] = GameObject.Find("Life2Player2").GetComponent<Image>();
        lifePlayer2[2] = GameObject.Find("Life3Player2").GetComponent<Image>();
        lifePlayer2[3] = GameObject.Find("Life4Player2").GetComponent<Image>();

        telegraph1 = GameObject.Find("TelegraphImage1").GetComponent<Image>();
        telegraph2 = GameObject.Find("TelegraphImage2").GetComponent<Image>();

        telegraph1.enabled = false;
        telegraph2.enabled = false;

        damageTextPlayer1 = GameObject.Find("DamageTextPlayer1").GetComponent<Text>();
        damageTextPlayer2 = GameObject.Find("DamageTextPlayer2").GetComponent<Text>();

        lastDamage = 0;
    }

    public override void OnStartLocalPlayer()
    {
        host = isServer;
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f);
        //CmdEnableRender();
        networkManager = GameObject.Find("Network Manager");
        networkManager.GetComponent<NetworkManagerHUD>().enabled = false;
        networkManager.GetComponent<CardEffects>().Initialize();
        playerNumber = networkManager.GetComponent<CharacterSelect>().GetPlayerNumber();

        playerMana = 1;
        Mana = 3;
        gravityScale = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //print(Time.timeSinceLevelLoad.ToString() + " - " + playerState);

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

            if (!isLocalPlayer)
            {
                return;
            }

            networkManager.GetComponent<CardEffects>().SetSources(gameObject);

            //SetCamera();

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

            //Checks player state
            CheckPlayerState();

            CheckInput();

            ManaSystem();

            if (!gameStarted && Opponent && lives == 4 && endGameText.GetComponent<Text>().text == "")
            {
                gameStarted = true;
            }

            if (Opponent)
            {
                #region EnemyTelegraphSetup
                o_NetFighterScript = opponent.GetComponent<NetworkFighterScript>();

                float opponentTimer = o_NetFighterScript.TimeStopTimer;

                if (opponentTimeStopTimer <= 0 && opponentTimer > opponentTimeStopTimer)
                {
                    opponentTimeStopTimer = opponentTimer;
                }
                if (opponentTimeStopTimer > 0)
                {
                    telegraph2.GetComponent<Image>().sprite = networkManager.GetComponent<CardSelect>().cardArt[o_NetFighterScript.ArtArrayNum];
                    telegraph2.enabled = true;
                }
                else
                {
                    telegraph2.enabled = false;
                }
                #endregion

                if (timeStopTimer <= 0)
                {
                    telegraph1.enabled = false;
                }

                if (telegraph1.enabled || telegraph2.enabled)
                {
                    //Time.timeScale = cardTimeScale;
                    // telegraph time slow code here, Dito
                }
                else
                {
                    //Time.timeScale = defaultTime;
                    // disable telegraph time slow code here, Dito
                }

                float nextTimeStopTimer = timeStopTimer - Time.deltaTime;

                CmdSetStopTimer(nextTimeStopTimer);
                timeStopTimer = nextTimeStopTimer;
                opponentTimeStopTimer -= Time.deltaTime;

                int dmg = o_NetFighterScript.OpponentDamage - lastDamage;

                if (dmg > 0)
                {
                    localPlayerHealthScript.CmdTakeDamage(dmg);
                    lastDamage += dmg;
                }
                
                //float myOppGravTimer = Opponent.GetComponent<NetworkFighterScript>().OpponentGravTimer;
                //print("oppgravtime: " + myOppGravTimer);
                //if (myOppGravTimer > totalGravTimer)
                //{
                //    print("triggered grav start");
                //    gravTimer = myOppGravTimer - totalGravTimer;
                //    totalGravTimer = myOppGravTimer;
                //}
                //if (gravTimer > 0)
                //{
                //    print("grav increased");
                //    GetComponent<Rigidbody2D>().gravityScale = gravityScale;
                //    gravTimer -= Time.deltaTime;
                //    if (gravTimer <= 0)
                //    {
                //        GetComponent<Rigidbody2D>().gravityScale = baseGravScale;
                //    }
                //}
            }
        }

        //run this code if the player has won
        if (gameStarted && playerState == 1)
        {
            endGameText.SetActive(true);

            endGameText.GetComponent<Text>().text = "You Win!\nPress Any Button to Exit";

            if (Input.anyKey)
            {
                Debug.Break();
                Application.Quit();
            }
        }

        //run this code if the player has lost
        if (gameStarted && playerState == 2)
        {
            endGameText.SetActive(true);

            endGameText.GetComponent<Text>().text = "You Lose...\nPress Any Button to Exit";

            Reset();

            rigid.bodyType = RigidbodyType2D.Static;

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
            IsHit = false;
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

        localPlayerHealthScript.CmdReset();

        playerSpeed = 10;

        playerMana = 1;
    }

    private void FullReset()
    {
        transform.position = new Vector3(0.0f, 2.5f, -1.0f);

        rigid.velocity = new Vector2();

        localPlayerHealthScript.CmdReset();

        playerSpeed = 10;

        playerMana = 1;

        CmdSetLives(4);

        CmdSetPlayerState(0);
    }

    private void CheckBoundaries()
    {
        //If the player position is outside the boundaries of the stage, reset them to the stage and show death FX accordingly
        if (transform.position.x < -40.0f)
        {
            deathObj = Instantiate(deathExplosion);

            renderDeathExplosion = deathObj.GetComponent<SpriteRenderer>();
            animDeathExplosion = deathObj.GetComponent<Animator>();

            deathExplosion.transform.position = new Vector3(-27f, transform.position.y, -2);
            deathExplosion.transform.eulerAngles = new Vector3(0, 0, -90.0f);
            renderDeathExplosion.enabled = true;
            animDeathExplosion.Play(0);
            Reset();
            CmdSetLives(Lives - 1);
        }
        if (transform.position.x > 44.0f)
        {
            deathObj = Instantiate(deathExplosion);

            renderDeathExplosion = deathObj.GetComponent<SpriteRenderer>();
            animDeathExplosion = deathObj.GetComponent<Animator>();

            deathExplosion.transform.position = new Vector3(29.5f, transform.position.y, -2);
            deathExplosion.transform.eulerAngles = new Vector3(0, 0, 90.0f);
            renderDeathExplosion.enabled = true;
            animDeathExplosion.Play(0);
            Reset();
            CmdSetLives(Lives - 1);
        }
        if (transform.position.y < -18.0f)
        {
            deathObj = Instantiate(deathExplosion);

            renderDeathExplosion = deathObj.GetComponent<SpriteRenderer>();
            animDeathExplosion = deathObj.GetComponent<Animator>();

            deathExplosion.transform.position = new Vector3(transform.position.x, -3.5f, -2);
            deathExplosion.transform.eulerAngles = new Vector3(0, 0, 0);

            renderDeathExplosion.enabled = true;
            animDeathExplosion.Play(0);
            Reset();
            CmdSetLives(Lives - 1);
        }
        if (transform.position.y > 24.0f)
        {
            deathObj = Instantiate(deathExplosion);

            renderDeathExplosion = deathObj.GetComponent<SpriteRenderer>();
            animDeathExplosion = deathObj.GetComponent<Animator>();

            deathExplosion.transform.position = new Vector3(transform.position.x, 10.0f, -2);
            deathExplosion.transform.eulerAngles = new Vector3(0, 0, -180.0f);
            renderDeathExplosion.enabled = true;
            animDeathExplosion.Play(0);
            Reset();
            CmdSetLives(Lives - 1);
        }
    }

    private void CheckJump()
    {
        //if the player is on the ground and you are pressing space or A, jump
        if ((anim.GetBool("isGrounded") && Input.GetButtonDown("Jump")))
        {
            anim.SetBool("isJumping", true);
            rigid.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        }

        //if the player is falling, set the falling speed using the fallMaultiplier value
        if (rigid.velocity.y < 0)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
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
        if(isHit)
        {
            rigid.AddForce(new Vector3(playerHitSpeed * inputX, 0, 0), ForceMode2D.Impulse);
        }
        else
        {
            rigid.velocity = new Vector2(playerSpeed * inputX, rigid.velocity.y);
        }

        //clamps player's velocity to the playerSpeed 1.5x
        rigid.velocity = Vector2.ClampMagnitude(rigid.velocity, playerSpeed * 1.5f);
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
            GameObject damageTextPlayer1 = GameObject.Find("DamageTextPlayer1");
            //Set % of local player dmg
            if (damageTextPlayer1)
                damageTextPlayer1.GetComponent<Text>().text = localPlayerHealthScript.Damage.ToString() + "%";

            //Enables/Disables Image component based on how many lives player has
            switch (Lives)
            {
                case 0:
                    lifePlayer1[0].enabled = false;
                    lifePlayer1[1].enabled = false;
                    lifePlayer1[2].enabled = false;
                    lifePlayer1[3].enabled = false;
                    
                    break;

                case 1:
                    lifePlayer1[0].enabled = true;
                    lifePlayer1[1].enabled = false;
                    lifePlayer1[2].enabled = false;
                    lifePlayer1[3].enabled = false;
                    
                    break;

                case 2:
                    lifePlayer1[0].enabled = true;
                    lifePlayer1[1].enabled = true;
                    lifePlayer1[2].enabled = false;
                    lifePlayer1[3].enabled = false;
                    
                    break;
                case 3:
                    lifePlayer1[0].enabled = true;
                    lifePlayer1[1].enabled = true;
                    lifePlayer1[2].enabled = true;
                    lifePlayer1[3].enabled = false;

                    break;

                case 4:
                    lifePlayer1[0].enabled = true;
                    lifePlayer1[1].enabled = true;
                    lifePlayer1[2].enabled = true;
                    lifePlayer1[3].enabled = true;

                    break;
            }
        

            //Mana "Power" Bar
            manaBar.value = playerMana;
            //Enables/Disables Image component based on how much mana the player has
            switch(Mana)
            {
                case 0:
                    manaGems[0].enabled = false;
                    manaGems[1].enabled = false;
                    manaGems[2].enabled = false;
                    manaGems[3].enabled = false;
                    manaGems[4].enabled = false;
                    break;
                case 1:
                    manaGems[0].enabled = true;
                    manaGems[1].enabled = false;
                    manaGems[2].enabled = false;
                    manaGems[3].enabled = false;
                    manaGems[4].enabled = false;
                    break;
                case 2:
                    manaGems[0].enabled = true;
                    manaGems[1].enabled = true;
                    manaGems[2].enabled = false;
                    manaGems[3].enabled = false;
                    manaGems[4].enabled = false;
                    break;
                case 3:
                    manaGems[0].enabled = true;
                    manaGems[1].enabled = true;
                    manaGems[2].enabled = true;
                    manaGems[3].enabled = false;
                    manaGems[4].enabled = false;
                    break;
                case 4:
                    manaGems[0].enabled = true;
                    manaGems[1].enabled = true;
                    manaGems[2].enabled = true;
                    manaGems[3].enabled = true;
                    manaGems[4].enabled = false;
                    break;
                case 5:
                    manaGems[0].enabled = true;
                    manaGems[1].enabled = true;
                    manaGems[2].enabled = true;
                    manaGems[3].enabled = true;
                    manaGems[4].enabled = true;
                    break;
            }

            if (opponent)
            {
                GameObject damageTextPlayer2 = GameObject.Find("DamageTextPlayer2");
                //Set % of opponent dmg
                if (damageTextPlayer2)
                    damageTextPlayer2.GetComponent<Text>().text = Opponent.GetComponent<FighterHealthScript>().Damage.ToString() + "%";

                //Enables/Disables Image component based on how many lives player has
                switch (o_NetFighterScript.Lives)
                {
                    case 0:
                        lifePlayer2[0].enabled = false;
                        lifePlayer2[1].enabled = false;
                        lifePlayer2[2].enabled = false;
                        lifePlayer2[3].enabled = false;

                        break;

                    case 1:
                        lifePlayer2[0].enabled = true;
                        lifePlayer2[1].enabled = false;
                        lifePlayer2[2].enabled = false;
                        lifePlayer2[3].enabled = false;

                        break;

                    case 2:
                        lifePlayer2[0].enabled = true;
                        lifePlayer2[1].enabled = true;
                        lifePlayer2[2].enabled = false;
                        lifePlayer2[3].enabled = false;

                        break;
                    case 3:
                        lifePlayer2[0].enabled = true;
                        lifePlayer2[1].enabled = true;
                        lifePlayer2[2].enabled = true;
                        lifePlayer2[3].enabled = false;

                        break;

                    case 4:
                        lifePlayer2[0].enabled = true;
                        lifePlayer2[1].enabled = true;
                        lifePlayer2[2].enabled = true;
                        lifePlayer2[3].enabled = true;

                        break;
                }


                o_NetFighterScript.CorrectFlip();
            }
        }
        else if (playerNumber == 2)
        {
            GameObject damageTextPlayer2 = GameObject.Find("DamageTextPlayer2");
            //Set % of local player dmg
            if (damageTextPlayer2)
                damageTextPlayer2.GetComponent<Text>().text = localPlayerHealthScript.Damage.ToString() + "%";
           
            //Enables/Disables Image component based on how many lives player has
            switch (Lives)
            {
                case 0:
                    lifePlayer2[0].enabled = false;
                    lifePlayer2[1].enabled = false;
                    lifePlayer2[2].enabled = false;
                    lifePlayer2[3].enabled = false;

                    break;

                case 1:
                    lifePlayer2[0].enabled = true;
                    lifePlayer2[1].enabled = false;
                    lifePlayer2[2].enabled = false;
                    lifePlayer2[3].enabled = false;

                    break;

                case 2:
                    lifePlayer2[0].enabled = true;
                    lifePlayer2[1].enabled = true;
                    lifePlayer2[2].enabled = false;
                    lifePlayer2[3].enabled = false;

                    break;
                case 3:
                    lifePlayer2[0].enabled = true;
                    lifePlayer2[1].enabled = true;
                    lifePlayer2[2].enabled = true;
                    lifePlayer2[3].enabled = false;

                    break;

                case 4:
                    lifePlayer2[0].enabled = true;
                    lifePlayer2[1].enabled = true;
                    lifePlayer2[2].enabled = true;
                    lifePlayer2[3].enabled = true;

                    break;
            }


            //Mana "Power" Bar
            manaBar.value = playerMana;
            //Enables/Disables Image component base on how much mana the player has
            switch (Mana)
            {
                case 0:
                    manaGems[0].enabled = false;
                    manaGems[1].enabled = false;
                    manaGems[2].enabled = false;
                    manaGems[3].enabled = false;
                    manaGems[4].enabled = false;
                    break;
                case 1:
                    manaGems[0].enabled = true;
                    manaGems[1].enabled = false;
                    manaGems[2].enabled = false;
                    manaGems[3].enabled = false;
                    manaGems[4].enabled = false;
                    break;
                case 2:
                    manaGems[0].enabled = true;
                    manaGems[1].enabled = true;
                    manaGems[2].enabled = false;
                    manaGems[3].enabled = false;
                    manaGems[4].enabled = false;
                    break;
                case 3:
                    manaGems[0].enabled = true;
                    manaGems[1].enabled = true;
                    manaGems[2].enabled = true;
                    manaGems[3].enabled = false;
                    manaGems[4].enabled = false;
                    break;
                case 4:
                    manaGems[0].enabled = true;
                    manaGems[1].enabled = true;
                    manaGems[2].enabled = true;
                    manaGems[3].enabled = true;
                    manaGems[4].enabled = false;
                    break;
                case 5:
                    manaGems[0].enabled = true;
                    manaGems[1].enabled = true;
                    manaGems[2].enabled = true;
                    manaGems[3].enabled = true;
                    manaGems[4].enabled = true;
                    break;
            }


            if (opponent)
            {
                GameObject damageTextPlayer1 = GameObject.Find("DamageTextPlayer1");
                //Set % of opponent dmg
                if (damageTextPlayer1)
                    damageTextPlayer1.GetComponent<Text>().text = localPlayerHealthScript.Damage.ToString() + "%";

                //Enables/Disables Image component based on how many lives player has
                switch (o_NetFighterScript.Lives)
                {
                    case 0:
                        lifePlayer1[0].enabled = false;
                        lifePlayer1[1].enabled = false;
                        lifePlayer1[2].enabled = false;
                        lifePlayer1[3].enabled = false;

                        break;

                    case 1:
                        lifePlayer1[0].enabled = true;
                        lifePlayer1[1].enabled = false;
                        lifePlayer1[2].enabled = false;
                        lifePlayer1[3].enabled = false;

                        break;

                    case 2:
                        lifePlayer1[0].enabled = true;
                        lifePlayer1[1].enabled = true;
                        lifePlayer1[2].enabled = false;
                        lifePlayer1[3].enabled = false;

                        break;
                    case 3:
                        lifePlayer1[0].enabled = true;
                        lifePlayer1[1].enabled = true;
                        lifePlayer1[2].enabled = true;
                        lifePlayer1[3].enabled = false;

                        break;

                    case 4:
                        lifePlayer1[0].enabled = true;
                        lifePlayer1[1].enabled = true;
                        lifePlayer1[2].enabled = true;
                        lifePlayer1[3].enabled = true;

                        break;
                }


                o_NetFighterScript.CorrectFlip();
            }
        }
    }

    public void ManaSystem()
    {
        if (Opponent)
        {
            playerMana += Time.deltaTime;
            manaDisplay = (int)playerMana;
            
            if (playerMana >= 7.5)
            {
                Mana = Mana + 1;
                playerMana = 0;
            }
            if (Mana <= -1)
            {
                Mana = 0;
            }
            if (Mana >= 6)
            {
                Mana = 5;
            }
        }
    }

    public void CheckPlayerState()
    {
        //if the player loses all their lives, they lose
        if (gameStarted && lives <= 0)
        {
            //set player state to 2, meaning they lost
            CmdSetPlayerState(2);
            print("check player state - loss");
        }

        //if your opponent loses, you win
        if (o_NetFighterScript && o_NetFighterScript.PlayerState == 2)
        {
            CmdSetPlayerState(1);
            print("check player state - win");
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
        if (Input.GetButtonDown("Punch") && anim.GetBool("isPlayingCard") == false)
        {
            Debug.Log("Punch!");
            anim.SetTrigger("hitPunch");
            anim.SetBool("isAttacking", true);
        }

        //press escape or the select button to quit
        if (Input.GetButton("Quit"))
        {
            Application.Quit();
        }
    }

    

    public void TeleportDir(float xDir, float yDir)
    {
        //Calculate the direction of the input
        Vector2 dir = new Vector2(xDir, yDir);
        //Calculate the magnitude of the
        float mag = dir.magnitude;

        //add the direction to the position with the max distance being 5, multiplied by the xDir and yDir (-1 to 1)
        transform.position += new Vector3(dir.x, dir.y, 0) * mag * 5;
    }

    [Command]
    public void CmdEnableRender()
    {
        GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
    }

    [Command]
    public void CmdSetLives(int num)
    {
        Lives = num;
    }

    [Command]
    public void CmdSetPlayerState(int num)
    {
        PlayerState = num;
    }

    [Command]
    public void CmdAddOpponentDamage(int amount)
    {
        OpponentDamage = OpponentDamage + amount;
    }

    [Command]
    public void CmdSetStopTimer(float time)
    {
        timeStopTimer = time;
    }

    [Command]
    public void CmdSetArtArrayNum(int num)
    {
        artArrayNum = num;
    }

    //[Command]
    //public void CmdSetGravTimer(float myTimer)
    //{
    //    GravTimer = myTimer;
    //}

    //[Command]
    //public void CmdAddOppGravTimer(float myTimer)
    //{
    //    OpponentGravTimer += myTimer;
    //}

    [Command]
    public void CmdSpawnArrow()
    {
        var myArrow = Instantiate(arrow, transform.position, Quaternion.identity);
        myArrow.GetComponent<ArrowScript>().SetSource(this);
        myArrow.GetComponent<ArrowScript>().Shoot(facingRight);
        NetworkServer.Spawn(myArrow);
    }
}