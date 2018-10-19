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

    [Command]
    public void CmdStartMatch()
    {
        print(NetworkServer.active);

        if (!NetworkServer.active)
        {
            return;
        }

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<NetworkFighterScript>().MatchStarted = true;
            player.GetComponent<NetworkFighterScript>().CmdEnableRender();
            if (player.GetComponent<NetworkFighterScript>().Host)
            {
                player.transform.position = new Vector3(-5, 3, player.transform.position.z);
            }
            else
            {
                player.transform.position = new Vector3(5, 3, player.transform.position.z);
            }
        }

        //opponent.GetComponent<NetworkFighterScript>().MatchStarted = true;
        //opponent.GetComponent<SpriteRenderer>().enabled = true;
        //transform.position = new Vector3(-5, 0, transform.position.z);
        //opponent.transform.position = new Vector3(5, 0, transform.position.z);
    }
}
