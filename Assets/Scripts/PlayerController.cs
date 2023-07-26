using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Logic
    public bool isHiding = false;
    public bool isSearching = false;
    
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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHiding && !isSearching)
        {
            MoveCharacter();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            GameManager.Instance.SetPlayerRoom(collision.gameObject.name);
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
    }
    #endregion

    #region PLAYER_SEARCHING
    public void IsSearching(Vector3 position)
    {
        isSearching = true;
        // Transports the player to the searching spot position
        gameObject.transform.position = new Vector3(position.x, transform.position.y, transform.position.z);

        // Stops player movement
        rb.velocity = Vector3.zero;
    }

    public void DoneSearching()
    {
        isSearching = false;
    }
    #endregion
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
