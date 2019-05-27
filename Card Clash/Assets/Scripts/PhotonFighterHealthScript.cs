using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PhotonFighterHealthScript : MonoBehaviourPun, IPunObservable
{
    public SyncVars sync;
    public struct SyncVars
    {
        public int currentPercentage;
    }
    public int startingPercentage = 0;
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
            return sync.currentPercentage;
        }
        set
        {
            sync.currentPercentage = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        //Setting variables
        localPlayer = GetComponent<NetworkFighterScript>();

        //set current percentage to the starting percentage amount
        Damage = startingPercentage;
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

    }

    public void TakeHitDamage(int amount, Vector2 dir)
    {
        print(amount);
        //increase the percentage by the amount of damage taken
        CmdTakeDamage(amount);

        //based off of the Smash Bros. series knockback calculation
        float knockback = (((((Damage / 10) + ((Damage * amount) / 20)) * 1.4f) + 18) * 15);

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
        Damage += amount;
        if (Damage < 0)
        {
            Damage = 0;
        }
    }

    [Command]
    public void CmdTakeDamage(int amount)
    {
        //print(amount);
        TakeDamage(amount);
    }

    [ClientRpc]
    public void RpcTakeDamage(int amount)
    {
        if (isServer)
        {
            //print("meh");
            return;
        }
        CmdTakeDamage(amount);
    }

    [Command]
    public void CmdReset()
    {
        Damage = 0;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Damage);
        }
        else
        {
            Damage = (int)stream.ReceiveNext();
        }
    }
}
