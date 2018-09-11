using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchScript : MonoBehaviour {

    public FighterHealthScript enemyHealth;
    private BoxCollider2D punchHitBox;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DamageEnemy()
    {
        //deal damage to the player
        //enemyHealth.TakeDamage(7);
    }
}
