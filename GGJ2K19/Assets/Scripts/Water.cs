using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    public GameObject player;
    private Player playerScript;

	// Use this for initialization
	void Start () {
        playerScript = player.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (Input.GetMouseButton(0))
            {
                if (!playerScript.hasResource)
                {
                    playerScript.hasResource = true;
                }
            }
        }
    }
}
