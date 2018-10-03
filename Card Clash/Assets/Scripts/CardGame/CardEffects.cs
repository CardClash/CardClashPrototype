﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffects : MonoBehaviour {

    private NetworkFighterScript source;
    private FighterHealthScript health;
    private Stack stack;
    private bool active = false;
    private bool initialized = false;

    public Dictionary<int, System.Action> database;
    public int[] keyList;

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
        stack = new Stack();
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
        source.Opponent.GetComponent<FighterHealthScript>().TakeDamage(10);
        //source.Opponent.GetComponent<FighterHealthScript>().CmdTakeDamage(10);
        print("took damage");
    }
    
    void TakeBigDamage()
    {
        source.Opponent.GetComponent<FighterHealthScript>().TakeDamage(25);
        print("took big damage");
    }

    void SpeedBoost()
    {
        source.playerSpeed = source.playerSpeed + 1;
        print("speed increased by 1");
    }

    void HealSelf()
    {
        health.TakeDamage(-10);
        print("healed self");
    }

    void Teleport()
    {
        if (source.facingRight == false)
        {
            source.transform.position = source.transform.position + new Vector3(1.5f, 0.0f, 0.0f);
        }
        else if (source.facingRight == true)
        {
            source.transform.position = source.transform.position + new Vector3(-1.5f, 0.0f, 0.0f);
        }
        print("Teleported Forward");
    }
    void TeleportBackwards()
    {
        if (source.facingRight == true)
        {
            source.transform.position = source.transform.position + new Vector3(1.5f, 0.0f, 0.0f);
        }
        else if (source.facingRight == false)
        {
            source.transform.position = source.transform.position + new Vector3(-1.5f, 0.0f, 0.0f);
        }
        print("Teleported Backwards");
    }
}
