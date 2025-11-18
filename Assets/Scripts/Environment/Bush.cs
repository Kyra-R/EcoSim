using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{

    public GameObject fruitPrefab;

    public float fruitLifetime = 5.0f;

    public int fruitAmount = 3;

    public List<GameObject> fruits;
    // Start is called before the first frame update
    void Start()
    {
        InitialiseFruits();
        
        InvokeRepeating("GrowFruits", fruitLifetime, fruitLifetime);
 
        // Set the value so we can show a countdown for each population
    }


     private void InitialiseFruits()
    {
        for (int i = 0; i < fruitAmount; i++)
        {
            // Choose a random position for the creature to appear
            Vector2 pos = new Vector2(transform.position.x + Random.Range(-1.0f, 1.0f),transform.position.y + Random.Range(-1.0f, 1.0f));
 
            // Instantiate a new creature
            GameObject fruit = Instantiate(fruitPrefab, pos, Quaternion.identity);
 
            // Add the creature to the population
            fruits.Add(fruit);
        }
    }

    private void GrowFruits(){
        for (int i = 0; i < fruitAmount; i++)
        {
            Destroy(fruits[i]);
        }
        fruits.Clear();
        InitialiseFruits();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
