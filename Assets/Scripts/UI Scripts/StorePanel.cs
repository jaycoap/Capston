using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePanel : MonoBehaviour
{
    public Inventory _inventory;

    public ItemData[] _itemDataArray;

    [Space(4)]
    public Button _AddPortionA1;
    public Button _AddPortionB1;
    public Button _AddPortionA2;
    public Button _AddPortionB2;

    private void Start()
    {
        _AddPortionA1.onClick.AddListener(() => _inventory.Add(_itemDataArray[0]));
        _AddPortionB1.onClick.AddListener(() => _inventory.Add(_itemDataArray[1]));
        _AddPortionA2.onClick.AddListener(() => _inventory.Add(_itemDataArray[2]));
        _AddPortionB2.onClick.AddListener(() => _inventory.Add(_itemDataArray[3]));
    }

}