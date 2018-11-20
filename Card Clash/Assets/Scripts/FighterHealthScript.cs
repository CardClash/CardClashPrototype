using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FighterHealthScript : NetworkBehaviour {

    public int startingPercentage = 0;
    [SyncVar]
    public int currentPercentage;
    private Rigidbody2D rigid;
    private NetworkFighterScript localPlayer;
    private GameObject hitObj;

    public GameObject hitEffect;
    private SpriteRenderer hitFXRenderer;
    private Animator hitFXAnimator;

    public int Damage
    {
        get
        {
            return currentPercentage;
        }
    }

	// Use this for initialization
	void Start ()
    {
        //Setting variables
        localPlayer = GetComponent<NetworkFighterScript>();

        //set current percentage to the starting percentage amount
        currentPercentage = startingPercentage;
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {   
        //Check if the hit effect is done, then delete it
        if(hitObj && !hitFXAnimator.GetCurrentAnimatorStateInfo(0).IsName("HitAnimation"))
        {
            Destroy(hitObj);
        }
    }

    public void TakeHitDamage(int amount, Vector2 dir)
    {
        print(amount);
        //increase the percentage by the amount of damage taken
        CmdTakeDamage(amount);
        
        //based off of the Smash Bros. series knockback calculation
        float knockback = (((((currentPercentage / 10) + ((currentPercentage * amount) / 20)) * 1.4f) + 18) * 15);

        localPlayer.IsHit = true;

        hitObj = Instantiate(hitEffect);

        hitFXRenderer = hitObj.GetComponent<SpriteRenderer>();
        hitFXAnimator = hitObj.GetComponent<Animator>();
        hitFXRenderer.enabled = true;
        hitObj.transform.position = transform.position;
        hitFXAnimator.Play(0);

        rigid.AddForce(new Vector2(knockback * dir.x, knockback), ForceMode2D.Force);
    }

    public void TakeDamage(int amount)
    {
        //increase the percentage by the amount of damage taken
        currentPercentage += amount;
        if (currentPercentage < 0)
        {
            currentPercentage = 0;
        }
    }

    [Command]
    public void CmdTakeDamage(int amount)
    {
        print(amount);
        TakeDamage(amount);
    }

    [ClientRpc]
    public void RpcTakeDamage(int amount)
    {
        if (isServer)
        {
            print("meh");
            return;
        }
        CmdTakeDamage(amount);
    }

    [Command]
    public void CmdReset()
    {
        currentPercentage = 0;
    }

}
