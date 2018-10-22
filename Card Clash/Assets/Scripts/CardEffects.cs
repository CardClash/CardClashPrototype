using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffects : MonoBehaviour {

    private NetworkFighterScript source;
    private FighterHealthScript health;
  
    private bool initialized = false;
    public bool played;

    public Dictionary<int, System.Action> database;
    public int[] keyList;

    public int manaCost;

    public void Initialize()
    {
        if (initialized)
        {
            return;
        }
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
            { 14, TeleportBackwards },
            { 15, TakeBigDamage }
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

    void TakeDamage()
    {
        //use source.Opponent to reference enemy player
        manaCost = 1;

        if (manaCost <= source.actualMana)
        {
            played = true;
            source.Opponent.GetComponent<FighterHealthScript>().CmdTakeDamage(10);
            print("took damage");
            source.actualMana -= manaCost;
        }
        else
        {
            played = false;
            print("can't play card");
        }
       
    }
    
    void TakeBigDamage()
    {

        manaCost = 2;

        if (manaCost <= source.actualMana)
        {
            played = true;
            source.Opponent.GetComponent<FighterHealthScript>().CmdTakeDamage(25);
            print("took big damage");
            source.actualMana -= manaCost;
        }

        else
        {
            played = false;
            print("can't play card");
        }

    }

    void SpeedBoost()
    {
        manaCost = 2;
        if (manaCost <= source.actualMana)
        {
            played = true;
            print(manaCost);
            print(source.actualMana);
            source.playerSpeed = source.playerSpeed + 1;
            print("speed increased by 1");
            source.actualMana -= manaCost;
        }

          else
        {
            played = false;
            print("can't play card");
        }
    }

    void HealSelf()
    {
        manaCost = 2;

        if (manaCost <= source.actualMana)
        {
            played = true;
            print(manaCost);
            print(source.actualMana);
            health.CmdTakeDamage(-10);
            print("healed self");
            source.actualMana -= manaCost;
        }

        else
        {
            played = false;
            print("can't play card");
        }


    }

    void Teleport()
    {
        manaCost = 2;

        if (manaCost <= source.actualMana)

        {
            played = true;
            print(manaCost);
            print(source.actualMana);
            if (source.facingRight == false)
            {
                source.transform.position = source.transform.position + new Vector3(1.5f, 0.0f, 0.0f);
            }
            else if (source.facingRight == true)
            {
                source.transform.position = source.transform.position + new Vector3(-1.5f, 0.0f, 0.0f);
            }
            print("Teleported Forward");
            source.actualMana -= manaCost;
        }

        else
        {
            played = false;
            print("can't play card");
        }
        
    }
    void TeleportBackwards()
    {
        manaCost = 2;

        if (manaCost <= source.actualMana)

        {
            played = true;
            print(manaCost);
            print(source.actualMana);
            if (source.facingRight == true)
            {
                source.transform.position = source.transform.position + new Vector3(1.5f, 0.0f, 0.0f);
            }
            else if (source.facingRight == false)
            {
                source.transform.position = source.transform.position + new Vector3(-1.5f, 0.0f, 0.0f);
            }
            print("Teleported Backwards");
            source.actualMana -= manaCost;
        }

        else
        {
            played = false;
            print("can't play card");
        }
        
    }
}
