using System;
using System.Collections;
using Data;
using UnityEngine;

public class Projectile : BaseObject
{
    SpriteRenderer spriteRenderer;
    public Creature Owner { get; private set; }
    public SkillBase Skill { get; private set; }
    public ProjectileData ProjectileData { get; private set; }
    public ProjectileMotionBase ProjectileMotion { get; private set; }

    void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.GetComponent<BaseObject>();
        if (target.IsValid() == false)
            return;

        // TODO
        target.OnDamaged(Owner, Skill);
        Managers.Object.Despawn(this);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = Define.EObjectType.Projectile;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = SortingLayers.PROJECTILE;

        return true;
    }

    public void SetInfo(int dataTemplateID)
    {
        ProjectileData = Managers.Data.ProjectileDic[dataTemplateID];
        spriteRenderer.sprite = Managers.Resource.Load<Sprite>(ProjectileData.ProjectileSpriteName);

        if (spriteRenderer.sprite == null)
        {
            Debug.LogWarning($"Projectile Sprite Missing {ProjectileData.ProjectileSpriteName}");
        }
    }

    public void SetSpawnInfo(Creature owner, SkillBase skill, LayerMask layer)
    {
        Owner = owner;
        Skill = skill;

        // Rule
        Collider.excludeLayers = layer;

        if (ProjectileMotion != null)
            Destroy(ProjectileMotion);

        var componentName = skill.SkillData.ComponentName;
        ProjectileMotion = gameObject.AddComponent(Type.GetType(componentName)) as ProjectileMotionBase;

        var straightMotion = ProjectileMotion as StraightMotion;
        if (straightMotion != null)
        {
            straightMotion.SetInfo(ProjectileData.DataId, owner.CenterPosition, owner.Target.CenterPosition,
                () => { Managers.Object.Despawn(this); });
        }

        var parabolaMotion = ProjectileMotion as ParabolaMotion;
        if (parabolaMotion != null)
        {
            parabolaMotion.SetInfo(ProjectileData.DataId, owner.CenterPosition, owner.Target.CenterPosition,
                () => { Managers.Object.Despawn(this); });
        }

        StartCoroutine(CoReserveDestroy(5.0f));
    }

    IEnumerator CoReserveDestroy(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Managers.Object.Despawn(this);
    }
}
