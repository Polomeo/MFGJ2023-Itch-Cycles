using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 movement;
    private float movementSpeed = 3f;
    private Rigidbody2D rb;
    
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
        MoveCharacter();
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

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    BaseInteractionItem col = collision.GetComponent<BaseInteractionItem>();
    //    if (col != null)
    //    {
    //        col.ShowInteractionText();

    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            col.Interact();
    //        }
    //    }
    //}
}
