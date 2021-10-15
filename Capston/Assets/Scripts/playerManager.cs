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
        movementSpeed = 0.1f;
        jumpSpeed = 9f;
    }

    void Update()
    {
        //Horizontal 버튼을 누를때 각 방향으로 캐릭터 스프라이트를 뒤집는 함수.
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
        //걷고 있는지 체크해서 애니메이션을 관리하는 변수를 바꾸는 함수.
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1)
        {
            animator.SetBool("isWalk", true);
        }
        else
        {
            animator.SetBool("isWalk", false);
        }
    }

    void FixedUpdate()
    {
        
        if (!isDead)
        {
            float H_input = Input.GetAxisRaw("Horizontal");
            //Horizontal버튼을 누르면 이동시키는 함수
            if(Input.GetButton("Horizontal"))
            {
                if(H_input == 1)
                {
                    if (Physics2D.OverlapBox(new Vector2(transform.position.x + 0.229166675f, transform.position.y), new Vector2(0.45833335f, 1.4f), 0, LayerMask.GetMask("Floor")))
                    {

                    }
                    else
                    {
                        transform.Translate(Vector2.right * H_input * movementSpeed);
                    }
                }
                if (H_input == -1)
                {
                    if (Physics2D.OverlapBox(new Vector2(transform.position.x - 0.229166675f, transform.position.y), new Vector2(0.45833335f, 1.4f), 0, LayerMask.GetMask("Floor")))
                    {

                    }
                    else
                    {
                        transform.Translate(Vector2.right * H_input * movementSpeed);
                    }
                }



            }
            
            //땅에 붙어있을 경우에만 점프를 시켜주는 함수
            if (isGrounded)
            {
                
                float J_input = Input.GetAxisRaw("Jump");

                if (J_input == 1)
                {
                    if(rigidBody.velocity.y < 9)
                    {
                        rigidBody.AddForce(Vector2.up * J_input * jumpSpeed, ForceMode2D.Impulse);
                    }  
                }


            }
        }
        //overlapbox(1,2,3,4) 1번 위치에서 2번 크기의 범위에 3번 회전의 오브젝트가 4번 레이어이면 true
        if (Physics2D.OverlapBox(new Vector2(transform.position.x,transform.position.y - 0.8333f), new Vector2(0.7f, 0.1f), 0, LayerMask.GetMask("Floor")))
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
