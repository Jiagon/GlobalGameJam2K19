using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGame : MonoBehaviour {

    public GameObject target;
    public Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = new Vector3(0f, 5.215f, -7.448f); //transform.position - target.transform.position

    }
	
	// Update is called once per frame
	void Update () {
        Vector3 position = target.transform.position;
        position += offset;
        transform.position = position;
        transform.LookAt(target.transform);
	}
}
