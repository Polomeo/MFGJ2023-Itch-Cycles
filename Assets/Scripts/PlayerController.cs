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

    public void HidePlayer(Vector3 position)
    {
        isHiding = true;

        // Moves the player to the hidding spot position
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
