using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabFish;

    [SerializeField]
    private GameObject prefabBomb;

    [SerializeField]
    private Sprite[] sprites;

    // Spawn speed (how many seconds between spawning)
    [SerializeField]
    private float spawnSpeed;

    // Countdown since last time a fish was spawned
    private float lastSpawn;

    // Tracked fish objects
    private List<GameObject> gameObjects;

    // Start is called before the first frame update
    void Start()
    {
        // Start a timer
        lastSpawn = spawnSpeed;
        gameObjects = new List<GameObject>();
    }

    public void Clear()
    {
        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            Destroy(gameObjects[i]);
        }
        gameObjects.Clear();
    }

    // Spawn a new fish
    void SpawnFish()
    {
        // Decide whether to spawn a fish or a bomb
        int random = Random.Range(0, 5);
        if(random == 1)
        {
            GameObject bomb = Instantiate(prefabBomb, new Vector3(Random.Range(-10.0f, 10.0f), 6, 0), Quaternion.identity);
            bomb.GetComponent<Rigidbody2D>().gravityScale = Random.Range(0.2f, 0.4f);
            gameObjects.Add(bomb);
            return;
        }

        GameObject fish = Instantiate(prefabFish, new Vector3(Random.Range(-10.0f, 10.0f), 6, 0), Quaternion.identity);
        SpriteRenderer renderer = fish.GetComponent<SpriteRenderer>();
        renderer.sprite = sprites[Random.Range(0, 3)];

        // Adjust the speed of gravity to add some randomness
        fish.GetComponent<Rigidbody2D>().gravityScale = Random.Range(0.2f, 0.4f);

        gameObjects.Add(fish);
    }

    // Remove a fish from the list once they have a collision
    public void DespawnFish(GameObject gameObject)
    {
        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            if (gameObjects[i] == gameObject)
            {
                Destroy(gameObjects[i]);
                gameObjects.RemoveAt(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // On timer, spawn a new fish
        lastSpawn -= Time.deltaTime;
        if(lastSpawn < 0.0f)
        {
            SpawnFish();
            lastSpawn = Random.Range(spawnSpeed - 1, spawnSpeed + 1);
        }

        // Adjust spawn speed down slightly when player catches a fish
        spawnSpeed = Mathf.Max(spawnSpeed - 0.1f, 1.0f);

        // Clean up game objects that fall below the line and don't get caught
        for(int i = gameObjects.Count - 1; i >= 0; i--)
        {
            GameObject fish = gameObjects[i];
            if(fish.transform.position.y < -6.0f)
            {
                Destroy(gameObjects[i]);
                gameObjects.RemoveAt(i);
            }
        }
    }
}