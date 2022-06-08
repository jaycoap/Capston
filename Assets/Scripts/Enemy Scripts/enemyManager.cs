using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyUI
{
    public GameObject hudDamageText;
    public Transform hudPos;
    public Slider enemySlider;
}

[Serializable]
public class EnemyStat
{
    public float movementSpeed;
    public int patternTime;
}

[Serializable]
public class SlimeStat
{
    public bool isBackShot;
}

[Serializable]
public class SlimeBossStat
{
    public float diveCool;
    public float backShotCool;
    public float birthCool;
    public int thornAmount;
    public Vector2 bossDiveRange;
    public int attackDamage;
    public GameObject warning;
    public GameObject spikeBall;
    public GameObject poisonBall;
    public GameObject slime;
    public Transform backShotPos;
    public Transform birthPos;
}

public class enemyManager : MonoBehaviour
{
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator animator;

    public static enemyManager Instance { get { return _instance; } }
    private static enemyManager _instance;
    private int maxHp;
    private bool isGrounded;
    private Vector2 divePosition;
    
    //enemy의 체력과 AI SerializeField로 유니티 내부에서 조작 가능 
    [SerializeField] private int enemyHp = 0;
    [SerializeField] private string enemyAI = "";
    [SerializeField] private Transform pos;
    [SerializeField] private Vector2 AttackRange;
    [SerializeField] private Vector2 playerDetectRange;

    public EnemyUI enemyUI;
    public EnemyStat enemyStat;
    public SlimeBossStat slimeBossStat;
    public SlimeStat slimeStat;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            return;

