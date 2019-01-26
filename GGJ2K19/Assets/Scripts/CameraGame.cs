using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGame : MonoBehaviour {

    public GameObject target;
    public Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - target.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 position = target.transform.position;
        position += offset;
        transform.position = position;
        transform.LookAt(target.transform);
	}
}
