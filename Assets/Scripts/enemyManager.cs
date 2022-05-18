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
    private int maxHp;
    //enemy의 체력과 AI SerializeField로 유니티 내부에서 조작 가능 
    [SerializeField] private Slider enemySlider;
    [SerializeField] private int enemyHp = 0;
    [SerializeField] private string enemyAI = "";


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
    }
    void FixedUpdate()
    {
        enemyAI_Control();
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
        //캐릭터 식별시 이동
        if(Physics2D.OverlapBox(new Vector2(transform.position.x,transform.position.y),playerDetectRange,0,LayerMask.GetMask("Player")))
        {
            Collider2D Player = Physics2D.OverlapBox(new Vector2(transform.position.x,transform.position.y),playerDetectRange,0,LayerMask.GetMask("Player"));
            float direction = Player.transform.position.x - transform.position.x;
            transform.Translate(Vector2.right * direction * 0.025f);
        }
        //몬스터 종류별 개별 AI
        switch(enemyAI){
            case "slime":

            default:
                    return;

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

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(3f);
        enemySlider.gameObject.SetActive(false);
    }
}