        _instance = this;
    }

    void Start()
    {
        maxHp = enemyHp;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyAI_Control();
    }

    private void Update()
    {
        int flip = (spriteRenderer.flipX) ? 1: -1;

        //방향전환시 공격범위 변경
        switch (enemyAI)
        {
            //슬라임일 경우
            case "slime":
                pos.position = new Vector2(transform.position.x + flip * 0.84491775f, pos.position.y);
                break;

            // 슬라임 보스 일 경우
            case "slimeBoss":   
                pos.position = new Vector2(transform.position.x + flip * 1.745f, pos.position.y);
                slimeBossStat.backShotPos.position
                        = new Vector2(transform.position.x - flip * 0.863f, slimeBossStat.backShotPos.position.y);
                slimeBossStat.birthPos.position
                        = new Vector2(transform.position.x + flip * 2f, slimeBossStat.birthPos.position.y);

                gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-flip * 0.2f, 0.5f);
                break;
        }
    }

    void FixedUpdate()
    {
        switch (enemyAI)
        {
            // 슬라임일 경우
            case "slime":
                slimeAniControl();
                break;

            // 슬라임 보스일 경우
            case "slimeBoss":
                slimeBossAniControl();

                bool isIdle = 
                        ((animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_dive1")
                             && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f
                             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f)) 

                      && (animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_dive2") 
                             && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f
                             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f))

                      && (animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_attack") 
                             && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f 
                             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f)) 

                      && (animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_shot") 
                             && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f 
                             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f)) 

                      && (animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_backShot") 
                             && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f 
                             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f)));

                //이동
                if (!isIdle
                     && !animator.GetBool("isAttack")
                     && !animator.GetBool("isBackShot")
                     && !animator.GetBool("isDive"))
                {
                    if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y)
                         , slimeBossStat.bossDiveRange, 0, LayerMask.GetMask("Player")))
                    {
                        Collider2D player 
                            = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y)
                             , slimeBossStat.bossDiveRange, 0, LayerMask.GetMask("Player"));
                        
                        int dirc = transform.position.x - player.transform.position.x > 0 ? -1 : 1;

                        transform.position 
                            = new Vector2(transform.position.x + (enemyStat.movementSpeed * dirc), transform.position.y);

                        //방향전환시 스프라이트 뒤집기
                        spriteRenderer.flipX = (dirc == 1) ? true : false;
                    }
                }
                break;
            default:
                break;
        }
        
        if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.8333f)
             , new Vector2(0.7f, 0.1f), 0, LayerMask.GetMask("Floor")))
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
            switch (enemyAI)
            {
                case "slime":
                    animator.SetBool("isDie", true);
                    break;
                case "slimeBoss":
                    animator.SetBool("isDie", true);
                    break;
            }
        }

        TakeDamage(damage);
        enemyHpBar();
    }
    
    public int getEnemyHp()
    {
        return enemyHp;
    }

    public int setEnemyHp(int X)
    {
        enemyHp = X;
        return enemyHp;
    }

    //AI 컨트롤러
    public void enemyAI_Control()
    {
        //몬스터 종류별 개별 AI
        switch(enemyAI)
        {
            case "slime":
                StartCoroutine("slimeAI", 2);
                break;
            case "slimeBoss":
                StartCoroutine("slimeBossAI", 2);
                break;
            default:
                break;

        }
    }

    public void enemyHpBar()
    {
        enemyUI.enemySlider.maxValue = maxHp;
        enemyUI.enemySlider.value = enemyHp;
        enemyUI.enemySlider.gameObject.SetActive(true);
        enemyUI.enemySlider.StopAllCoroutines();
        enemyUI.enemySlider.StartCoroutine(WaitCoroutine());
    }

    public void TakeDamage(int damage)
    {
        GameObject hudText = Instantiate(enemyUI.hudDamageText);
        hudText.transform.position = enemyUI.hudPos.position + new Vector3(0, 1, 0);
        hudText.GetComponent<DamageText>().setDmgText(damage);
    }

    // 슬라임 AI 코루틴
    IEnumerator slimeAI()
    {
        yield return new WaitForSeconds(enemyStat.patternTime);

        // 땅 위에 있을 경우
        if (isGrounded)
        {
            // 공격 중이 아닐 경우
            if (!animator.GetBool("isAttack"))
            {

                if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y)
                , playerDetectRange, 0, LayerMask.GetMask("Player")))
                {
                    //근접공격 범위감지
                    if (Physics2D.OverlapBox(pos.position, AttackRange, 0, LayerMask.GetMask("Player")))
                    {
                        //근접공격
                        Collider2D Player = Physics2D.OverlapBox(pos.position, AttackRange, 0, LayerMask.GetMask("Player"));
                        animator.SetBool("isAttack", true);

                        Player.gameObject.GetComponent<playerManager>().onDamaged(transform.position.x, 40);
                    }
                    else
                    {
                        //점프 이동
                        Collider2D Player
                         = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y)
                            , playerDetectRange, 0, LayerMask.GetMask("Player"));

                        int dirc = transform.position.x - Player.transform.position.x > 0 ? -1 : 1;

                        spriteRenderer.flipX = (dirc == 1) ? true : false;
                        rigidBody.AddForce(new Vector2(dirc * 6f, 7), ForceMode2D.Impulse);
                        animator.SetBool("isJump", true);
                    }
                }
            }
        }
        
        StartCoroutine("slimeAI", enemyStat.patternTime);
    }

    // 슬라임 보스 AI 코루틴
    IEnumerator slimeBossAI()
    {
        yield return new WaitForSeconds(enemyStat.patternTime);

        bool isIdle
         = ((animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_dive1")
                 && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f
                 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f))

             && (animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_dive2")
                 && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f
                 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f ))

             &&(animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_attack")
                 && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f 
                 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f ))

             && (animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_shot") 
                 && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f 
                 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f ))

             && (animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_backShot") 
                 && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f 
                 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0f)));

        Collider2D player 
            = Physics2D.OverlapBox(new Vector2(pos.position.x, pos.position.y)
             , AttackRange, 0, LayerMask.GetMask("Player"));

        if (isGrounded)
        {
            //다이브 패턴
            if (!animator.GetBool("isDive") && animator.GetBool("isDiveCoolOn") && !isIdle)  
            {
                if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y)
                    , slimeBossStat.bossDiveRange, 0, LayerMask.GetMask("Player")))
                {
                    animator.SetBool("isDive", true);
                }
            }

            //곡사포 패턴
            else if (!animator.GetBool("isBackShot") && animator.GetBool("isBackShotCoolOn") && !isIdle) 
            {
                animator.SetBool("isBackShot", true);
            }

            //전방 발사
            else if (!animator.GetBool("isBirth") && animator.GetBool("isBirthCoolOn") && !isIdle)
            {
                animator.SetBool("isBirth", true);
            }

            //근접 공격 패턴
            else if (!animator.GetBool("isAttack") && !isIdle && player == null) 
            {
                animator.SetBool("isAttack", true);
            }
        }
        StartCoroutine("slimeBossAI", enemyStat.patternTime);
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(3f);
        enemyUI.enemySlider.gameObject.SetActive(false);
    }

    private void slimeAniControl()
    {
        if (isGrounded)
        {
            animator.SetBool("isGround", true);
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("slime_jump2"))
             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                animator.SetBool("isFall", true);
            }

            //착지시 player_jump3 애니메이션 출력
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("slime_jump3"))
             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                animator.SetBool("isFall", false);
            }

            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("slime_attack"))
             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                animator.SetBool("isAttack", false);
            }
        }
        else
        {
            //점프 시작 애니메이션 완료시 
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("slime_jump1")
             && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)))
            {
                animator.SetBool("isJump", false);
            }

            //착지 애니메이션 완료시
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("slime_jump3")
             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
            {
                animator.SetBool("isFall", false);
            }

            animator.SetBool("isGround", false);
        }
    }

    private void slimeBossAniControl()
    {
        if ((animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_dive1")
         && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
         && animator.GetBool("isDiveCoolOn") == true))
        {
            divePosition = transform.position;
            transform.position = new Vector2(transform.position.x, transform.position.y - 100);
            animator.SetBool("isDiveCoolOn", false);
            
            for(int i = 0 ; i < slimeBossStat.thornAmount; i++)
            {
                Invoke("slimeBossThorn", i + 0.5f);
            }
     
            Invoke("diveOff", slimeBossStat.thornAmount + 1.5f);
            Invoke("ThornCool", slimeBossStat.diveCool);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_dive2")
         && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
        {
            animator.SetBool("isOut", false);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_attack")
         && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
        {
            animator.SetBool("isAttack", false);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_backShot")
         && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
        {
            animator.SetBool("isBackShot", false);
            Invoke("backShotCool", slimeBossStat.backShotCool);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("slimeBoss_birth")
         && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
        {
            animator.SetBool("isBirth", false);
            Invoke("birthCoolOn", slimeBossStat.birthCool);
        }
    }

    private void diveOff()
    {
        animator.SetBool("isDive", false);
        animator.SetBool("isOut", true);
        transform.position = divePosition;
    }
    private void slimeBossThorn()
    {
        Collider2D player
         = Physics2D.OverlapBox(new Vector2(divePosition.x, divePosition.y)
            , slimeBossStat.bossDiveRange, 0, LayerMask.GetMask("Player"));

        if(!(player == null))
        {
            Instantiate(slimeBossStat.warning
             , new Vector2(player.transform.position.x, divePosition.y), Quaternion.identity);
        }
    }

    private void ThornCool()
    {
        animator.SetBool("isDiveCoolOn", true);
    }

    private void slimeBossAttack()
    {
        Collider2D player
         = Physics2D.OverlapBox(new Vector2(pos.position.x, pos.position.y)
            , AttackRange, 0, LayerMask.GetMask("Player"));

        if(!(player == null))
        {
            player.GetComponent<playerManager>().onDamaged(transform.position.x, slimeBossStat.attackDamage);
        }
    }

    private void backShotCoolOff()
    {
        animator.SetBool("isBackShotCoolOn", false);
    }

    private void backShotCool()
    {
        animator.SetBool("isBackShotCoolOn", true);
    }

    private void birthCoolOff()
    {
        animator.SetBool("isBirthCoolOn", false);
    }

    private void birthCoolOn()
    {
        animator.SetBool("isBirthCoolOn", true);
    }
    
    private void birth()
    {
        Instantiate(slimeBossStat.slime, new Vector2(slimeBossStat.birthPos.position.x, slimeBossStat.birthPos.position.y), Quaternion.identity);
    }

    private void destroy()
    {
        Destroy(gameObject);
    }
    
    private void backShot()
    {
        int kind = Random.Range(1, 3);

        switch (kind)
        {
            case 1:
                Instantiate(slimeBossStat.spikeBall, new Vector2(slimeBossStat.backShotPos.position.x
                    , slimeBossStat.backShotPos.position.y), Quaternion.identity);
                break;

            case 2:
                Instantiate(slimeBossStat.poisonBall, new Vector2(slimeBossStat.backShotPos.position.x
                    , slimeBossStat.backShotPos.position.y), Quaternion.identity);
                break;
 
        }
    }

    private void OnDrawGizmos()
    {
            Gizmos.color = Color.red;
            
            switch (enemyAI)
            {
                case "slime":
                    Gizmos.DrawWireCube(pos.position, AttackRange);
                    break;

                case "slimeBoss":
                    Gizmos.DrawWireCube(pos.position, AttackRange);

                    Gizmos.DrawWireCube(slimeBossStat.birthPos.position, new Vector2(1, 1));
                    break;
            }
    }
}
