using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private Rigidbody2D rigid;
    [SerializeField] private SceneChange IngameSceneChange;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Potal")
        {
            if (Input.GetKey(KeyCode.G))
            {
                IngameSceneChange.CurrentSceneChange();
            }
        }

        if (other.gameObject.tag == "Store")
        {
            Debug.Log("ªÛ¡° ¡¢√À");
            if (Input.GetKey(KeyCode.G))
            {
                GameObject.Find("Main Canvas").transform.Find("Store Panel").gameObject.SetActive(true);
                GameObject.Find("Main Canvas").transform.Find("Inventory Panel").gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Store")
        {
            GameObject.Find("Main Canvas").transform.Find("Store Panel").gameObject.SetActive(false);
            GameObject.Find("Main Canvas").transform.Find("Inventory Panel").gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            IngameSceneChange.CurrentSceneChange();
        }
    }



}
