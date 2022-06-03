using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thornManager : MonoBehaviour
{
    Animator animator;
    [SerializeField] private GameObject thorn;
    [SerializeField] private int a;
    [SerializeField] private Transform pos;
    // Start is called before the first frame update
    void Start()
    {
        if (a == 1)
        {
            Invoke("thornSpawn", 0.5f);
        }
        else
        {
            Invoke("thornDestroy", 0.5f);
        }
    }

    
    private void thornSpawn()
    {
        Instantiate(thorn, new Vector2(pos.transform.position.x, pos.transform.position.y), Quaternion.identity);
        Destroy(gameObject);
    }
    private void thornDestroy()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!(a == 1))
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<playerManager>().onDamaged(transform.position.x, 12);
            }
        }
    }
}
