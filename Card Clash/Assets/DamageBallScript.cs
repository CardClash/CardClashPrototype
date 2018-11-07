using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBallScript : MonoBehaviour {

    private int damage;
    private Vector3 origin;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public Vector3 Origin
    {
        get { return origin; }
    }

    private void Start()
    {
        origin = transform.position;
    }
}
