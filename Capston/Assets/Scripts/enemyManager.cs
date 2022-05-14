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
    private Vector2 playerDetectRange;
    
    [SerializeField] private int enemyHp = 0;
    [SerializeField] private string enemyAI = "";
    [SerializeField] Slider EnemyHpSlider;
    [SerializeField] private GameObject heathlBar;
    private int MaxHp;


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
        MaxHp = enemyHp;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerDetectRange = new Vector2(10,10);

    }
    void FixedUpdate()
    {
        enemyAI_Control();
    }
    public void enemyDamaged(int damage){
        setEnemyHp(enemyHp - damage);
        if(enemyHp  == 0){
            Destroy(gameObject);
        }
        EnemyHpSlider.maxValue = MaxHp;
        EnemyHpSlider.value = enemyHp;
        heathlBar.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(WaitCoroutine());
    }
    
    public int getEnemyHp(){
        return enemyHp;
    }

    public int setEnemyHp(int X){
        enemyHp = X;
        return enemyHp;
    }

    public void enemyAI_Control()
    {
        if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y), playerDetectRange, 0, LayerMask.GetMask("Player")))
        {
            Collider2D Player = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y), playerDetectRange, 0, LayerMask.GetMask("Player"));
            float direction = Player.transform.position.x - transform.position.x;
            transform.Translate(Vector2.right * direction * 0.025f);
        }
        switch (enemyAI)
        {
            case "slime":

            default:
                return;

        }
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(3f);
        heathlBar.SetActive(false);
    }
}
