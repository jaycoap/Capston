using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public static Interaction instance;
    private Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Potal")
        {
            if (Input.GetKey(KeyCode.G))
            {
                GameObject.Find("Dynamic UI").transform.Find("Stage Panel").gameObject.SetActive(true);
            }
        }

        if (other.gameObject.tag == "Store")
        {
            Debug.Log("ªÛ¡° ¡¢√À");
            if (Input.GetKey(KeyCode.G))
            {
                GameObject.Find("Dynamic UI").transform.Find("Store Panel").gameObject.SetActive(true);
                GameObject.Find("Dynamic UI").transform.Find("Inventory Panel").gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Store")
        {
            GameObject.Find("Dynamic UI").transform.Find("Store Panel").gameObject.SetActive(false);
            GameObject.Find("Dynamic UI").transform.Find("Inventory Panel").gameObject.SetActive(false);
        }
    }
}
