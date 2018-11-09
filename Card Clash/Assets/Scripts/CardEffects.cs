﻿using System.Collections;
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
    private GameObject damageBall;

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

    public void Initialize()
    {
        if (initialized)
        {
            return;
        }

        damageBall = GameObject.Find("DamageBall");

        keyList = new int[6];
        keyList[0] = 10;
        keyList[1] = 11;
        keyList[2] = 12;
        keyList[3] = 13;
        keyList[4] = 14;
        keyList[5] = 15;

        database = new Dictionary<int, System.Action>
        {
            { 10, TakeDamage },
            { 11, SpeedBoost },
            { 12, HealSelf },
            { 13, Teleport },
            { 14, GravityIncrease },
            { 15, TakeBigDamage }
        };

        cardNames = new Dictionary<int, string>
        {
            { 10, "Damage 10" },
            { 11, "Speed Up" },
            { 12, "Heal Up" },
            { 13, "Gravity Increase" },
            { 15, "Damage 25" }
        };

        cardTexts = new Dictionary<int, string>
        {
            { 10, "Opponent takes 10 damage." },
            { 11, "Your speed increases slightly." },
            { 12, "You heal for up to 10 damage." },
            { 13, "Your opponent's gravity is now higher." },
            { 15, "Opponent takes 25 damage." }
        };

        manaCosts = new Dictionary<int, string>
        {
            { 10, "1" },
            { 11, "2" },
            { 12, "2" },
            { 13, "2" },
            { 14, "1" },
            { 15, "2" }
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
        //source.timeStopTimer = 1.5f;
        //source.telegraph.enabled = true;


       /* if (source.timeStopTimer <= 0.0f)
        {
            Time.timeScale = 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = 0.5f;
            source.timeStopTimer = 1.5f;
        }
        */
    }

    public void PlayCard(int id)
    {
        //source.TimeStop();

        foreach (int key in keyList)
        {
            if (key == id)
            {
                database[key]();
            }
        }
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
                source.Opponent.GetComponent<FighterHealthScript>().CmdTakeDamage(10);
                source.Mana -= manaCost;
            }
            else
            {
                played = true;
                //Replace the following lines with a Cmd method call in the NetworkFighterScript

                /*
                damageBall.GetComponent<DamageBallScript>().Damage = 10;
                damageBall.transform.position = source.Opponent.transform.position;
                */
                print("Take Damage");
                source.ApplyCardDamage(10);
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

        if (manaCost <= source.Mana)
        {
            //played = true;
            //source.Opponent.GetComponent<FighterHealthScript>().CmdTakeDamage(25);
            //source.Mana -= manaCost;
            if (source.isServer)
            {
                played = true;
                source.Opponent.GetComponent<FighterHealthScript>().CmdTakeDamage(25);
                source.Mana -= manaCost;
            }
            else
            {
                played = true;
                damageBall.GetComponent<DamageBallScript>().Damage = 25;
                damageBall.transform.position = source.Opponent.transform.position;
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
            print(manaCost);
            print(source.Mana);
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
            print(manaCost);
            print(source.Mana);
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
        float timer = 0;

        while(timer <= 5)
        {

            timer += Time.deltaTime;
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
