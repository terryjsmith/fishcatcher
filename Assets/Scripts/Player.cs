using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed;

    // Stored physics collider component
    BoxCollider2D m_collider;

    // Current player score
    private int currentScore;

    // Score display
    GameObject scoreDisplay;

    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<BoxCollider2D>();
        currentScore = 0;
        scoreDisplay = GameObject.Find("highScore");
        Time.timeScale = 0;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            if(Time.timeScale == 0)
            {
                currentScore = 0;
                scoreDisplay.GetComponent<Text>().text = "Score: " + currentScore.ToString();
                Time.timeScale = 1;
                GameObject text = GameObject.Find("instructionText");
                text.GetComponent<Text>().enabled = false;

                // Clear the deck
                GameObject gameObject = GameObject.Find("Spawner");
                FishSpawner spawner = gameObject.GetComponent<FishSpawner>();
                spawner.Clear();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(0, -horizontal * playerSpeed * Time.deltaTime, 0));
    }

    // Physics check
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("A collision!!");

        // Check for a collision with a bomb
        if(other.gameObject.tag == "Bomb")
        {
            // Oh no! Game over man!
            Time.timeScale = 0;
            GameObject text = GameObject.Find("instructionText");
            text.GetComponent<Text>().enabled = true;
            text.GetComponent<Text>().text = "Game over! Press space to restart.";
            return;
        }

        currentScore++;
        scoreDisplay.GetComponent<Text>().text = "Score: " + currentScore.ToString();

        // TODO: destroy the other game object once it's in the net
        GameObject gameObject = GameObject.Find("Spawner");
        FishSpawner spawner = gameObject.GetComponent<FishSpawner>();
        spawner.DespawnFish(other.gameObject);
    }
}
