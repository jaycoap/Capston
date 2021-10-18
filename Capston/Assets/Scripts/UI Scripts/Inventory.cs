using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject InventoryPanel;
    bool ActiveInventory = false;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            {
                ActiveInventory = !ActiveInventory;
                InventoryPanel.SetActive(ActiveInventory);
            }
        }
    }
}
