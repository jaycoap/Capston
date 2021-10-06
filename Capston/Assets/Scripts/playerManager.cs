using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    private float movementSpeed;
    private float jumpSpeed;
    private bool isDead;
    private bool isGrounded;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator animator;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isDead = false;
        isGrounded = false;
        movementSpeed = 0.5f;
        jumpSpeed = 9f;
    }

    void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        if (Mathf.Abs( rigidBody.velocity.x ) < 0.9)
        {
            animator.SetBool("isWalk", false);
        }
        else
        {
            animator.SetBool("isWalk", true);
        }
    }

    void FixedUpdate()
    {

        if (!isDead)
        {
            float H_input = Input.GetAxisRaw("Horizontal");
            
            if(Input.GetButton("Horizontal"))
            {
                //transform.Translate(Vector3.right * H_input * movementSpeed);
                rigidBody.AddForce(Vector2.right * H_input * movementSpeed, ForceMode2D.Impulse);

            }
            
            
            if (isGrounded)
            {
                
                float V_input = Input.GetAxisRaw("Vertical");

                if(V_input == 1)
                {
                    rigidBody.AddForce(Vector2.up * V_input * jumpSpeed, ForceMode2D.Impulse);
                    
                }

                
            }
        }

        if (Physics2D.OverlapBox(transform.position, new Vector2(0.8f, 1.666667f), 0, LayerMask.GetMask("Floor")))
        {
            isGrounded = true;

            rigidBody.drag = 3;
            rigidBody.gravityScale = 3;
        }
        else
        {
            isGrounded = false;

        }
    }         
}
