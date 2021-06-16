using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
    #region Properties

    public int HitPoint { get; set; }
    public int MaxHitPoint { get; set; }
    public int AttackValue { get; set; }
    public int AttackTimes { get; set; }
    public int Defence { get; set; }
    public int Regeneration { get; set; }

    #endregion

    #region Status

    public bool IsDead { get; set; }

    #endregion

    #region Fight Actions

    public void DeadAction()
    {
        IsDead = true;
    }

    public void HurtAction(int hurt)
    {
        HitPoint = Math.Max(0, HitPoint - hurt);
        if (HitPoint <= 0) DeadAction();
    }

    public int RegenerationAction()
    {
        var regeneration = Math.Min(Regeneration, MaxHitPoint - HitPoint);
        HitPoint += regeneration;

        return regeneration;
    }

    /// <summary>
    /// 攻击逻辑
    /// 1. 回复血量
    /// 2. 攻击 - 防御
    /// </summary>
    /// <param name="role"></param>
    public int AttackAction(Role role)
    {
        var hurt = Math.Max(0, AttackValue - role.Defence);
        if (hurt > 0) role.HurtAction(hurt);

        return hurt;
    }

    #endregion
}