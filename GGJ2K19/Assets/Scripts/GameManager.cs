using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int treeCount;
    public int treeGoal;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (treeCount <= 0)
        {
            // Lose State
        }

        else if(treeCount >= treeGoal)
        {
            // Win State
        }
	}
}
