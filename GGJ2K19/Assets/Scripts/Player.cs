using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Rigidbody rb;
    private Vector3 position;
    public Vector3 prevPosition;
    public bool isWithinRadius;
    public List<GameObject> radiusObjects;
    public bool hasResource;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        position = rb.transform.position;
        prevPosition = position;
        isWithinRadius = true;
        radiusObjects = new List<GameObject>();
        hasResource = false;
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

        int direction = 1;

        if (radiusObjects.Count <= 0/*isWithinRadius*/)
        {
            direction *= -1;
        }

        position = rb.transform.position;
        prevPosition = position;
        position.x += Input.GetAxis("Horizontal") * direction;
        position.z += Input.GetAxis("Vertical") * direction;

        rb.transform.position = position;
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
