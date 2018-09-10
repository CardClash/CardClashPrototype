using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanillaEffectScript : MonoBehaviour {

    private FighterScript source;
    private FighterHealthScript health;

	// Use this for initialization
	void Start () {
        source = GameObject.Find("Main Character").GetComponent<FighterScript>();
        health = GameObject.Find("Main Character").GetComponent<FighterHealthScript>();
    }
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            health.TakeDamage(10);
            print("took damage");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            source.playerSpeed = source.playerSpeed + 1;
            print("speed increased by 1");
        }

	}
}
