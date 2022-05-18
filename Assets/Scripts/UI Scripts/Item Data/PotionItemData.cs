using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    /// <summary> 소비 아이템 정보 </summary>
    [CreateAssetMenu(fileName = "Item_Potion_", menuName = "Inventory System/Item Data/Potion", order = 3)]
    public class PotionItemData : CountableItemData
    {
        /// <summary> 효과량(회복량 등) </summary>
        public int Value => _value;
        public string Type => _type;
        [SerializeField] private string _type;
        [SerializeField] private int _value;
        public override Item CreateItem()
        {
            return new PotionItem(this);
        }
    }
