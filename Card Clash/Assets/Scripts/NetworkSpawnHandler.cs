using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSpawnHandler : NetworkBehaviour {
    
    private NetworkManager manager;

	// Use this for initialization
	void Start ()
    {
        manager = GetComponent<NetworkManager>();
		if (NetworkInfo.Host)
        {
            manager.StartHost();
        }
        else if (!NetworkInfo.Host)
        {
            manager.networkAddress = NetworkInfo.IP;
            manager.StartClient();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
