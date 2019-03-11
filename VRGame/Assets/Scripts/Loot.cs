using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {


    public int points; // how much its worth
    public LootType type;
    public float flySpeed; // how fast it comes towards the player
    public bool beingCollected; // by player

    float t;
    Vector3 origPos;
    Transform player;
    float hoverTimer;
    float rotationSpeed;
    PlayerInventory inventory;

    public bool spawned;

    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(false); // inactive until needed
        beingCollected = false;
        t = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inventory = GameObject.FindObjectOfType<PlayerInventory>();
        hoverTimer = 0;
        rotationSpeed = 0.1f;

        spawned = false;
    }

    // Start collection
    public void Spawn()
    {
        gameObject.SetActive(true);
        transform.localPosition = Vector3.zero;
        origPos = transform.position;
        beingCollected = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawned) { spawned = false; gameObject.SetActive(true); Spawn(); }
        if (!beingCollected) { return; }

        if (hoverTimer < 2) // wait to fly to player
        {
            hoverTimer += Time.deltaTime;
            transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0), Space.Self);
            return;
        }

        // fly to player
        t += flySpeed * Time.deltaTime;
        transform.LookAt(player.position);
        transform.position = Vector3.Lerp(origPos, player.position, t);

        if (t >= 1) // delete
        {
            // player.Add(loot);
            Destroy(gameObject);
        }
    }
}
