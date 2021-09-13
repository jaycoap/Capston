using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject InventoryPanel;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryPanel.activeSelf == false)
            {
                InventoryPanel.SetActive(true);
            }
            else
            {
                InventoryPanel.SetActive(false);
            }
        }
    }
}
