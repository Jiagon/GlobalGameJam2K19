using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int treeGoal;

    List<TreeScript> trees;

    bool disastersEnabled;
    float fireTimer, fireTimerMax, windTimer, windTimerMax;

	// Use this for initialization
	void Start () {
        trees = new List<TreeScript>();

        GameObject[] allTreesInScene = GameObject.FindGameObjectsWithTag("Tree");
        foreach(GameObject t in allTreesInScene)
        {
            trees.Add(t.GetComponent<TreeScript>());
        }
        ResetTimers(fireTimer, fireTimerMax, 10f, 30f);
        ResetTimers(windTimer, windTimerMax, 5f, 20f);
    }

    // Update is called once per frame
    void Update() {
        if (trees.Count <= 0)
        {
            // Lose State
        }

        else if(trees.Count >= treeGoal)
        {
            // Win State
        }

        if (disastersEnabled)
        {
            fireTimer += Time.deltaTime;
            windTimer += Time.deltaTime;

            if(fireTimer >= fireTimerMax)
            {
                StartFire();
                ResetTimers(fireTimer, fireTimerMax, 10f, 30f);
            }
            if(windTimer >= windTimerMax)
            {
                StartWind();
                ResetTimers(windTimer, windTimerMax, 5f, 10f);
            }
        }
	}

    public void StartDisasters(TreeScript tree)
    {
        trees.Add(tree);
        if (!disastersEnabled)
        {
            int numElderTrees = 0;
            foreach (TreeScript t in trees)
            {
                if (t.currentStage >= 2)
                    ++numElderTrees;
            }
            if (trees.Count > 9 && numElderTrees > 4)
            {
                disastersEnabled = true;
                ResetTimers(fireTimer, fireTimerMax, 10f, 30f);
                ResetTimers(windTimer, windTimerMax, 5f, 20f);
                Debug.Log("Disasters now enabled!");
            }
        }
    }

    public void ResetTimers(float t1, float t2, float r1, float r2)
    {
        t1 = 0;
        t2 = Random.Range(r1, r2);
    }

    public void StartFire()
    {
        if (trees.Count <= 0)
            return;
        bool validTree = false;
        int whichTree = 0;
        while (!validTree)
        {
            whichTree = Random.Range(0, trees.Count);
            if (trees[whichTree].currentStage >= 2)
                validTree = true;
        }
        trees[whichTree].isOnFire = true;
    }
    public void StartWind()
    {
        if (trees.Count <= 0)
            return;
        bool validTree = false;
        int whichTree = 0;
        while (!validTree)
        {
            whichTree = Random.Range(0, trees.Count);
            if (trees[whichTree].currentStage <= 1)
                validTree = true;
        }
        trees[whichTree].isWinded = true;
    }
}
