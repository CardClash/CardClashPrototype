using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CardEffects : NetworkBehaviour {

    private NetworkFighterScript source;
    private FighterHealthScript health;
  
    private bool initialized = false;
    public bool played;

    public Dictionary<int, System.Action> database;
    private Dictionary<int, string> cardNames;
    private Dictionary<int, string> cardTexts;
    private Dictionary<int, string> manaCosts;
    public int[] keyList;
    public int manaCost;

    public Dictionary<int, string> CardNames
    {
        get { return cardNames; }
    }

    public Dictionary<int, string> CardTexts
    {
        get { return cardTexts; }
    }

    public Dictionary<int, string> ManaCosts
    {
        get { return manaCosts; }
    }

    public NetworkFighterScript Source
    {
        get { return source; }
    }

    public void Initialize()
    {
        if (initialized)
        {
            return;
        }

        keyList = new int[7]
        {
            10,
            11,
            12,
            13,
            14,
            15,
            16
        };

        database = new Dictionary<int, System.Action>
        {
            { 10, TakeDamage },
            { 11, SpeedBoost },
            { 12, HealSelf },
            { 13, Teleport },
            { 14, TakeBigDamage },
            { 15, Arrow },
            { 16, GravityIncrease }
        };

        cardNames = new Dictionary<int, string>
        {
            { 10, "Damage 10" },
            { 11, "Speed Up" },
            { 12, "Heal Up" },
            { 13, "Teleport" },
            { 14, "Damage 25" },
            { 15, "Arrow" },
            { 16, "Gravity Increase" }
        };

        cardTexts = new Dictionary<int, string>
        {
            { 10, "Opponent takes 10 damage." },
            { 11, "Your speed increases slightly." },
            { 12, "You heal for up to 10 damage." },
            { 13, "You can teleport in any direction you point towards." },
            { 14, "Opponent takes 25 damage." },
            { 15, "You shoot an arrow." },
            { 16, "Your opponent's gravity is now higher." }
        };

        manaCosts = new Dictionary<int, string>
        {
            { 10, "1" },
            { 11, "2" },
            { 12, "2" },
            { 13, "2" },
            { 14, "2" },
            { 15, "1" },
            { 16, "1" }
        };

        source = GameObject.Find("Main Character (Network)(Clone)").GetComponent<NetworkFighterScript>();
        health = GameObject.Find("Main Character (Network)(Clone)").GetComponent<FighterHealthScript>();

        //played = 0;
        
        initialized = true;
    }

    public void SetSources(GameObject player)
    {
        source = player.GetComponent<NetworkFighterScript>();
        health = player.GetComponent<FighterHealthScript>();
    }

    public void TimeStop()
    {
        //Time.timeScale = 0.5f;
        source.timeStopTimer = 1.5f;
        //source.telegraph.enabled = true;

        //if (source.timeStopTimer <= 0.0f)
        //{
        //    Time.timeScale = 1.0f;
        //}

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    source.CmdSetStopTimer(1.5f);
        //    source.timeStopTimer = 1.5f;
        //}
        
    }

    public void PlayCard(int id)
    {
        foreach (int key in keyList)
        {
            if (key == id)
            {
                database[key]();
            }
        }
    }

    public void SetCardID(int id)
    {
        source.ArtArrayNum = id;
    }

    void TakeDamage()
    {
        //use source.Opponent to reference enemy player
        manaCost = 1;
        if (manaCost <= source.Mana && source.Opponent)
        {
            //played = true;
            //source.Opponent.GetComponent<FighterHealthScript>().CmdTakeDamage(10);
            //source.Mana -= manaCost;
            if (source.isServer)
            {
                played = true;
                source.Opponent.GetComponent<FighterHealthScript>().RpcTakeDamage(10);
                //source.Opponent.GetComponent<FighterHealthScript>().CmdTakeDamage(10);
                source.Mana -= manaCost;
            }
            else
            {
                played = true;
                //Replace the following lines with a Cmd method call in the NetworkFighterScript
                //source.Opponent.GetComponent<FighterHealthScript>().CmdMakeDamage(source.GetComponent<FighterHealthScript>().Damage + 10);
                source.CmdAddOpponentDamage(10);
                source.OpponentDamage = source.OpponentDamage + 10;
                //source.Opponent.GetComponent<FighterHealthScript>().CmdTakeDamage(10);
                //source.Opponent.GetComponent<FighterHealthScript>().CmdTakeDamage(10);
                //source.Opponent.GetComponent<FighterHealthScript>().CmdUpdateDamage();
                /*
                damageBall.GetComponent<DamageBallScript>().Damage = 10;
                damageBall.transform.position = source.Opponent.transform.position;
                */
                //source.ApplyCardDamage(10);
                source.Mana -= manaCost;
            }
        }
        else
        {
            played = false;
        }
    }
    
    void TakeBigDamage()
    {
        manaCost = 2;

        if (manaCost <= source.Mana && source.Opponent)
        {
            if (source.isServer)
            {
                played = true;
                source.Opponent.GetComponent<FighterHealthScript>().RpcTakeDamage(25);
                //source.Opponent.GetComponent<FighterHealthScript>().CmdTakeDamage(10);
                source.Mana -= manaCost;
            }
            else
            {
                played = true;
                source.CmdAddOpponentDamage(25);
                source.OpponentDamage = source.OpponentDamage + 25;
                source.Mana -= manaCost;
            }
        }
        else
        {
            played = false;
        }
    }

    void SpeedBoost()
    {
        manaCost = 2;
        if (manaCost <= source.Mana)
        {
            played = true;
            //print(manaCost);
            //print(source.Mana);
            source.playerSpeed = source.playerSpeed + 1;
            source.Mana -= manaCost;
        }
        else
        {
            played = false;
        }
    }

    void HealSelf()
    {
        manaCost = 2;

        if (manaCost <= source.Mana)
        {
            played = true;
            //print(manaCost);
            //print(source.Mana);
            health.CmdTakeDamage(-10);
            source.Mana -= manaCost;
        }
        else
        {
            played = false;
        }
    }

    void Teleport()
    {
        manaCost = 2;

        if (manaCost <= source.Mana)
        {
            played = true;
            source.TeleportDir(Input.GetAxis("Horizontal"));
            source.Mana -= manaCost;
        }
        else
        {
            played = false;
        }
    }

    //Sets gravity scale from 1 to 2 for a set amount of time to the opponent
    void GravityIncrease()
    {
        manaCost = 1;

        if (manaCost <= source.Mana)
        {
            played = true;

            if (source.isServer)
            {
                source.CmdSetGravTimer(10.0f);
            }
            else
            {
                source.CmdAddOppGravTimer(10.0f);
                source.OpponentGravTimer += 10.0f;
            }

            source.Mana -= manaCost;
        }
        else
        {
            played = false;
        }
    }

    //shoots an arrow (currently at a set strength and arc)
    void Arrow()
    {
        manaCost = 1;
        if(manaCost <= source.Mana)
        {
            played = true;

            //var myArrow = Instantiate(arrow, source.transform.position, Quaternion.identity);
            //myArrow.GetComponent<ArrowScript>().SetSource(source);
            //myArrow.GetComponent<ArrowScript>().Shoot(source.facingRight);
            //NetworkServer.Spawn(myArrow);
            source.CmdSpawnArrow();

            source.Mana -= manaCost;
        }
        else
        {
            played = false;
        }
    }

    //void Teleport()
    //{
    //    manaCost = 2;

    //    if (manaCost <= source.Mana)

    //    {
    //        played = true;
    //        print(manaCost);
    //        print(source.Mana);
    //        if (source.facingRight == false)
    //        {
    //            source.transform.position = source.transform.position + new Vector3(1.5f, 0.0f, 0.0f);
    //        }
    //        else if (source.facingRight == true)
    //        {
    //            source.transform.position = source.transform.position + new Vector3(-1.5f, 0.0f, 0.0f);
    //        }
    //        print("Teleported Forward");
    //        source.Mana -= manaCost;
    //    }

    //    else
    //    {
    //        played = false;
    //        print("can't play card");
    //    }

    //}

    //void TeleportBackwards()
    //{
    //    manaCost = 2;

    //    if (manaCost <= source.Mana)

    //    {
    //        played = true;
    //        print(manaCost);
    //        print(source.Mana);
    //        if (source.facingRight == true)
    //        {
    //            source.transform.position = source.transform.position + new Vector3(1.5f, 0.0f, 0.0f);
    //        }
    //        else if (source.facingRight == false)
    //        {
    //            source.transform.position = source.transform.position + new Vector3(-1.5f, 0.0f, 0.0f);
    //        }
    //        print("Teleported Backwards");
    //        source.Mana -= manaCost;
    //    }

    //    else
    //    {
    //        played = false;
    //        print("can't play card");
    //    }

    //}
}
