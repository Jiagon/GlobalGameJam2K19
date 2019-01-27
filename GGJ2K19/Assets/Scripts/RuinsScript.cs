using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinsScript : MonoBehaviour {

    public bool reclaimed;
    public List<Sprite> sprites;

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().sprite = sprites[0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlantedTree()
    {
        reclaimed = true;
        GetComponent<SpriteRenderer>().sprite = sprites[1];
    }
}
