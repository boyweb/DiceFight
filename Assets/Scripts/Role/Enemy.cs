using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Role
{
    public void NextTurn()
    {
        AttackValue = Random.Range(5, 11);
        AttackTimes = Random.Range(1, 7);
        Defence = Random.Range(0, 10);
        Regeneration = Random.Range(0, 10);
    }
}