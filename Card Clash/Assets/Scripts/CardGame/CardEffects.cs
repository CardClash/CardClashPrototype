using System.Collections;
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
        keyList = new int[4];
        keyList[0] = 10;
        keyList[1] = 11;
        keyList[2] = 12;
        keyList[3] = 13;

        database = new Dictionary<int, System.Action>
        {
            { 10, TakeDamage },
            { 11, SpeedBoost }
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
        health.TakeDamage(10);
        print("took damage");

    }

    void SpeedBoost()
    {
        source.playerSpeed = source.playerSpeed + 1;
        print("speed increased by 1");
    }
}
