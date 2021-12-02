using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTester : MonoBehaviour
{
    public Inventory _inventory;

    public ItemData[] _itemDataArray;

    [Space(12)]
    public Button _removeAllButton;

    [Space(8)]
    public Button _AddPortionA1;
    public Button _AddPortionA50;
    public Button _AddPortionB1;
    public Button _AddPortionB50;
    public Button _AddPortionA2;
    public Button _AddPortionA2add50;
    public Button _AddPortionB2;
    public Button _AddPortionB2add50;

    private void Start()
    {
        _removeAllButton.onClick.AddListener(() =>
        {
            int capacity = _inventory.Capacity;
            for (int i = 0; i < capacity; i++)
                _inventory.Remove(i);
        });

        _AddPortionA1.onClick.AddListener(() => _inventory.Add(_itemDataArray[0]));
        _AddPortionA50.onClick.AddListener(() => _inventory.Add(_itemDataArray[0], 50));
        _AddPortionB1.onClick.AddListener(() => _inventory.Add(_itemDataArray[1]));
        _AddPortionB50.onClick.AddListener(() => _inventory.Add(_itemDataArray[1], 50));
        _AddPortionA2.onClick.AddListener(() => _inventory.Add(_itemDataArray[2]));
        _AddPortionA2add50.onClick.AddListener(() => _inventory.Add(_itemDataArray[2], 50));
        _AddPortionB2.onClick.AddListener(() => _inventory.Add(_itemDataArray[3]));
        _AddPortionB2add50.onClick.AddListener(() => _inventory.Add(_itemDataArray[3], 50));
    }

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.G))
    //    {
    //        GameManager.Instance.getexp(100);
    //    }
    //    GameManager.Instance.setLevel();
    //}
}
