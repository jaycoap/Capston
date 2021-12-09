using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    private float movementSpeed;
    private float jumpSpeed;
    private bool isDead;
    private bool isGrounded;
    [SerializeField] private Transform pos;
    private Vector2 attackRange;
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
        attackRange = new Vector2(0.9166667f, 1.666667f);
    }

    void Update()
    {
        //Horizontal 버튼을 누를때 각 방향으로 캐릭터 스프라이트를 뒤집는 함수.
        if (Input.GetButton("Horizontal") && !animator.GetBool("isAttack"))
        {

            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
            
        }
        if(spriteRenderer.flipX == true)
        {
            pos.position = new Vector2(transform.position.x - 0.9166667f, pos.position.y);
        }else
        {
            pos.position = new Vector2(transform.position.x + 0.9166667f, pos.position.y);
        }
        //걷고 있는지, 공중이 아닌지, 공격중인지 체크해서 걷는 애니메이션을 관리하는 변수를 바꾸는 함수.
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1 && !animator.GetBool("isAttack") && isGrounded)
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

            
            //땅에 붙어있을 경우 체크
            if (isGrounded)
            {
                animator.SetBool("isGround", true);
                animator.SetBool("isJump", false); 
                //player_jump2 애니메이션이 끝난걸 체크해서 isFall을 참으로 만드는 함수
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump2") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool("isFall", true);
                }
                //player_jump3 애니메이션이 끝난걸 체크해서 isFall을 거짓으로 만드는 함수
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump3") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool("isFall", false);
                }
                //점프를 시켜주는 부분
                float J_input = Input.GetAxisRaw("Jump");

                if (J_input == 1 && !animator.GetBool("isAttack"))
                {
                    if (rigidBody.velocity.y < 9 )
                    {
                        rigidBody.AddForce(Vector2.up * J_input * jumpSpeed, ForceMode2D.Impulse);
                        animator.SetBool("isJump",true);
                    }
                }
                //attackCount가 0이면 player_attack1 애니메이션 실행
                if (animator.GetInteger("attackCount") == 0)
                {

                    if (Input.GetButton("Attack1"))
                    {
                        animator.SetBool("isAttack", true);
                        animator.SetInteger("attackCount", 1);
                        //hitEnemy = OverlapBoxAll(1,2,3,4) 1위치 기준 2범위에 있는 3회전의 4레이어 오브젝트를 전부 hitEnemy에 넣음
                        Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(pos.position, attackRange, 0, LayerMask.GetMask("Enemy"));
                        //hitEnemy에 있는 모든 오브젝트에 안에 함수 실행 <- 여기다가 데미지 넣는 함수 넣으면댐
                        foreach(Collider2D collider in hitEnemy)
                        {
                            collider.gameObject.GetComponent<enemyManager>().enemyDamaged(10);
                            
                        }
                    }
                }
                else if (animator.GetInteger("attackCount") == 1)
                {
                    //player_attack1 애니메이션이 50%~99% 완료중에 Attack1키입력을 하면 player_attack2 애니메이션 실행
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_attack1") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                    {
                        if (Input.GetButton("Attack1"))
                        {
                            animator.SetBool("isAttack", true);
                            animator.SetInteger("attackCount", 2);
                            //hitEnemy = OverlapBoxAll(1,2,3,4) 1위치 기준 2범위에 있는 3회전의 4레이어 오브젝트를 전부 hitEnemy에 넣음
                            Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(pos.position, attackRange, 0, LayerMask.GetMask("Enemy"));
                            //hitEnemy에 있는 모든 오브젝트에 안에 함수 실행 <- 여기다가 데미지 넣는 함수 넣으면댐
                            foreach (Collider2D collider in hitEnemy)
                            {
                                collider.gameObject.GetComponent<enemyManager>().enemyDamaged(10);
                            }
                        }
                    }
                    //player_attack1 애니메이션이 완료되면 애니메이션 종료후 초기화
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_attack1") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
                    {
                        animator.SetBool("isAttack", false);
                        animator.SetInteger("attackCount", 0);
                    }

                }
                else if (animator.GetInteger("attackCount") == 2)
                {
                    //player_attack2 애니메이션이 50%~99% 완료중에 Attack1키입력을 하면 player_attack3 애니메이션 실행
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_attack2") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                    {
                        if (Input.GetButton("Attack1"))
                        {
                            animator.SetBool("isAttack", true);
                            animator.SetInteger("attackCount", 3);
                            //hitEnemy = OverlapBoxAll(1,2,3,4) 1위치 기준 2범위에 있는 3회전의 4레이어 오브젝트를 전부 hitEnemy에 넣음
                            Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(pos.position, attackRange, 0, LayerMask.GetMask("Enemy"));
                            //hitEnemy에 있는 모든 오브젝트에 안에 함수 실행 <- 여기다가 데미지 넣는 함수 넣으면댐
                            foreach (Collider2D collider in hitEnemy)
                            {
                                collider.gameObject.GetComponent<enemyManager>().enemyDamaged(10);
                            }
                        }
                    }
                    //player_attack2 애니메이션이 완료되면 애니메이션 종료후 초기화
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_attack2") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
                    {
                        animator.SetBool("isAttack", false);
                        animator.SetInteger("attackCount", 0);
                    }
                }
                else if (animator.GetInteger("attackCount") == 3)
                {
                    //player_attack3 애니메이션이 50%~99% 완료중에 Attack1키입력을 하면 player_attack1 애니메이션 실행
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_attack3") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                    {
                        if (Input.GetButton("Attack1"))
                        {
                            animator.SetBool("isAttack", true);
                            animator.SetInteger("attackCount", 1);
                            //hitEnemy = OverlapBoxAll(1,2,3,4) 1위치 기준 2범위에 있는 3회전의 4레이어 오브젝트를 전부 hitEnemy에 넣음
                            Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(pos.position, attackRange, 0, LayerMask.GetMask("Enemy"));
                            //hitEnemy에 있는 모든 오브젝트에 안에 함수 실행 <- 여기다가 데미지 넣는 함수 넣으면댐
                            foreach (Collider2D collider in hitEnemy)
                            {
                                collider.gameObject.GetComponent<enemyManager>().enemyDamaged(10);
                               
                            }
                        }
                    }
                    //player_attack3 애니메이션이 완료되면 애니메이션 종료후 초기화
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_attack3") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
                    {
                        animator.SetBool("isAttack", false);
                        animator.SetInteger("attackCount", 0);
                    }
                }
            }
            //isGrounded
            else
            {
                //player_jump1 애니메이션이 종료되면 isJump를 거짓으로 만드는 함수
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump1") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
                {
                    animator.SetBool("isJump", false);
                }
                //player_jump3 애니메이션이 종료되면 isFall을 거짓으로 만드는 함수
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump3") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool("isFall", false);
                }
                animator.SetBool("isGround", false);
            }

            //Horizontal버튼을 누르면 이동시키는 함수
            if (Input.GetButton("Horizontal"))
            {
                if(H_input == 1 && !animator.GetBool("isAttack"))
                {
                    //캐릭터의 오른쪽,왼쪽에 벽이 있는지 체크하고 각각 벽이 있으면 그쪽으로 이동이 안되는 함수
                    if (Physics2D.OverlapBox(new Vector2(transform.position.x + 0.229166675f, transform.position.y), new Vector2(0.45833335f, 1.4f), 0, LayerMask.GetMask("Floor")))
                    {

                    }
                    else
                    {
                        transform.Translate(Vector2.right * H_input * movementSpeed);
                    }
                }
                if (H_input == -1 && !animator.GetBool("isAttack"))
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

    //플레이어가 Enemy태그를 가진 오브젝트와 충돌하면 onDaamged 실행
    void OnCollisionEnter2D(Collision2D other) {
            if(other.gameObject.tag == "Enemy")
            {
                onDamaged(other.transform.position.x);
            }
            
        }
    //플레이어의 레이어를 PlayerDamaged로 바꾸고 반투명 하게 만들며 피격 당한 반대방향으로 이동됨 Invoke로 3초후에 무적을 끄는 OffDamaged실행
    void onDamaged(float Enemy_X){
        int dirc = transform.position.x - Enemy_X > 0 ? 1 : -1;
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1,1,1,0.4f);
        rigidBody.AddForce(new Vector2(dirc*7,5),ForceMode2D.Impulse);

        Invoke("OffDamaged",3);

    }
    //플레이어의 레이어를 Player레이어로 변경, 반투명 해제 
    void OffDamaged()
    {
        gameObject.layer = 7;
        spriteRenderer.color = new Color(1,1,1,1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, attackRange);
    }


}
