using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DamageBallScript : NetworkBehaviour {

    [SerializeField]
    private int damage;
    private Vector3 origin;
    private GameObject target = null;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public Vector3 Origin
    {
        get { return origin; }
    }

    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }

    private void Start()
    {
        origin = transform.position;
    }

    private void Update()
    {
        //if (target)
        //{
        //    transform.position = target.transform.position;
        //}
    }

    public void SetLocation(Vector3 nextPosition)
    {
        transform.position = nextPosition;
    }

    [Command]
    public void CmdSetLocation(Vector3 nextPosition)
    {
        transform.position = nextPosition;
    }

    public void ResetLoc()
    {
        transform.position = origin;
    }
    
    [Command]
    public void CmdResetLoc()
    {
        transform.position = origin;
    }

    [Command]
    public void CmdSetDamage(int amount)
    {
        Damage = amount;
    }
}
