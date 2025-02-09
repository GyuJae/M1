using System;
using System.Collections.Generic;

public class SkillComponent : InitBase
{
    Creature owner;
    public List<SkillBase> SkillList { get; } = new();

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetInfo(Creature owner, List<int> skillTemplateIDs)
    {
        this.owner = owner;

        foreach (var skillTemplateID in skillTemplateIDs)
            AddSkill(skillTemplateID);
    }

    void AddSkill(int skillTemplateID = 0)
    {
        var className = Managers.Data.SkillDic[skillTemplateID].ClassName;

        var skill = gameObject.AddComponent(Type.GetType(className)) as SkillBase;
        if (skill == null)
            return;

        skill.SetInfo(owner, skillTemplateID);
        SkillList.Add(skill);
    }

    public SkillBase GetReadySkill()
    {
        // TEMP
        return SkillList[0];
    }
}
