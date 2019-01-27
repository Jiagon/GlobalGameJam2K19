﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int goalTreeCount;
    public int treeGoal;
    public Text winLose;
    public Button restart;
    public RawImage backgroundWinLose;

    List<TreeScript> trees = new List<TreeScript>();
    List<TreeScript> protectedTrees = new List<TreeScript>();
    public GameObject firePrefab;
    public GameObject windPrefab;

    bool disastersEnabled;
    float fireTimer, fireTimerMax, windTimer, windTimerMax;

	// Use this for initialization
	void Start () {

        restart.onClick.AddListener(RestartGame);

        trees = new List<TreeScript>();

        GameObject[] allTreesInScene = GameObject.FindGameObjectsWithTag("Tree");
        foreach(GameObject t in allTreesInScene)
        {
            trees.Add(t.GetComponent<TreeScript>());
        }
        fireTimer = 0;
        windTimer = 0;
        fireTimerMax = Random.Range(30f, 50f);
        windTimerMax = Random.Range(5f, 10f);
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(winLose.IsActive());
        if (trees.Count <= 0)
        {
            backgroundWinLose.gameObject.SetActive(true);
            winLose.text = "You Lose";
            restart.gameObject.SetActive(true);
            restart.gameObject.GetComponent<Button>().enabled = true;
        }

        else if(goalTreeCount >= treeGoal)
        {
            Debug.Log("Win");
            backgroundWinLose.gameObject.SetActive(true);
            winLose.text = "You Win";
            restart.gameObject.SetActive(true);
            restart.gameObject.GetComponent<Button>().enabled = true;
        }

        else
        {
            backgroundWinLose.gameObject.SetActive(false);
            winLose.text = "";
            restart.enabled = false;
            restart.gameObject.GetComponent<Button>().enabled = false;
        }

        if (disastersEnabled)
        {
            fireTimer += Time.deltaTime;
            windTimer += Time.deltaTime;

            if(fireTimer >= fireTimerMax)
            {
                StartFire();
                fireTimer = 0;
                fireTimerMax = Random.Range(30f, 50f);
            }
            if(windTimer >= windTimerMax)
            {
                StartWind();
                windTimer = 0;
                windTimerMax = Random.Range(5f, 10f);
            }
        }
	}

    private void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
	}
	
    public void StartDisasters(TreeScript tree, bool protect)
    {
        if(tree != null)
            trees.Add(tree);
        if (protect)
        {
            protectedTrees.Add(tree);
        }
        if (!disastersEnabled && trees.Count > 0)
        {
            int numElderTrees = 0;
            foreach (TreeScript t in trees)
            {
                if (t.currentStage >= 2)
                    ++numElderTrees;
            }
            //if (trees.Count > 9 && numElderTrees > 4)
            if (numElderTrees > 0)
            {
                disastersEnabled = true;
                fireTimer = 0;
                windTimer = 0;
                fireTimerMax = Random.Range(10f, 20f); // TODO: change back to 30f, 50f
                windTimerMax = Random.Range(5f, 10f);
            }
        }
    }
    

    public void StartFire()
    {
        if (trees.Count <= 0)
            return;
        bool validTree = false;
        int whichTree = 0;
        int numTimesTriedToStart = 0;
        while (!validTree && numTimesTriedToStart < 5)
        {
            whichTree = Random.Range(0, trees.Count);
            if (trees[whichTree].currentStage >= 2 && !trees[whichTree].isOnFire && !protectedTrees.Contains(trees[whichTree]))
            {
                validTree = true;
            }
            ++numTimesTriedToStart;
        }
        if (validTree)
        {
            trees[whichTree].isOnFire = true;
            trees[whichTree].firePrefab = Instantiate(firePrefab, trees[whichTree].transform.position, Quaternion.identity);
            trees[whichTree].firePrefab.transform.localScale = new Vector3((float)trees[whichTree].GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2,
                                                                           (float)trees[whichTree].GetComponent<SpriteRenderer>().sprite.bounds.size.y,
                                                                           1);
        }

    }
    public void StartWind()
    {
        if (trees.Count <= 0)
            return;
        bool validTree = false;
        int whichTree = 0;
        int numTimesTriedToStart = 0;
        while (!validTree && numTimesTriedToStart < 5)
        {
            whichTree = Random.Range(0, trees.Count);
            if (trees[whichTree].currentStage <= 1 && !protectedTrees.Contains(trees[whichTree]))
                validTree = true;
            ++numTimesTriedToStart;
        }
        //trees[whichTree].isWinded = true;
    }

    public void TreeDeath(TreeScript tree)
    {
        trees.Remove(tree);
    }
}
