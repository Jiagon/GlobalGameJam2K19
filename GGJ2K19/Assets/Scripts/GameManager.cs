using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int treeCount;
    public int treeGoal;
    public Text winLose;
    public Button restart;

	// Use this for initialization
	void Start () {
        restart.onClick.AddListener(RestartGame);
	}

    // Update is called once per frame
    void Update() {
        if (treeCount <= 0)
        {
            winLose.text = "You Lose";
            winLose.enabled = true;
            restart.enabled = true;
        }

        else if(treeCount >= treeGoal)
        {
            winLose.text = "You Win";
            winLose.enabled = true;
            restart.enabled = true;
        }

        else
        {
            winLose.enabled = false;
            restart.enabled = false;
        }
	}

    private void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
