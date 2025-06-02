using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class RelicManager
{ 
    public void Init()
    {
        if(BaseManager.Item.SetItemCheck("Dice")) { Delegate_Holder.OnMonsterDead += Dice; }
        if(BaseManager.Item.SetItemCheck("Axe")) { Delegate_Holder.OnPlayerAttack += Axe; }
        if(BaseManager.Item.SetItemCheck("Mana")) { Delegate_Holder.OnPlayerDameged += Mana; }
        
    }

    public void Dice(Monster monster)
    {
        Item_Scriptable itemData = BaseManager.Data.ItemData["Dice"]; // Dice 아이템의 정보를 가져옴.
        Info itemInfo = BaseManager.Data.ItemInfos["Dice"]; // Dice 아이템의 Info를 가져옴.
        
        float dropChance = itemData.ItemChance * ((itemInfo.Level + 1) / 3 + 1);// 추가 drop 확률 계산

        float dropeValue = Random.Range(0.0f, 100.0f);

        if (dropeValue <= dropChance) // 확률이 일치하면 골드 50% 추가 획득
        {
            BaseManager.Pool.Pooling_OBJ("COIN_PARENT").Get((value) =>
            {
                value.GetComponent<COIN_PARENT>().Init(monster.transform.position, 0.5f);  // 현재 스테이지의 MONEY() 값의 50%를 추가로 획득
            });            
        }
    }

    public void Axe(Player player, Monster monster)
    {
        if(monster == null || monster.isDead) return; // 몬스터가 없거나 죽은 경우 리턴

        Info itemInfo = BaseManager.Data.ItemInfos["Axe"];

        float Axe = Mathf.Clamp01(0.2f + (itemInfo.Level) * 0.033f); // Axe 아이템의 레벨에 따른 확률 계산 0.2f + (레벨 * 0.033f)

        BaseManager.Pool.Pooling_OBJ("Attack_Helper").Get((value) =>
        {
            value.transform.position = monster.transform.position;
            value.GetComponent<Bullet>().Attack_Init(monster.transform, player.ATK * Axe, true);
        });
    }

    public void Mana(Player player)
    {
        Info itemInfo = BaseManager.Data.ItemInfos["Mana"];
        
        float mana = 3.0f * Mathf.Clamp01(0.2f + (itemInfo.Level) * 0.033f);

        player.GetMp(mana);
    }
}
