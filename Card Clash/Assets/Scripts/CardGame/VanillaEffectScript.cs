using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanillaEffectScript : MonoBehaviour {

    private FighterScript source;
    private FighterHealthScript health;
    private Stack stack;
    private bool active = false;

	// Use this for initialization
	void Start () {
        source = GameObject.Find("Main Character").GetComponent<FighterScript>();
        health = GameObject.Find("Main Character").GetComponent<FighterHealthScript>();
        stack = new Stack();
    }
	
	// Update is called once per frame
	void Update () {
		
   
    // This is old code
    
    /*    if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            health.TakeDamage(10);
            print("took damage");

            //stack.Push(takeDamage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            source.playerSpeed = source.playerSpeed + 1;
            print("speed increased by 1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach(Object obj in st)
            {

            }
        }
    */
	}

  void takeDamage()
    {
            health.CmdTakeDamage(10);
            print("took damage");

    }

   void speedBoost()
    {
        source.playerSpeed = source.playerSpeed + 1;
        print("speed increased by 1");
    }
}
