using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField]private float maxJumpSpeed;
    private float skill2_movement = 15;
    //쿨타임 변수
    private float coolTime1_start = 0;
    private float coolTime1_current;
    private float coolTime1_skill = 4;
    private bool isCoolTime1;

    private float coolTime2_start = 0;
    private float coolTime2_current;
    private float coolTime2_skill = 1;
    private bool isCoolTime2;

    private float coolTime3_start = 0;
    private float coolTime3_current;
    private float coolTime3_skill = 10;
    private bool isCoolTime3;

    private bool isDead;
    private bool isGrounded;
    [SerializeField] private Transform pos;
    private Vector2 attackRange;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator animator;
    [SerializeField] private GameObject Bullet1;
    [SerializeField] private GameObject Bullet2;
    [SerializeField] private GameObject Bullet3;
    [SerializeField] private GameObject AttackBox;
    [SerializeField] private GameObject Qbox;
    [SerializeField] private SkillCoolTime Skill_1slot;
    [SerializeField] private SkillCoolTime Skill_2slot;
    [SerializeField] private SkillCoolTime Skill_3slot;
    public static bool isStart = false;
    public static bool flipx;
    GameManager GM;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        GM = GetComponent<GameManager>();

        isDead = false;
        isGrounded = false;
        attackRange = new Vector2(0.9166667f, 1.666667f);
        Physics.IgnoreLayerCollision(8, 8, true);
    }

    void Update()
    {
        if (isStart)
        {
            //방향전환시 스프라이트 뒤집기
            if (Input.GetButton("Horizontal") && !animator.GetBool("isAttack")
            && !(animator.GetBool("isSkill1")) && !(animator.GetBool("isSkill2")) && !(animator.GetBool("isSkill3On")))
            {

                spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
                flipx = spriteRenderer.flipX;
            }
            //방향전환시 공격범위 변경
            if (spriteRenderer.flipX == true)
            {
                AttackBox.transform.position = new Vector2(transform.position.x - 1.3f, pos.position.y);
                Qbox.transform.position = new Vector2(transform.position.x - 1.55f, pos.position.y);
            }
            else
            {
                AttackBox.transform.position = new Vector2(transform.position.x + 1.3f, pos.position.y);
                Qbox.transform.position = new Vector2(transform.position.x + 1.55f, pos.position.y);
            }
            //이동시 걷는 애니메이션 출력
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1 && !animator.GetBool("isAttack") && isGrounded
                && !(animator.GetBool("isSkill1")) && !(animator.GetBool("isSkill2")) && !(animator.GetBool("isSkill3On")))
            {
                animator.SetBool("isWalk", true);
            }
            else
            {
                animator.SetBool("isWalk", false);
            }

            checkCool1();
            checkCool2();
            checkCool3();
        }
        
    }

    void FixedUpdate()
    {
        
        if (!isDead)
        {
            if (isStart)
            {
                float H_input = Input.GetAxisRaw("Horizontal");

                //player_jump2 애니메이션이 실행중일때 isFall을 true로
                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump2") || animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump2_skill"))
                    && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.0f)
                {
                    animator.SetBool("isFall", true);
                }

                //땅에 닿아있을때
                if (isGrounded)
                {
                    animator.SetBool("isGround", true);
                    animator.SetBool("isJump", false);

                    //player_jump3 애니메이션 출력 완료시 isFall false
                    if ((animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump3") || animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump3_skill"))
                        && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        animator.SetBool("isFall", false);
                    }
                    //점프
                    float J_input = Input.GetAxisRaw("Jump");

                    if (J_input == 1 && !animator.GetBool("isAttack") && !animator.GetBool("isSkill3On") && !animator.GetBool("isJump"))
                    {
                        if (rigidBody.velocity.y < maxJumpSpeed)
                        {
                            rigidBody.AddForce(Vector2.up * J_input * jumpSpeed, ForceMode2D.Impulse);
                            animator.SetBool("isJump", true);
                        }
                    }
                    if (Input.GetButton("Attack1"))
                    {
                        if ((animator.GetCurrentAnimatorStateInfo(0).IsName("player_idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("player_idle_skill")
                            || animator.GetCurrentAnimatorStateInfo(0).IsName("player_run") || animator.GetCurrentAnimatorStateInfo(0).IsName("player_run_skill"))
                            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.0f)
                        {
                            animator.SetBool("isAttack", true);
                        }
                        else
                        {
                            switch (animator.GetInteger("attackCount"))
                            {
                                case 1:
                                    if (animator.GetBool("isSkill3") == true)
                                    {
                                        attacker("player_attack1_skill", 2);
                                    }
                                    else
                                    {
                                        attacker("player_attack1", 2);
                                    }
                                    break;
                                case 2:
                                    if (animator.GetBool("isSkill3") == true)
                                    {
                                        attacker("player_attack2_skill", 3);
                                    }
                                    else
                                    {
                                        attacker("player_attack2", 3);
                                    }
                                    break;
                                case 3:
                                    if (animator.GetBool("isSkill3") == true)
                                    {
                                        attacker("player_attack3_skill", 1);
                                    }
                                    else
                                    {
                                        attacker("player_attack3", 1);
                                    }
                                    break;
                            }
                        }

                    }

                    //스킬1
                    if ((Input.GetButton("Skill1")) && !(animator.GetBool("isSkill1")) && !(animator.GetBool("isSkill2")) && !(animator.GetBool("isSkill3On"))
                        && (animator.GetBool("isAttack") == false) && !isCoolTime1 && (GameManager.Instance.getMp() >= 10))
                    {
                        GameManager.Instance.DecreaseMP(10);
                        animator.SetBool("isSkill1", true);
                        coolTime1_start = Time.time;
                        Skill_1slot.CoolTime = coolTime1_skill;
                        Skill_1slot.UseSkill();
                    }

                    //스킬2
                    if (Input.GetButton("Skill2") && !(animator.GetBool("isSkill1")) && !(animator.GetBool("isSkill2")) && !(animator.GetBool("isSkill3On"))
                        && (animator.GetBool("isAttack") == false) && !isCoolTime2 && (GameManager.Instance.getMp() >= 20))
                    {
                        GameManager.Instance.DecreaseMP(20);
                        animator.SetBool("isSkill2", true);
                        coolTime2_start = Time.time;
                        Skill_2slot.CoolTime = coolTime2_skill;
                        Skill_2slot.UseSkill();
                    }

                    //스킬3
                    if (Input.GetButton("Skill3") && !(animator.GetBool("isSkill1")) && !(animator.GetBool("isSkill2")) && !(animator.GetBool("isSkill3On"))
                        && (animator.GetBool("isAttack") == false) && !isCoolTime3 && (GameManager.Instance.getMp() >= 30))
                    {
                        GameManager.Instance.DecreaseMP(30);
                        //E검기 데미지는 bulletManager 참고 
                        animator.SetBool("isSkill3On", true);
                        animator.SetBool("isSkill3", true);
                        coolTime3_start = Time.time;
                        Skill_3slot.CoolTime = coolTime3_skill;
                        Skill_3slot.UseSkill();
                    }
                }
                //isGrounded
                else
                {
                    animator.SetBool("isGround", false);
                }

                //이동
                if (Input.GetButton("Horizontal") && !(animator.GetBool("isSkill1")) && !(animator.GetBool("isSkill2")) && !(animator.GetBool("isSkill3On"))
                    && !animator.GetBool("isAttack"))
                {
                    //오른쪽
                    if (H_input == 1)
                    {
                        //벽이 있는지 체크
                        if (Physics2D.OverlapBox(new Vector2(transform.position.x + 0.229166675f, transform.position.y), new Vector2(0.45833335f, 1.4f), 0, LayerMask.GetMask("Floor")))
                        {

                        }
                        else
                        {
                            transform.Translate(Vector2.right * H_input * movementSpeed);
                        }
                    }
                    //왼쪽
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
            }
            else
            {
                //player_jump2 애니메이션이 실행중일때 isFall을 true로
                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump2") || animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump2_skill"))
                    && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.0f)
                {
                    animator.SetBool("isFall", true);
                }
                if (isGrounded)
                {
                    animator.SetBool("isGround", true);
                    animator.SetBool("isJump", false);

                    //player_jump3 애니메이션 출력 완료시 isFall false
                    if ((animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump3") || animator.GetCurrentAnimatorStateInfo(0).IsName("player_jump3_skill"))
                        && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        animator.SetBool("isFall", false);
                    }
                }
            }
        }
        //바닥 체크
        if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.8134f), new Vector2(0.6f, 0.1f), 0, LayerMask.GetMask("Floor")))
        {
            isGrounded = true;

            rigidBody.drag = 2;
            rigidBody.gravityScale = 5;
        }
        else
        {
            isGrounded = false;
        }
    }

    //animation 진행 0%~50% 일때 isAttack을 true로 만들고 attackCount를 next로 만듬
    void attacker(string animation, int next)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animation) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
                animator.SetBool("isAttack", true);
                animator.SetInteger("attackCount", next);
        }
    }

    void AttackDamage(int nowAttack)
    {
        int damage = 0;
        Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(AttackBox.transform.position, AttackBox.transform.localScale, 0, LayerMask.GetMask("Enemy"));
        foreach (Collider2D collider in hitEnemy)
        {
            switch (nowAttack)
            {
                case 1:
                    damage = GameManager.Instance.Return1AD(); 
                    break;
                case 2:
                    damage = GameManager.Instance.Return2AD();
                    break;
                case 3:
                    damage = GameManager.Instance.Return3AD(); 
                    break;
            }
            collider.gameObject.GetComponent<enemyManager>().enemyDamaged(damage);
        }
    }
    void isAttackFalse()
    {
        animator.SetBool("isAttack", false);
    }

    void isJumpFalse()
    {
        animator.SetBool("isJump", false);
    }

    void isFallFalse()
    {
        animator.SetBool("isFall", false);
    }
    void attackEnd()
    {
        animator.SetBool("isAttack", false);
        animator.SetInteger("attackCount", 1);
    }

    void QskillDamage()
    {
        //hitEnemy = OverlapBox 안에 있는 Enemy 레이어의 모든 오브젝트
        Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(Qbox.transform.position, Qbox.transform.localScale, 0, LayerMask.GetMask("Enemy"));
        //hitEnemy 안에 있는 모든 오브젝트에게 enemyDamaged 실시
        foreach (Collider2D collider in hitEnemy)
        {
            collider.gameObject.GetComponent<enemyManager>().enemyDamaged(GameManager.Instance.ReturnSlash());
        }
    }
    //enemy 데미지 받게하는 함수 
    void attackDamage(int damage, GameObject Range)
    {
        //hitEnemy = OverlapBox 안에 있는 Enemy 레이어의 모든 오브젝트
        Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(Range.transform.position, Range.transform.localScale, 0, LayerMask.GetMask("Enemy"));
        //hitEnemy 안에 있는 모든 오브젝트에게 enemyDamaged 실시
        foreach (Collider2D collider in hitEnemy)
        {
            collider.gameObject.GetComponent<enemyManager>().enemyDamaged(damage);

        }
    }

    void skill3_Off()
    {
        animator.SetBool("isSkill3", false);
        animator.SetBool("isAttack", false);
        animator.SetInteger("attackCount", 1);
    }

    //OnCollisionEnter2D - 현재 오브젝트가 다른 오브젝트의 콜라이더2d와 닿을때 호출
    //Enemy 태그를 가진 오브젝트와 충돌시 onDamaged
    void OnCollisionEnter2D(Collision2D other)
    {
        if(gameObject.layer == 7)
        {
            if (other.gameObject.tag == "Enemy" && other.gameObject.layer == 8)
            {
                onDamaged(other.transform.position.x, 10);
            }
        }
        if(gameObject.layer == 11) // W스킬 데미지
        {
            other.gameObject.GetComponent<enemyManager>().enemyDamaged(GameManager.Instance.ReturnRush());
            rigidBody.velocity = Vector2.zero;
        }       

    }
    //몬스터와 반대방향으로 튀어오르고 레이어 변경 - 일정시간 후 OffDamaged 함수 호출 !추후 데미지 추가 
    public void onDamaged(float Enemy_X,int Enemy_damage)
    {
        int dirc = transform.position.x - Enemy_X > 0 ? 1 : -1;
        gameObject.layer = 9;
        //무적색상 변경
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        rigidBody.AddForce(new Vector2(dirc * 7, 5), ForceMode2D.Impulse);
        //무적시간
        Invoke("OffDamaged", 1);

        GameManager.Instance.PlayerDamage(Enemy_damage);
    }
    //레이어,캐릭터 색상 변경
    void OffDamaged()
    {
        gameObject.layer = 7;
        
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    void fire(int a)
    {
        switch (a)
        {
            case 1:
                Instantiate(Bullet1, new Vector2(transform.position.x,transform.position.y + 0.3f), transform.rotation);
                break;
            case 2:
                Instantiate(Bullet2, transform.position, transform.rotation);
                break;
            case 3:
                Instantiate(Bullet3, new Vector2(transform.position.x, transform.position.y + 0.3f), transform.rotation);
                break;
            default:
                break;
        }
        
    }
    //스킬2 이동
    void skill2Move()
    {
        int dirc = spriteRenderer.flipX == true ? -1 : 1;
        rigidBody.AddForce(new Vector2(dirc * skill2_movement, 0), ForceMode2D.Impulse);
        gameObject.layer = 11;
    }

    void skill1End()
    {
        animator.SetBool("isAttack", false);
        animator.SetBool("isSkill1", false);
    }

    void skill2End()
    {
        animator.SetBool("isAttack", false);
        animator.SetBool("isSkill2", false);
        rigidBody.velocity = Vector2.zero;
        gameObject.layer = 7;
    }

    void skill3End()
    {
        animator.SetBool("isSkill3On", false);
        Invoke("skill3_Off", 5);
    }
    //쿨타임
    void checkCool1()
    {
        coolTime1_current = Time.time - coolTime1_start;
        if (coolTime1_start == 0)
        {
            isCoolTime1 = false;
        }
        else
        {
            if (coolTime1_current < coolTime1_skill)
            {
                isCoolTime1 = true;
            }
            else
            {
                isCoolTime1 = false;
            }
        } 
    }

    void checkCool2()
    {
        coolTime2_current = Time.time - coolTime2_start;
        if (coolTime2_start == 0)
        {
            isCoolTime2 = false;
        }
        else
        {
            if (coolTime2_current < coolTime2_skill)
            {
                isCoolTime2 = true;
            }
            else
            {
                isCoolTime2 = false;
            }
        }
    }

    void checkCool3()
    {
        coolTime3_current = Time.time - coolTime3_start;
        if(coolTime3_start == 0)
        {
            isCoolTime3 = false;
        }
        else
        {
            if (coolTime3_current < coolTime3_skill)
            {
                isCoolTime3 = true;
            }
            else
            {
                isCoolTime3 = false;
            }
        }
    }

    void ting()
    {
        Application.Quit();
    }
    //기즈모
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(AttackBox.transform.position, new Vector2(AttackBox.transform.localScale.x, AttackBox.transform.localScale.y));
        Gizmos.DrawWireCube(Qbox.transform.position, new Vector2(Qbox.transform.localScale.x, Qbox.transform.localScale.y));
    }


}