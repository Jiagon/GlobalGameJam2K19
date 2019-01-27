﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeScript : MonoBehaviour {

    public GameManager gm;
    public GameObject player;
    Player playerScript;
    bool isPlayerWithinRadius;

    public GameObject seed;

    public List<Sprite> treeStages = new List<Sprite>();    // Prefab objects the trees will display as
    public List<int> waterStages = new List<int>();         // How much water is necessary for a stage to display
    public List<int> nutrientStages = new List<int>();      // How much nutriets are necessary for a stage to display
    public int currentStage;                                // Which stages of growth a tree is currently on


    public int surroundingNutrients;                        // How many nutrients per second a tree gets based on surrounding soil

    int waterLevel;                                         // How much water the tree currently has
    int nutrientLevel;                                      // How many nutrients the tree currently has
    float nutrientTimer;                                    // Keeps track of nutrient gain time
    float magicTimer;                                       // Keeps track of player magic gain time
    public int health;                                      // How much health a tree has - deteroiates due to environment disasters

    bool givenFirstSeeds;                                   // Whether a tree at stage 2 has dropped seeds yet
    bool givenSecondSeeds;                                  // Whether a tree at stage 3 has dropped seeds yet

    public GameObject firePrefab;
    public bool isOnFire;
    public bool isWinded;
    float fireTimer;
    float windTimer;


	// Use this for initialization
	void Start () {
        if (gm == null)
            gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        currentStage = 0;
        health = 10;
        UpdateSpriteRenderer();
	}
	
	// Update is called once per frame
	void Update () {
        if(health <= 0)
        {
            if (firePrefab != null)
                Destroy(firePrefab);
            gm.TreeDeath(this);
            Destroy(this.gameObject);
        }
        nutrientTimer += Time.deltaTime;
        magicTimer += Time.deltaTime;
        if (isOnFire)
        {
            fireTimer += Time.deltaTime;
            if(fireTimer >= 3.0f)
            {
                health -= 3;
                fireTimer = 0;
            }
        }
        if (isWinded)
        {
            windTimer += Time.deltaTime;
            if (windTimer >= 5.0f)
            {
                health -= 1;
                windTimer = 0;
            }
        }

        if (magicTimer >= 5.0f)
        {
            if (currentStage == 2)
            {
                ++playerScript.MagicCount;
            }
            else if (currentStage == 3)
            {
                playerScript.MagicCount += 3;
            }
            magicTimer = 0;
        }

        if (nutrientTimer >= 3.0f && surroundingNutrients > 0)
        {
            nutrientLevel += surroundingNutrients;
            nutrientTimer = 0;
        }
        
        if(waterLevel >= waterStages[currentStage] && nutrientLevel >= nutrientStages[currentStage] && currentStage < 3)
        {
            ++currentStage;
            UpdateSpriteRenderer();
            health += 5;
            if (currentStage == 2)
                Instantiate(seed, new Vector3(transform.position.x + Random.Range(-3, 3), .5f, transform.position.z + Random.Range(-3, 3)), Quaternion.identity);
            if (currentStage == 3)
            {
                Instantiate(seed, new Vector3(transform.position.x + Random.Range(-3, 3), .5f, transform.position.z + Random.Range(-3, 3)), Quaternion.identity);
                Instantiate(seed, new Vector3(transform.position.x + Random.Range(-3, 3), .5f, transform.position.z + Random.Range(-3, 3)), Quaternion.identity);
                Instantiate(seed, new Vector3(transform.position.x + Random.Range(-3, 3), .5f, transform.position.z + Random.Range(-3, 3)), Quaternion.identity);
            }
        }
	}

    void UpdateSpriteRenderer()
    {
        waterLevel = 0;
        nutrientLevel = 0;
        this.GetComponent<SpriteRenderer>().sprite = treeStages[currentStage];
        /*this.GetComponent<CapsuleCollider>().size = new Vector3((float)this.GetComponent<SpriteRenderer>().sprite.bounds.size.x,
                                                                (float)this.GetComponent<SpriteRenderer>().sprite.bounds.size.y,
                                                                (float)this.GetComponent<SpriteRenderer>().sprite.bounds.size.x);*/
        Sprite sprite = this.GetComponent<SpriteRenderer>().sprite;
        this.GetComponent<CapsuleCollider>().radius = (float)sprite.bounds.size.x / 2;
        this.GetComponent<CapsuleCollider>().height = (float)sprite.bounds.size.y;

        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,
                                                         ((float)sprite.bounds.size.y / 2f),// - ((float)sprite.bounds.size.y / 16f),
                                                         this.gameObject.transform.position.z);
        gm.StartDisasters(null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isPlayerWithinRadius = true;
            playerScript.radiusObjects.Add(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Input.GetKeyDown("space") && playerScript.hasResource && player.GetComponentInChildren<PlayerItemFollow>().sprite == playerScript.water)
        {
            ++waterLevel;
            playerScript.hasResource = false;
            playerScript.DisableItem();
            
            if (isOnFire)
            {
                isOnFire = false;
                if (firePrefab != null)
                    Destroy(firePrefab);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerWithinRadius = false;
            playerScript.radiusObjects.Remove(this.gameObject);
        }
    }

    /*private void OnMouseDown()
    {
        ++waterLevel;
        //++nutrientLevel;
        if (playerScript.hasResource)   // Change later to check for what kind of resource
        {
            playerScript.hasResource = false;
            ++waterLevel;
            Debug.Log("Current water level: " + waterLevel);
            Debug.Log("Current nutrient level: " + nutrientLevel);
            Debug.Log("Water level needed: " + waterStages[currentStage]);
            Debug.Log("Nutrient level needed: " + nutrientStages[currentStage]);
        }
/*
        // Quick hit detection courtesy of Kyle W Banks:
        // https://kylewbanks.com/blog/unity-2d-detecting-gameobject-clicks-using-raycasts
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.z);
        //RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (hit.collider == this.GetComponent<CapsuleCollider>())// && isPlayerWithinRadius)
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if (playerScript.hasResource)   // Change later to check for what kind of resource
            {
                playerScript.hasResource = false;
                ++waterLevel;
            }
        }
    }*/
}
