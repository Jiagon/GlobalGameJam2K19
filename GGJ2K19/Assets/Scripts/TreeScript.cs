using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeScript : MonoBehaviour {

    public List<Sprite> treeStages = new List<Sprite>();    // Prefab objects the trees will display as
    public List<int> waterStages = new List<int>();         // How much water is necessary for a stage to display
    public List<int> nutrientStages = new List<int>();      // How much nutriets are necessary for a stage to display
    int currentStage;                                       // Which stages of growth a tree is currently on


    public int surroundingNutrients;                        // How many nutrients per second a tree gets based on surrounding soil

    int waterLevel;                                         // How much water the tree currently has
    int nutrientLevel;                                      // How many nutrients the tree currently has
    float timer;                                            // Keeps track of nutrient gain time
    int health;                                             // How much health a tree has - deteroiates due to environment disasters



	// Use this for initialization
	void Start () {
        currentStage = 0;
        health = 10;
        UpdateSpriteRenderer();
	}
	
	// Update is called once per frame
	void Update () {
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }

        timer += Time.deltaTime;
        if (timer >= 3.0f && surroundingNutrients > 0)
        {
            nutrientLevel += surroundingNutrients;
            timer = 0;
            Debug.Log("Got nutrients");
        }
        
        if(waterLevel >= waterStages[currentStage] && nutrientLevel >= nutrientStages[currentStage] && currentStage < 3)
        {
            ++currentStage;
            UpdateSpriteRenderer();
        }
	}

    void UpdateSpriteRenderer()
    {
        waterLevel = 0;
        nutrientLevel = 0;
        this.GetComponent<SpriteRenderer>().sprite = treeStages[currentStage];
        this.GetComponent<CapsuleCollider2D>().size = new Vector2((float)this.GetComponent<SpriteRenderer>().sprite.bounds.size.x,
                                                                  (float)this.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
    }

    private void OnMouseDown()
    {
        // Quick hit detection courtesy of Kyle W Banks:
        // https://kylewbanks.com/blog/unity-2d-detecting-gameobject-clicks-using-raycasts
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.z);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider == this.GetComponent<CapsuleCollider>())
        {
            /*++waterLevel;
            //++nutrientLevel;
            Debug.Log("Current water level: " + waterLevel);
            Debug.Log("Current nutrient level: " + nutrientLevel);
            Debug.Log("Water level needed: " + waterStages[currentStage]);
            Debug.Log("Nutrient level needed: " + nutrientStages[currentStage]);*/
        }
    }

}
