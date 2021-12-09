using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class enemyManager : MonoBehaviour
{
    public static enemyManager Instance { get { return _instance; } }
    private static enemyManager _instance;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator animator;
    [SerializeField]
    private int enemyHp = 0;
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
        
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        enemyHp = 50;
    }

    public void enemyDamaged(int damage){
        setEnemyHp(enemyHp - damage);
    }
    
    public int getEnemyHp(){
        return enemyHp;
    }

    public int setEnemyHp(int X){
        enemyHp = X;
        return enemyHp;
    }
}
