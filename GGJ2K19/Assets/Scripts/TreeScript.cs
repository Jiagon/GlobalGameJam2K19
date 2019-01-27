using System.Collections;
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
    int magicLevel;
    public int health;                                      // How much health a tree has - deteroiates due to environment disasters

    bool givenFirstSeeds;                                   // Whether a tree at stage 2 has dropped seeds yet
    bool givenSecondSeeds;                                  // Whether a tree at stage 3 has dropped seeds yet


    public GameObject firePrefab;
    public bool isOnFire;
    public bool isWinded;
    float fireTimer;
    float windTimer;

    public GameObject ruins;

    public Canvas canvas;
    public Text nutrientsText;
    public Text waterText;
    public Text healthText;
    //public Text magicOutputText;

    // Use this for initialization
    void Start()
    {
        if (gm == null)
            gm = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        playerScript = player.GetComponent<Player>();
        currentStage = 0;
        health = 10;
        UpdateSpriteRenderer();
        nutrientsText = gameObject.transform.parent.GetComponentInChildren<Canvas>().gameObject.transform.GetChild(3).GetComponent<Text>();
        waterText = gameObject.transform.parent.GetComponentInChildren<Canvas>().gameObject.transform.GetChild(4).GetComponent<Text>();
        healthText = gameObject.transform.parent.GetComponentInChildren<Canvas>().gameObject.transform.GetChild(5).GetComponent<Text>();

        nutrientLevel = 0;
        waterLevel = 0;

        nutrientsText.text = "Nutrients: " + nutrientLevel;
        waterText.text = "Water: " + waterLevel;
        healthText.text = "Health: " + health;
        //magicOutputText.text = "Magic Output: " + magicLevel;
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
            if (currentStage >= 2)
            {
                playerScript.MagicCount += magicLevel;
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
            Vector3 canvasPosition = new Vector3();
			switch(currentStage) {
				case 1:
                    canvasPosition = canvas.gameObject.GetComponent<RectTransform>().position;
                    canvasPosition.y += 1f;
                    canvas.gameObject.GetComponent<RectTransform>().position = canvasPosition;
					break;
				case 2:
					magicLevel = 1;
					Instantiate(seed, new Vector3(transform.position.x + Random.Range(-3, 3), 0, transform.position.z + Random.Range(-3, 3)), Quaternion.identity);
					canvasPosition = canvas.gameObject.GetComponent<RectTransform>().position;
					canvasPosition.y += 2f;
                    canvasPosition.z -= .2f;
                    canvas.gameObject.GetComponent<RectTransform>().position = canvasPosition;
					break;
				case 3:
					magicLevel = 3;
					Instantiate(seed, new Vector3(transform.position.x + Random.Range(-3, 3), 0, transform.position.z + Random.Range(-3, 3)), Quaternion.identity);
					Instantiate(seed, new Vector3(transform.position.x + Random.Range(-3, 3), 0, transform.position.z + Random.Range(-3, 3)), Quaternion.identity);
					Instantiate(seed, new Vector3(transform.position.x + Random.Range(-3, 3), 0, transform.position.z + Random.Range(-3, 3)), Quaternion.identity);
					canvasPosition = canvas.gameObject.GetComponent<RectTransform>().position;
                    canvasPosition.x += 2f;
                    canvasPosition.y += 2f;
                    canvasPosition.z -= .2f;
					canvas.transform.position = canvasPosition;
                    canvas.gameObject.GetComponent<RectTransform>().position = canvasPosition;
					break;
				default:
					break;
			}
        }

        nutrientsText.text = "Nutrients: " + nutrientLevel;
        waterText.text = "Water: " + waterLevel;
        healthText.text = "Health: " + health;
        //magicOutputText.text = "Magic Output: " + magicLevel;
    }

    void UpdateSpriteRenderer()
    {
        waterLevel = 0;
        nutrientLevel = 0;
        this.GetComponent<SpriteRenderer>().sprite = treeStages[currentStage];

        Sprite sprite = this.GetComponent<SpriteRenderer>().sprite;
        this.GetComponent<CapsuleCollider>().radius = (float)sprite.bounds.size.x / 2;
        this.GetComponent<CapsuleCollider>().height = (float)sprite.bounds.size.y;

        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,
                                                         ((float)sprite.bounds.size.y / 2f),// - ((float)sprite.bounds.size.y / 16f),
                                                         this.gameObject.transform.position.z);
        if (currentStage >= 3 && ruins != null)
        {
            ruins.GetComponent<RuinsScript>().PlantedTree();
            gm.goalTreeCount++;
        }
        gm.StartDisasters(null, false);
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
