using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemFollow : MonoBehaviour {

    public Sprite sprite;
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
        //ResetSphere();
    }

    public void ResetSphere()
    {
        sprite = playerScript.spriteToPass;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
