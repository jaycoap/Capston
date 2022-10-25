using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int Price;
    Rigidbody2D rigidbody;
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPrice(int setPrice)
    {
        Price = setPrice;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (gameObject.layer == 13)
        {
            if (other.gameObject.tag == "Player")
            {
                gm.setGold(Price);
                Destroy(gameObject);
            }
        } 
    }
}
