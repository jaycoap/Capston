using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    private float movementSpeed;
    private bool isDead;
    private bool isGrounded;
    Rigidbody2D rigidBody;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        isDead = false;
        isGrounded = false;
        movementSpeed = 0.3f;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {

        if (!isDead)
        {
            float H_input = Input.GetAxisRaw("Horizontal");
            transform.Translate(Vector3.right * H_input * movementSpeed);
            if (isGrounded)
            {
                
                float V_input = Input.GetAxisRaw("Vertical");

                if(V_input == 1)
                {
                    rigidBody.AddForce(Vector2.up * V_input * 5, ForceMode2D.Impulse);
                }

                
            }
        }

        if (Physics2D.OverlapBox(transform.position, new Vector2(1.9f,2), 0, LayerMask.GetMask("Floor")))
        {
            isGrounded = true;

            rigidBody.drag = 3;
            rigidBody.gravityScale = 2;
        }
        else
        {
            isGrounded = false;

        }
    }         
}
