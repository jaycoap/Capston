using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class slimeBossBulletManager : MonoBehaviour
{
    Collider2D slimeBoss;
    Collider2D player;
    Rigidbody2D rigidBody;
    [SerializeField] string bulletType;

    //발사 가속도
    [SerializeField] float bulletPower;
    //가시공 파괴시간
    [SerializeField] float spikeBallDestroyTime;

    [SerializeField] GameObject poisonFloor;
    [SerializeField] GameObject poison;
    //독 장판 poisonFloorTime -> 독데미지 틱 관리 poisonFloorDestroyTime -> 독장판 파괴 시간
    [SerializeField] float poisonFloorTime;
    [SerializeField] float poisonFloorDestroyTime;
    [SerializeField] int poisonDamage;
    //발사각도 최소,최대
    [SerializeField] int minAngle;
    [SerializeField] int maxAngle;

    bool isFlip;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        slimeBoss = Physics2D.OverlapBox(new Vector2(transform.position.x,transform.position.y), new Vector2(1,1), 0, LayerMask.GetMask("Enemy"));
        if(!(slimeBoss == null)) isFlip = slimeBoss.GetComponent<SpriteRenderer>().flipX;
        
        switch (bulletType)
        {
            case "spikeBall":
                backShotBullet();
                Invoke("Destroy", spikeBallDestroyTime);
                break;
            case "poisonBall":
                backShotBullet();
                Invoke("Destroy", 5);
                break;
            case "poisonFloor":
                StartCoroutine("PoisonFloor", poisonFloorTime);
                Invoke("Destroy", poisonFloorDestroyTime);
                break;
        }
    }


    void Update()
    {
        
    }

    void backShotBullet()
    {
        int dirc;
        if (isFlip)
        {
            dirc = 1;
        }
        else
        {
            dirc = -1;
        }
        float angle = (Mathf.PI) * Random.Range(minAngle, maxAngle + 1) / 180;
        rigidBody.AddForce(new Vector2(dirc * Mathf.Cos(angle) * bulletPower, Mathf.Sin(angle) * bulletPower), ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        switch (bulletType)
        {
            case "spikeBall":
                if (other.gameObject.tag == "Player" && other.gameObject.layer == 7)
                {
                    other.gameObject.GetComponent<playerManager>().onDamaged(transform.position.x, 10);
                }
                if (other.gameObject.layer == 6)
                {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, -rigidBody.velocity.y +10);
                }
                break;
            case "poisonBall":
                if (other.gameObject.layer == 6)
                {
                    Instantiate(poisonFloor, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                    Destroy(gameObject);
                }
                break;
        }
    }

    IEnumerator PoisonFloor()
    {
        yield return new WaitForSeconds(poisonFloorTime);
        player = Physics2D.OverlapBox(transform.position, poison.transform.localScale, 0, LayerMask.GetMask("Player"));
        if(!(player == null))
        {
            GameManager.Instance.PlayerDamage(poisonDamage);
        }
        StartCoroutine("PoisonFloor", poisonFloorTime);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(bulletType == "poisonFloor")
        {
            Gizmos.DrawWireCube(transform.position, poison.transform.localScale);
        }
    }
}
