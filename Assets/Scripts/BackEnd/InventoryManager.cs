using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using System;

public class InventoryManager : MonoBehaviour
{
    public InventoryManager instance;

    private void Awake()
    {
        { instance = this; }
    }

    public int HpSmallPotion;
    public int HpBigPotion;
    public int MpSmallPotion;
    public int MpBigPotion;
    public int Item;


    public void OnclickInsertInventoryData()
    {
        //int charHpSmall = 여기에 Inventory에서 넘겨주는 값; 
        //int charHpBig = 여기에 Inventory에서 넘겨주는 값; 
        //int charMpSmall = 여기에 Inventory에서 넘겨주는 값; 
        //int charMpBig = 여기에 Inventory에서 넘겨주는 값; 
        //int charItem = 여기에 Inventory에서 넘겨주는 값; 

        Param param = new Param();
        //param.Add("HPSmallPotion", charHpSmall);
        //param.Add("HPBigPotion", charHpBig);
        //param.Add("MPSmallPotion", charMpSmall);
        //param.Add("MPBigPotion", charMpBig);
    }


}
