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
        Item_Scriptable itemData = BaseManager.Data.ItemData["Dice"]; // Dice �������� ������ ������.
        Info itemInfo = BaseManager.Data.ItemInfos["Dice"]; // Dice �������� Info�� ������.
        
        float dropChance = itemData.ItemChance * ((itemInfo.Level + 1) / 3 + 1);// �߰� drop Ȯ�� ���

        float dropeValue = Random.Range(0.0f, 100.0f);

        if (dropeValue <= dropChance) // Ȯ���� ��ġ�ϸ� ��� 50% �߰� ȹ��
        {
            BaseManager.Pool.Pooling_OBJ("COIN_PARENT").Get((value) =>
            {
                value.GetComponent<COIN_PARENT>().Init(monster.transform.position, 0.5f);  // ���� ���������� MONEY() ���� 50%�� �߰��� ȹ��
            });            
        }
    }

    public void Axe(Player player, Monster monster)
    {
        if(monster == null || monster.isDead) return; // ���Ͱ� ���ų� ���� ��� ����

        Info itemInfo = BaseManager.Data.ItemInfos["Axe"];

        float Axe = Mathf.Clamp01(0.2f + (itemInfo.Level) * 0.033f); // Axe �������� ������ ���� Ȯ�� ��� 0.2f + (���� * 0.033f)

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
