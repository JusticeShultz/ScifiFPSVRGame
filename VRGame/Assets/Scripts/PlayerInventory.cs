using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    public List<Loot> inventory;
    public float maxAmmo;

    // Use this for initialization
    void Start()
    {
        inventory = new List<Loot>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
