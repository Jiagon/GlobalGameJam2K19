using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameManager gm;

    public Rigidbody rb;
    private Vector3 position;
    public Vector3 prevPosition;
    public bool isWithinRadius;
    public List<GameObject> radiusObjects;
    public bool hasResource;
    public Vector3 direction;

    public Sprite water;
    public Sprite pebble;
    public Sprite spriteToPass;
    public GameObject followingSphere;
    public bool tooCloseToWater;

    int magicCount;
    public int MagicCount {
        get {
            return magicCount;
        }
        set {
            magicCount = value;
            magicText.text = "Magic: " + magicCount;
        }
    }
    public GameObject parentPrefab;
    public GameObject treePrefab;
	
    public int seedCount;

    public Text magicText;
    public Text seedText;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        position = rb.transform.position;
        direction = rb.transform.forward;
        prevPosition = position;
        isWithinRadius = true;
        radiusObjects = new List<GameObject>();
        hasResource = false;
        magicText.text = "Magic: " + magicCount;
        seedText.text = "Seeds: " + seedCount;
	}

    // Update is called once per frame
    void Update()
    {
        /*if(radiusObjects.Count == 0)
        {
            isWithinRadius = false;
        }

        else
        {
            isWithinRadius = true;
        }*/

        /*int direction = 1;

        if (radiusObjects.Count <= 0/*isWithinRadius)
        {
            direction *= -1;
        }*/

        position = rb.transform.position;
        prevPosition = position;
        position.x += (Input.GetAxis("Horizontal")/10)/* * direction*/;
        position.y = 1.3f;
        position.z += (Input.GetAxis("Vertical")/10)/* * direction*/;
        direction = prevPosition - position;

        if (direction.x > 0)
            GetComponent<SpriteRenderer>().flipX = false;
        else if(direction.x < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        rb.transform.position = position;

        PlantSeed();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Seed")
        {
            seedCount++;
            seedText.text = "Seeds: " + seedCount;
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Water")
        {
            if (other.GetType() == typeof(SphereCollider))
                tooCloseToWater = true;
            else
            {
                if (Input.GetKeyDown("space"))
                {
                    hasResource = true;
                    spriteToPass = water;
                    followingSphere.SetActive(true);
                    followingSphere.GetComponent<PlayerItemFollow>().ResetSphere();
                }
            }
        }
        if(other.tag == "Pebble")
        {
            if (Input.GetKeyDown("space"))
            {
                hasResource = true;
                spriteToPass = pebble;
                followingSphere.SetActive(true);
                followingSphere.GetComponent<PlayerItemFollow>().ResetSphere();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water" && other.GetType() == typeof(SphereCollider))
        {
            tooCloseToWater = false;
        }
    }

    public void DisableItem()
    {
        followingSphere.SetActive(false);
    }

    public void PlantSeed()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(seedCount > 0 && !tooCloseToWater)
            {
                GameObject treeParent = GameObject.Instantiate(parentPrefab, transform.position, Quaternion.identity);
                treeParent.GetComponent<TreePlayerDetection>().player = gameObject;

                gm.StartDisasters(treeParent.GetComponentInChildren<TreeScript>());
                //GameObject tree = GameObject.Instantiate(treePrefab, transform.position, Quaternion.identity);
                //tree.GetComponent<TreeScript>().player = gameObject;

                seedCount--;
                seedText.text = "Seeds: " + seedCount;
            }
        }
    }
}
