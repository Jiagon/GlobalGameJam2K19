using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(StartGame);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void StartGame()
    {
        SceneManager.LoadScene("InstructionsScene");
    }
}
