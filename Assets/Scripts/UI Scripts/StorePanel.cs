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
        _AddPortionA1.onClick.AddListener(() => BuyItem(0, 100));
        _AddPortionB1.onClick.AddListener(() => BuyItem(1, 80));
        _AddPortionA2.onClick.AddListener(() => BuyItem(2, 1100));
        _AddPortionB2.onClick.AddListener(() => BuyItem(3, 880));
    }

    public void BuyItem(int num, int price)
    {
        if(GameManager.Instance.getGold() >= price)
        {
            _inventory.Add(_itemDataArray[num]);
            GameManager.Instance.payGold(price);
        }
    }

}