using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Main : BaseSkill
{
    float delay = 5.0f;

    private void Start()
    {
        StageManager.m_ReadyEvent += OnReady;
    }

    public override void Set_Skill()
    {
        m_Character.isGetSkill = true;
        m_Character.AnimatorChange("isSKILL");

        var character = HP_Check();
        m_Character.transform.LookAt(character.transform.position);

        character.Heal(SkillDamage(110));
        SkillParticle.gameObject.SetActive(true);
        SkillParticle.transform.position = character.transform.position;
        base.Set_Skill();

        Invoke("ReturnSkill",0.5f);
    }

    public override void ReturnSkill()
    {
        base.ReturnSkill();
        skill_Coroutine = StartCoroutine(SetSkill_Coroutine(delay));        
    }

    public void OnReady()
    {
        skill_Coroutine = StartCoroutine(SetSkill_Coroutine(delay));
    }

    IEnumerator SetSkill_Coroutine(float value)
    {
        float timer = value;
        while (value > 0.0f)
        {
            timer -= Time.deltaTime;
            MainUI.instance.main_HeroSkillFill.fillAmount = timer/value;
            yield return null;
        }
        Set_Skill();
    }
}
