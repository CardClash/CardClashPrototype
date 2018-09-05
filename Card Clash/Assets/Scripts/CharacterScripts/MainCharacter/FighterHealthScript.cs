using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour {

    public int startingHealth = 10;
    public int currentHealth;
    public Slider healthSlider;

    bool isDead;

	// Use this for initialization
	void Start ()
    {
        //set current health to the starting health amount
        currentHealth = startingHealth;
	}

    public void TakeDamage(int amount)
    {
        //reduce health by the amount of damage taken
        currentHealth -= amount;

        //set health bar to current health
        healthSlider.value = currentHealth;

        //if health is less than or equal to zero, the player is dead
        if(currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
    }

}
