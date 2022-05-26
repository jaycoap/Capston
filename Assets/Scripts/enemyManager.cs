using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class enemyManager : MonoBehaviour
{
    public static enemyManager Instance { get { return _instance; } }
    private static enemyManager _instance;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator animator;
    [SerializeField] GameObject hudDamageText;
    [SerializeField] Transform hudPos;
   
    private Vector2 playerDetectRange;
    private Vector2 meleeAttackRange;
    private Vector2 meleeAttack;
    private int maxHp;
    private bool isGrounded;
    //enemy의 체력과 AI SerializeField로 유니티 내부에서 조작 가능 
    [SerializeField] private Slider enemySlider;
    [SerializeField] private int enemyHp = 0;
    [SerializeField] private string enemyAI = "";
    [SerializeField] private Transform pos;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            return;
        }

        _instance = this;
    }
    void Start()
    {
        maxHp = enemyHp;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerDetectRange = new Vector2(10,10);
        meleeAttackRange = new Vector2(1, 1);
        enemyAI_Control();
        meleeAttack = new Vector2(0.5632785f, 0.7097316f);
    }

    private void Update()
    {
        
        //방향전환시 공격범위 변경
        if (spriteRenderer.flipX == true)
        {
            pos.position = new Vector2(transform.position.x + 0.84491775f, pos.position.y);
        }
        else
        {
            pos.position = new Vector2(transform.position.x - 0.84491775f, pos.position.y);
        }
    }
    void FixedUpdate()
    {
        if (isGrounded)
        {
            animator.SetBool("isGround", true);
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("slime_jump2")) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                animator.SetBool("isFall", true);
            }
            //착지시 player_jump3 애니메이션 출력
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("slime_jump3")) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                animator.SetBool("isFall", false);
            }
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("slime_attack")) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                animator.SetBool("isAttack", false);
            }
        }
        else
        {
            //점프 시작 애니메이션 완료시 
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("slime_jump1") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)))
            {
                animator.SetBool("isJump", false);
            }
            //착지 애니메이션 완료시
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("slime_jump3") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
            {
                animator.SetBool("isFall", false);
            }
            animator.SetBool("isGround", false);
        }
        if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.8333f), new Vector2(0.7f, 0.1f), 0, LayerMask.GetMask("Floor")))
        {
            isGrounded = true;

            rigidBody.drag = 3;
            rigidBody.gravityScale = 1;
        }
        else
        {
            isGrounded = false;
        }
    }
    //적이 데미지 받는 함수
    public void enemyDamaged(int damage){
        setEnemyHp(enemyHp - damage);
        if(enemyHp  <= 0){
            Destroy(gameObject);
        }
        TakeDamage(damage);
        enemyHpBar();
    }
    
    public int getEnemyHp(){
        return enemyHp;
    }

    public int setEnemyHp(int X){
        enemyHp = X;
        return enemyHp;
    }
    //AI 컨트롤러
    public void enemyAI_Control()
    {
        
        //몬스터 종류별 개별 AI
        switch(enemyAI){
            case "slime":
                StartCoroutine("slimeAI", 2);
                break;
            default:
                break;

        }


    }

    public void enemyHpBar()
    {
        enemySlider.maxValue = maxHp;
        enemySlider.value = enemyHp;
        enemySlider.gameObject.SetActive(true);
        enemySlider.StopAllCoroutines();
        enemySlider.StartCoroutine(WaitCoroutine());
    }

    public void TakeDamage(int damage)
    {
        GameObject hudText = Instantiate(hudDamageText);
        hudText.transform.position = hudPos.position + new Vector3(0, 1, 0);
        hudText.GetComponent<DamageText>().setDmgText(damage);
    }

    IEnumerator slimeAI()
    {
        yield return new WaitForSeconds(2);
        //10 , 10 범위에 플레이어를 감지 
        if (isGrounded)
        {
            if (!animator.GetBool("isAttack"))
            {
                if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y), playerDetectRange, 0, LayerMask.GetMask("Player")))
                {
                    //근접공격 범위감지
                    if (Physics2D.OverlapBox(pos.position, meleeAttackRange, 0, LayerMask.GetMask("Player")))
                    {
                        //근접공격
                        Collider2D Player = Physics2D.OverlapBox(pos.position, meleeAttackRange, 0, LayerMask.GetMask("Player"));
                        animator.SetBool("isAttack", true);

                        Player.gameObject.GetComponent<playerManager>().onDamaged(transform.position.x, 50);
                    }
                    else
                    {
                        //점프 이동
                        Collider2D Player = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y), playerDetectRange, 0, LayerMask.GetMask("Player"));
                        int dirc = transform.position.x - Player.transform.position.x > 0 ? -1 : 1;
                        if (dirc == 1)
                        {
                            spriteRenderer.flipX = true;
                        }
                        else
                        {
                            spriteRenderer.flipX = false;
                        }
                        rigidBody.AddForce(new Vector2(dirc * 6f, 7), ForceMode2D.Impulse);
                        animator.SetBool("isJump", true);
                        //방향전환시 스프라이트 뒤집기

                        Debug.Log("jump");

                    }
                }
            }
        }
        
        StartCoroutine("slimeAI", 2);
    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(3f);
        enemySlider.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, meleeAttack);
    }
}
