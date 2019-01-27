using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public Rigidbody rb;
    private Vector3 position;
    public Vector3 prevPosition;
    public bool isWithinRadius;
    public List<GameObject> radiusObjects;
    public bool hasResource;
    public Vector3 direction;

    public int magicCount;
    public int seedCount;

    public Text magicText;
    public Text seedText;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        position = rb.transform.position;
        direction = rb.transform.forward;
        prevPosition = position;
        isWithinRadius = true;
        radiusObjects = new List<GameObject>();
        hasResource = false;
        magicText.text = "Magic: " + magicCount;
        seedText.text = "Seeds: " + seedCount;
	}
	
	// Update is called once per frame
	void Update () {
        /*if(radiusObjects.Count == 0)
        {
            isWithinRadius = false;
        }

        else
        {
            isWithinRadius = true;
        }*/

        /*int direction = 1;

        if (radiusObjects.Count <= 0/*isWithinRadius)
        {
            direction *= -1;
        }*/

        position = rb.transform.position;
        prevPosition = position;
        position.x += (Input.GetAxis("Horizontal")/10)/* * direction*/;
        position.z += (Input.GetAxis("Vertical")/10)/* * direction*/;
        direction = prevPosition - position;

        rb.transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Seed")
        {
            seedCount++;
            seedText.text = "Seeds: " + seedCount;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Water")
        {
            if (Input.GetMouseButtonDown(0))
            {
                hasResource = true;
            }
        }
    }
}
