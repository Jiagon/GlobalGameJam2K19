using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemFollow : MonoBehaviour {

    Material color;
    public GameObject player;
    Player playerScript;

    private void Awake()
    {
        playerScript = player.GetComponent<Player>();
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.position = playerScript.prevPosition;
	}
    
    private void OnEnable()
    {
        color = playerScript.colorToPass;
        this.gameObject.GetComponent<Renderer>().material = color;
    }
}
