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
    [Header("Game Loop Conditions")]
    public bool isGameActive = true;
    public bool playerLoseScenario = false;
    public bool playerWinScenario = false;
    public bool playerIsChasingEnemyPhase = false;
    public int dollPlaced = 0;

    // GameObjects
    [Header("Objects in scene")]
    public GameObject player;
    public GameObject enemy;
    public List<GameObject> searchSpots;
    public List<GameObject> hideSpots;
    public List<GameObject> burningSpots;

    // Audio
    [Header("Audio")]
    [SerializeField] private AudioClip burnDollSFX;
    [SerializeField] private AudioClip backgroundSong;
    [SerializeField] private AudioClip urgentSong;

    private AudioSource audioSource;
    private AudioSource mainCamSound;

    
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

        // Components
        audioSource = GetComponent<AudioSource>();
        mainCamSound = Camera.main.GetComponent<AudioSource>();

        // Store references
        searchSpots = new List<GameObject>(GameObject.FindGameObjectsWithTag("SearchSpot"));
        burningSpots = new List<GameObject>(GameObject.FindGameObjectsWithTag("BurningSpot"));
        hideSpots = new List<GameObject>(GameObject.FindGameObjectsWithTag("HideSpot"));

        // Starting setup
        RandomPlaceDolls();

    }

    // Update is called once per frame
    void Update()
    {
        // Exit Game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }

        // Game Over
        if(!isGameActive && playerLoseScenario)
        {
            // Restart Game
            if(Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }

        if (!isGameActive && playerWinScenario)
        {
            // Restart Game
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }


    }

    private void RandomPlaceDolls()
    {
        // Remove all dolls
        foreach(GameObject spot in searchSpots)
        {
            spot.GetComponent<SearchingSpot>().thisSpotHasADoll = false;
        }

        // There are 3 dolls in the game, to be put in random searching spots
        for (int i = 0; i < 3; i++) 
        { 
            // Select a Random Search Spot index
            int randomSearchSpot = Random.Range(0, searchSpots.Count);

            // Put a doll in it
            searchSpots[randomSearchSpot].GetComponent<SearchingSpot>().PutDollIn();

            // Remove from list (so it does not repeat)
            searchSpots.RemoveAt(randomSearchSpot);
        }

        // Clear the list
        searchSpots.Clear();

        // Repopulate the list with all spots
        searchSpots = new List<GameObject>(GameObject.FindGameObjectsWithTag("SearchSpot"));
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

        // If they are in the same room, and the player is not hidding, and the enemy is not escaping
        if (playerCurrentRoom == enemyCurrentRoom && !playerController.isHiding && !enemyController.isEscapingFromPlayer)
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

        // Check for each burning spot for a doll
        for(int i = 0; i < burningSpots.Count; i++)
        {
            if (burningSpots[i].GetComponent<BurningSpot>().dollPlaced)
            {
                totalDollsPlaced++;
            }
        }

        // Upgrade the total dolls placed (relevant for enemy dificulty)
        dollPlaced = totalDollsPlaced;

        // Update enemy difficulty
        enemy.GetComponent<PatrolAI>().SetTotalSpeed(dollPlaced);

        // If all dolls are placed
        if(totalDollsPlaced == burningSpots.Count)
        {
            Debug.Log("All dolls placed in ritual room.");

            StartRitual();
            
            return true;
        }
        else
        {
            Debug.Log((burningSpots.Count - totalDollsPlaced).ToString() + " dolls left." );
            return false;
        }
    }

    public void StartRitual()
    {
        // AUDIO - Stop the song
        mainCamSound.Stop();

        // Burn dolls
        foreach (GameObject go in  burningSpots)
        {
            go.GetComponent<BurningSpot>().BurnDoll();
        }

        // Give the knife to the player
        player.GetComponent<PlayerController>().GetKnife();

        // Scare the clown
        enemy.GetComponent<PatrolAI>().EscapeFromPlayer();

        // Play ritual sound
        audioSource.PlayOneShot(burnDollSFX);

        // AUDIO - Play the Urgent Song
        mainCamSound.clip = urgentSong;
        mainCamSound.Play();

        // Remove interaction for hidding, searching and burning
        RemoveInteraction();
   
    }

    private void RemoveInteraction()
    {
        // Removes the interaction with objects (other than ladders)

        // Search spots
        foreach(GameObject spot in searchSpots)
        {
            spot.GetComponent<BoxCollider2D>().enabled = false;
        }

        // Hide spots
        foreach (GameObject spot in hideSpots)
        {
            spot.GetComponent<BoxCollider2D>().enabled = false;
        }

        // Burning spots
        foreach (GameObject spot in burningSpots)
        {
            spot.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        playerLoseScenario = true;
        UIManager.Instance.ShowGameOverCanvas();
    }

    public void GameWin()
    {
        isGameActive = false;
        playerWinScenario = true;
        UIManager.Instance.ShowGameWinCanvas();

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
