using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

    private NetworkFighterScript source;
    private float arrowSpeed;

    // Use this for initialization
    void Start () {
        arrowSpeed = 5;

        GetComponent<Rigidbody2D>().AddForce(new Vector2(source.transform.forward.x * arrowSpeed, 0));
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void SetSource(NetworkFighterScript _source)
    {
        source = _source;
    }
}
