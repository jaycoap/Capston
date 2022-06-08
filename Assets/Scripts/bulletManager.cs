using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletManager : MonoBehaviour
{
    private float rotation = 0;
    SpriteRenderer spriteRenderer;
    Animator animator;
    GameManager GM;
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (playerManager.flipx)
        {
            rotation = -1;
            spriteRenderer.flipX = true;
        }
        else
        {
            rotation = 1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.right * rotation * 0.7f);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("bullet1") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)) Destroy(gameObject);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("bullet2") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)) Destroy(gameObject);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("bullet3") && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //E½ºÅ³µ©
            collision.gameObject.GetComponent<enemyManager>().enemyDamaged(3);
        }
            
    }
}
