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
        //keyList[4] = 14;
        keyList[5] = 15;

        database = new Dictionary<int, System.Action>
        {
            { 10, TakeDamage },
            { 11, SpeedBoost },
            { 12, HealSelf },
            { 13, Teleport },
            //{ 14, TeleportBackwards },
            { 15, TakeBigDamage }
        };

        cardNames = new Dictionary<int, string>
        {
            { 10, "Damage 10" },
            { 11, "Speed Up" },
            { 12, "Heal Up" },
            { 13, "Teleport Forward" },
            { 15, "Damage 25" }
        };

        cardTexts = new Dictionary<int, string>
        {
            { 10, "Opponent takes 10 damage." },
            { 11, "Your speed increases slightly." },
            { 12, "You heal for up to 10 damage." },
            { 13, "You teleport in the direction you're moving when you cast this card." },
            { 15, "Opponent takes 25 damage." }
        };

        manaCosts = new Dictionary<int, string>
        {
            { 10, "1" },
            { 11, "2" },
            { 12, "2" },
            { 13, "2" },
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
        if (manaCost <= source.Mana && source.Opponent)
        {
            played = true;
            source.Opponent.GetComponent<FighterHealthScript>().CmdTakeDamage(10);
            source.Mana -= manaCost;
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
            played = true;
            source.Opponent.GetComponent<FighterHealthScript>().CmdTakeDamage(25);
            source.Mana -= manaCost;
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
