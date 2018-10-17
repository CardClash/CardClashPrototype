using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour {



    public float mana;

    // Use this for initialization
    void Start()
    {

        mana = 1;        

    }

    // Update is called once per frame
    void Update()
    {
        mana = mana + Time.deltaTime;

        print(mana);
    }

   
}
