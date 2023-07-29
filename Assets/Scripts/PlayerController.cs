using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Logic
    public bool isHiding = false;
    public bool isSearching = false;
    public bool isClimbing = false;
    public bool hasBeenFound = false;
    public string currentRoom;
    
    // Movement
    private Vector2 movement;
    private float movementSpeed = 3f;
    private Rigidbody2D rb;
    
    // Animation
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Set the initial state
        isHiding = false;
        isSearching = false;
        isClimbing = false;
        hasBeenFound = false;
}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHiding && !isSearching && !isClimbing && !hasBeenFound)
        {
            MoveCharacter();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            currentRoom = collision.gameObject.name;
            GameManager.Instance.SetPlayerRoom(currentRoom);
        }
    }

    #region PLAYER_HIDDING
    public void HidePlayer(Vector3 position)
    {
        isHiding = true;

        // Transports the player to the hidding spot position
        gameObject.transform.position = new Vector3(position.x, transform.position.y, transform.position.z);

        // Stops player movement
        rb.velocity = Vector3.zero;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void UnHidePlayer()
    {
        isHiding = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

        // As the player exits its cover, we compare if they are in the same room;
        GameManager.Instance.ComparePlayerAndEnemyRooms();
    }
    #endregion

    #region PLAYER_SEARCHING
    public void IsSearching(Vector3 position)
    {
        isSearching = true;
        animator.SetBool("b_isInteracting", isSearching);
        
        // Transports the player to the searching spot position
        gameObject.transform.position = new Vector3(position.x, transform.position.y, transform.position.z);

        // Stops player movement
        rb.velocity = Vector3.zero;

        // Searching
        Debug.Log("Is searching");
    }

    public void DoneSearching()
    {
        isSearching = false;
        animator.SetBool("b_isInteracting", isSearching);
    }
    #endregion

    #region PLAYER_LADDER
    public void IsClimbing(Vector3 position)
    {
        isClimbing = true;
        animator.SetBool("b_isInteracting", isClimbing);

        // Transports the player to the ladder spot position
        gameObject.transform.position = new Vector3(position.x, transform.position.y, transform.position.z);

        // Stops player movement
        rb.velocity = Vector3.zero;

        // Set the Rigidbody to Kinematic so can be moved from here
        rb.isKinematic = true;

    }

    public void DoneClimbing()
    {
        isClimbing = false;
        animator.SetBool("b_isInteracting", isClimbing);

        // Set the Rigidbody back to Dynamic
        rb.isKinematic = false;
    }
    #endregion

    public void PlayerHasBeenFound()
    {
        // Player has been found by Manny (the enemy)
        hasBeenFound = true;

        // Freeze
        rb.velocity = Vector3.zero;

        // Animation
        animator.SetBool("b_hasBeenFound", hasBeenFound);
    }
    void MoveCharacter()
    {
        // Movement

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.Normalize();
        rb.velocity = movement * movementSpeed;

        // Animate

        // flip the animation if is walking to the left
        if (movement.x < 0.0f) { transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f); }
        else if (movement.x > 0.0f) { transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); }

        animator.SetBool("b_isWalking", movement.x != 0f);
    }

}
