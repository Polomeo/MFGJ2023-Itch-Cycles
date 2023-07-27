using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public string playerCurrentRoom { get; private set; }
    public string enemyCurrentRoom { get; private set; }

    GameObject player;
    GameObject enemy;
    
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
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        // Exit Game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
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
    private void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void ComparePlayerAndEnemyRooms()
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
}
