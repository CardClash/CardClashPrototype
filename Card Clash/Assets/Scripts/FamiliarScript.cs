using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FamiliarScript : MonoBehaviour
{
    public int startingPower;
    public int startingHealth;
    public Text myPowerText;
    public Text myHealthText;
    public Vector2 powerOffset;
    public Vector2 healthOffset;

    private int power;
    private int health;
    private int maxHealth;

    public int Power
    {
        get { return power; }
        set { power = value; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public int Health
    {
        get { return health; }
        set
        {
            if (value > maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health = value;
            }

            if (value <= 0)
            {
                Die();
            }
        }
    }

    void Start()
    {
        power = startingPower;
        maxHealth = startingHealth;
        health = maxHealth;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void Fight(FamiliarScript opponent)
    {
        Health -= opponent.Power;
        opponent.Health -= Power;
    }

    void Update()
    {
        myPowerText.text = Power.ToString();
        //myPowerText.rectTransform.position = new Vector3(GetComponent<Transform>().position.x + powerOffset.x, GetComponent<Transform>().position.y + powerOffset.y, myPowerText.transform.position.z);
        //Debug.Log(GetComponent<Transform>().position.x + powerOffset.x);
    }
}