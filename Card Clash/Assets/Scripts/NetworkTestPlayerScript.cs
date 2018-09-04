using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkTestPlayerScript : NetworkBehaviour
{
    public float horizMag;
    public float vertMag;

    public override void OnStartLocalPlayer()
    {
        transform.position = new Vector3(Random.Range(-8, 8), 3, 0);
    }
    
    void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(horizMag, 0));
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-horizMag, 0));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, vertMag));
        }
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f);
    }
}
