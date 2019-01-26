using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePlayerDetection : MonoBehaviour {

    public GameObject player;
    private Player playerScript;
    public bool isPlayerWithinRadius;
	// Use this for initialization
	void Start () {
        playerScript = player.GetComponent<Player>();
        isPlayerWithinRadius = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isPlayerWithinRadius = true;
            playerScript.radiusObjects.Add(gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            isPlayerWithinRadius = false;
            playerScript.radiusObjects.Remove(gameObject);
            if(playerScript.radiusObjects.Count <= 0)
            {
                //playerScript.rb.transform.position = playerScript.prevPosition;
            }
        }
    }
}
