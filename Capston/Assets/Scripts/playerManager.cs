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
        Physics.IgnoreLayerCollision(8,8,true);
    }

    void Update()
    {
        //Horizontal ��ư�� ������ �� �������� ĳ���� ��������Ʈ�� ������ �Լ�.
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
        //�Ȱ� �ִ���, ������ �ƴ���, ���������� üũ�ؼ� �ȴ� �ִϸ��̼��� �����ϴ� ������ �ٲٴ� �Լ�.
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

            
            //���� �پ����� ��� üũ
            if (isGrounded)
            {
                animator.SetBool("isGround", true);
                animator.SetBool("isJump", false); 
                //player_jump2 �ִϸ��̼��� ������ üũ�ؼ� isFall�� ������ ����� �Լ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump2") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool("isFall", true);
                }
                //player_jump3 �ִϸ��̼��� ������ üũ�ؼ� isFall�� �������� ����� �Լ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump3") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool("isFall", false);
                }
                //������ �����ִ� �κ�
                float J_input = Input.GetAxisRaw("Jump");

                if (J_input == 1 && !animator.GetBool("isAttack"))
                {
                    if (rigidBody.velocity.y < 9 )
                    {
                        rigidBody.AddForce(Vector2.up * J_input * jumpSpeed, ForceMode2D.Impulse);
                        animator.SetBool("isJump",true);
                    }
                }
                //attackCount�� 0�̸� player_attack1 �ִϸ��̼� ����
                if (animator.GetInteger("attackCount") == 0)
                {

                    if (Input.GetButton("Attack1"))
                    {
                        animator.SetBool("isAttack", true);
                        animator.SetInteger("attackCount", 1);
                        //hitEnemy = OverlapBoxAll(1,2,3,4) 1��ġ ���� 2������ �ִ� 3ȸ���� 4���̾� ������Ʈ�� ���� hitEnemy�� ����
                        Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(pos.position, attackRange, 0, LayerMask.GetMask("Enemy"));
                        //hitEnemy�� �ִ� ��� ������Ʈ�� �ȿ� �Լ� ���� <- ����ٰ� ������ �ִ� �Լ� �������
                        foreach(Collider2D collider in hitEnemy)
                        {
                            collider.gameObject.GetComponent<enemyManager>().enemyDamaged(10);
                            
                        }
                    }
                }
                else if (animator.GetInteger("attackCount") == 1)
                {
                    //player_attack1 �ִϸ��̼��� 50%~99% �Ϸ��߿� Attack1Ű�Է��� �ϸ� player_attack2 �ִϸ��̼� ����
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_attack1") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                    {
                        if (Input.GetButton("Attack1"))
                        {
                            animator.SetBool("isAttack", true);
                            animator.SetInteger("attackCount", 2);
                            //hitEnemy = OverlapBoxAll(1,2,3,4) 1��ġ ���� 2������ �ִ� 3ȸ���� 4���̾� ������Ʈ�� ���� hitEnemy�� ����
                            Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(pos.position, attackRange, 0, LayerMask.GetMask("Enemy"));
                            //hitEnemy�� �ִ� ��� ������Ʈ�� �ȿ� �Լ� ���� <- ����ٰ� ������ �ִ� �Լ� �������
                            foreach (Collider2D collider in hitEnemy)
                            {
                                collider.gameObject.GetComponent<enemyManager>().enemyDamaged(10);
                            }
                        }
                    }
                    //player_attack1 �ִϸ��̼��� �Ϸ�Ǹ� �ִϸ��̼� ������ �ʱ�ȭ
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_attack1") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
                    {
                        animator.SetBool("isAttack", false);
                        animator.SetInteger("attackCount", 0);
                    }

                }
                else if (animator.GetInteger("attackCount") == 2)
                {
                    //player_attack2 �ִϸ��̼��� 50%~99% �Ϸ��߿� Attack1Ű�Է��� �ϸ� player_attack3 �ִϸ��̼� ����
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_attack2") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                    {
                        if (Input.GetButton("Attack1"))
                        {
                            animator.SetBool("isAttack", true);
                            animator.SetInteger("attackCount", 3);
                            //hitEnemy = OverlapBoxAll(1,2,3,4) 1��ġ ���� 2������ �ִ� 3ȸ���� 4���̾� ������Ʈ�� ���� hitEnemy�� ����
                            Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(pos.position, attackRange, 0, LayerMask.GetMask("Enemy"));
                            //hitEnemy�� �ִ� ��� ������Ʈ�� �ȿ� �Լ� ���� <- ����ٰ� ������ �ִ� �Լ� �������
                            foreach (Collider2D collider in hitEnemy)
                            {
                                collider.gameObject.GetComponent<enemyManager>().enemyDamaged(10);
                            }
                        }
                    }
                    //player_attack2 �ִϸ��̼��� �Ϸ�Ǹ� �ִϸ��̼� ������ �ʱ�ȭ
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_attack2") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
                    {
                        animator.SetBool("isAttack", false);
                        animator.SetInteger("attackCount", 0);
                    }
                }
                else if (animator.GetInteger("attackCount") == 3)
                {
                    //player_attack3 �ִϸ��̼��� 50%~99% �Ϸ��߿� Attack1Ű�Է��� �ϸ� player_attack1 �ִϸ��̼� ����
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_attack3") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                    {
                        if (Input.GetButton("Attack1"))
                        {
                            animator.SetBool("isAttack", true);
                            animator.SetInteger("attackCount", 1);
                            //hitEnemy = OverlapBoxAll(1,2,3,4) 1��ġ ���� 2������ �ִ� 3ȸ���� 4���̾� ������Ʈ�� ���� hitEnemy�� ����
                            Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(pos.position, attackRange, 0, LayerMask.GetMask("Enemy"));
                            //hitEnemy�� �ִ� ��� ������Ʈ�� �ȿ� �Լ� ���� <- ����ٰ� ������ �ִ� �Լ� �������
                            foreach (Collider2D collider in hitEnemy)
                            {
                                collider.gameObject.GetComponent<enemyManager>().enemyDamaged(10);
                               
                            }
                        }
                    }
                    //player_attack3 �ִϸ��̼��� �Ϸ�Ǹ� �ִϸ��̼� ������ �ʱ�ȭ
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
                //player_jump1 �ִϸ��̼��� ����Ǹ� isJump�� �������� ����� �Լ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump1") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
                {
                    animator.SetBool("isJump", false);
                }
                //player_jump3 �ִϸ��̼��� ����Ǹ� isFall�� �������� ����� �Լ�
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump3") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    animator.SetBool("isFall", false);
                }
                animator.SetBool("isGround", false);
            }

            //Horizontal��ư�� ������ �̵���Ű�� �Լ�
            if (Input.GetButton("Horizontal"))
            {
                if(H_input == 1 && !animator.GetBool("isAttack"))
                {
                    //ĳ������ ������,���ʿ� ���� �ִ��� üũ�ϰ� ���� ���� ������ �������� �̵��� �ȵǴ� �Լ�
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
        //overlapbox(1,2,3,4) 1�� ��ġ���� 2�� ũ���� ������ 3�� ȸ���� ������Ʈ�� 4�� ���̾��̸� true
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

    //�÷��̾ Enemy�±׸� ���� ������Ʈ�� �浹�ϸ� onDaamged ����
    void OnCollisionEnter2D(Collision2D other) {
            if(other.gameObject.tag == "Enemy")
            {
                onDamaged(other.transform.position.x);
            }
            
        }
    //�÷��̾��� ���̾ PlayerDamaged�� �ٲٰ� ������ �ϰ� ����� �ǰ� ���� �ݴ�������� �̵��� Invoke�� 3���Ŀ� ������ ���� OffDamaged����
    void onDamaged(float Enemy_X){
        int dirc = transform.position.x - Enemy_X > 0 ? 1 : -1;
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1,1,1,0.4f);
        rigidBody.AddForce(new Vector2(dirc*7,5),ForceMode2D.Impulse);

        Invoke("OffDamaged",1);

    }
    //�÷��̾��� ���̾ Player���̾�� ����, ������ ���� 
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
