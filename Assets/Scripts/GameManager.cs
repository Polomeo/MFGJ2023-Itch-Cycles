using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    // Player and Enemy current room
    public string playerCurrentRoom { get; private set; }
    public string enemyCurrentRoom { get; private set; }

    // Gameplay loop conditions
    public bool isGameActive = true;
    public bool playerIsChasingEnemyPhase = false;

    // GameObjects
    public GameObject player;
    public GameObject enemy;
    public List<GameObject> searchSpots;
    public List<GameObject> burningSpots;

    
    // SINGLETON
    void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Store player and enemy reference
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");

        // Store references
        searchSpots = new List<GameObject>(GameObject.FindGameObjectsWithTag("SearchSpot"));
        burningSpots = new List<GameObject>(GameObject.FindGameObjectsWithTag("BurningSpot"));

    }

    // Update is called once per frame
    void Update()
    {
        // Exit Game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }

        // Restart Game
        if(!isGameActive)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }
    }

    public void SetPlayerRoom(string room)
    {
        playerCurrentRoom = room;
        Debug.Log("Player current room: " + room);
        
        ComparePlayerAndEnemyRooms();
    }
    public void SetEnemyRoom(string room)
    {
        enemyCurrentRoom = room;
        Debug.Log("Enemy current room: " +  room);

        ComparePlayerAndEnemyRooms();
    }
    public void ComparePlayerAndEnemyRooms()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        PatrolAI enemyController = enemy.GetComponent<PatrolAI>();

        // If they are in the same room, and the player is not hidding
        if (playerCurrentRoom == enemyCurrentRoom && !playerController.isHiding)
        { 
            // Freeze player position
            playerController.PlayerHasBeenFound();

            // Enemy charges towards player
            enemyController.PlayerIsSpotted();

            // On contact --> Game Over
        }
    }
    public bool CheckIfAllDollsArePlaced()
    {
        int totalDollsPlaced = 0;

        for(int i = 0; i < burningSpots.Count; i++)
        {
            if (burningSpots[i].GetComponent<BurningSpot>().dollPlaced)
            {
                totalDollsPlaced++;
            }
        }

        if(totalDollsPlaced == burningSpots.Count)
        {
            Debug.Log("All dolls placed in ritual room.");
            return true;
        }
        else
        {
            Debug.Log((burningSpots.Count - totalDollsPlaced).ToString() + " dolls left." );
            return false;
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        UIManager.Instance.ShowGameOverCanvas();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
