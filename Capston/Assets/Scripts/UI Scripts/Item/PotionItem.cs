using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    /// <summary> 수량 아이템 - 포션 아이템 </summary>
    public class PotionItem : CountableItem, IUsableItem
    {
         public PotionItemData PotionData { get; private set; }
         public PotionItem(PotionItemData data, int amount = 1) : base(data, amount) 
         {
            PotionData = data;
         }
        public int PotionHeal => PotionData.Value;
        public string PotionType => PotionData.Type;

    public bool Use()
        {

        switch (PotionType)
        {
            case "HP":
                GameManager.Instance.usePotionHealHP(PotionHeal);
                break;
            case "MP":
                GameManager.Instance.usePotionHealMP(PotionHeal);
                break;
        }
            Amount--;
            return true;
        }

        protected override CountableItem Clone(int amount)
        {
            return new PotionItem(CountableData as PotionItemData, amount);
        }
    }
