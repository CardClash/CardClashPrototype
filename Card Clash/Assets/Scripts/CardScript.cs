using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardScript : MonoBehaviour
{
    public int startingCost;

    private int cost;
    private int actualCost;

    public int Cost
    {
        get { return actualCost; }
        set
        {
            cost = value;
            if (cost < 0)
            {
                actualCost = 0;
            }
            else
            {
                actualCost = cost;
            }
        }
    }

	void Start ()
    {
        Cost = startingCost;
	}
	
	void Update ()
    {
		
	}

    public abstract void Cast();
}