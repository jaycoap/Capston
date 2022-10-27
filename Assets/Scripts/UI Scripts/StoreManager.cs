using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{


    public Button StorePotion1;
    public Button StorePotion2;
    public Button StorePotion3;
    public Button StoreItem;



    public void BuyPotion1()
    {
        GameManager.Instance.payGold(100);
    }
    public void BuyPotion2()
    {
        GameManager.Instance.payGold(80);
    }
    public void BuyPotion3()
    {
        GameManager.Instance.payGold(1100);
    }

    public void BuyItem()
    {
        GameManager.Instance.payGold(8000);
    }

}
