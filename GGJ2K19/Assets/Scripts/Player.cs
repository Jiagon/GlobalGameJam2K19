﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Rigidbody rb;
    private Vector3 position;
    public Vector3 prevPosition;
    public bool isWithinRadius;
    public List<GameObject> radiusObjects;
    public bool hasResource;
    public Vector3 direction;

    public Material water;
    public Material colorToPass;
    public GameObject followingSphere;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        position = rb.transform.position;
        direction = rb.transform.forward;
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

        /*int direction = 1;

        if (radiusObjects.Count <= 0/*isWithinRadius)
        {
            direction *= -1;
        }*/

        position = rb.transform.position;
        prevPosition = position;
        position.x += (Input.GetAxis("Horizontal")/10)/* * direction*/;
        position.y = 0.78f;
        position.z += (Input.GetAxis("Vertical")/10)/* * direction*/;
        direction = prevPosition - position;

        if (direction.x > 0)
            GetComponent<SpriteRenderer>().flipX = false;
        else
            GetComponent<SpriteRenderer>().flipX = true;
        rb.transform.position = position;
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Water")
        {
            if (Input.GetKeyDown("space"))
            {
                hasResource = true;
                colorToPass = water;
                followingSphere.SetActive(true);
            }
        }
    }

    public void DisableItem()
    {
        followingSphere.SetActive(false);
    }
}
