using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {



    public Dictionary<int, System.Action> database;
    public int[] keyList;

    // Use this for initialization
    void Start()
    {

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

    }

    // Update is called once per frame
    void Update()
    {
        PlayCard(11);
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
        
        print("took damage");

    }

    void SpeedBoost()
    {
        
        print("speed increased by 1");
    }
}
